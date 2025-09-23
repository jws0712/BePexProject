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

    //��Ʈ ����
    public void JudgeNote()
    {
        if (noteQueue.Count == 0) return;

        Note currentNote = noteQueue.Peek();

        float notePosY = currentNote.gameObject.transform.position.y;

        for (int j = 0; j < judgeDatas.Length; j++)
        {
            //��Ʈ�� Y ���� ���� �������� �ִ��� Ȯ��
            if ((center.position.y + judgeDatas[j].distance) >= notePosY
             && (center.position.y - judgeDatas[j].distance) <= notePosY)
            {
                //��Ʈ ������Ʈ �޸� ����
                AddressableManager.Instance.ReleaseObject(currentNote.gameObject);

                //����Ʈ ��ȯ
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
