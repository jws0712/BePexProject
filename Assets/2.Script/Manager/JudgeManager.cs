//System
using System;
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;

public enum JudgeType
{
    Perfect,
    Greate,
    Good,
    Bad,
}

[Serializable]
public struct JudgeData
{
    public JudgeType type;
    public float distance;
}

public class JudgeManager : Singleton<JudgeManager>
{
    [SerializeField] private JudgeData[] judgeDatas;
    [SerializeField] private GameObject judgeEffect;

    private AudioClip sfx;
    private Transform center;
    private Queue<Note> noteQueue = new();
    private JudgeType judgeResult;

    public JudgeType JudgeResult => judgeResult;

    public Queue<Note> NoteQueue => noteQueue;

    private void Start()
    {
        center = GameManager.Instance.Center;
        sfx = GameManager.Instance.NoteHitSfx;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsAuto || noteQueue.Count == 0) return;

        if(SoundManager.Instance.SongPosition > noteQueue.Peek().NoteHitTime)
        {
            JudgeNote();
        }
    }

    //노트 판정
    public void JudgeNote()
    {
        if (noteQueue.Count == 0) return;

        Note currentNote = noteQueue.Peek();

        float notePosY = currentNote.gameObject.transform.position.y;

        for (int j = 0; j < judgeDatas.Length; j++)
        {
            //노트의 Y 값이 판정 범위내에 있는지 확인
            if ((center.position.y + judgeDatas[j].distance) >= notePosY
             && (center.position.y - judgeDatas[j].distance) <= notePosY)
            {
                //노트 오브젝트 메모리 해제
                AddressableManager.Instance.ReleaseObject(currentNote.gameObject);

                //이펙트 소환
                ObjectPoolManager.Instance.SpawnObject(judgeEffect, center.position, Quaternion.identity);

                SoundManager.Instance.PlaySFX(sfx);

                judgeResult = judgeDatas[j].type;
                noteQueue.Dequeue();
                return;
            }
        }
    }

    public void JudgeNoteOutCamera(Note note)
    {
        if(noteQueue.Count > 0 && noteQueue.Peek() == note)
        {
            noteQueue.Dequeue();
            judgeResult = JudgeType.Bad;
            AddressableManager.Instance.ReleaseObject(note.gameObject);
        }
    }
}
