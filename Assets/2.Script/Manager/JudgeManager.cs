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

    private Transform center;

    private Queue<Note>[] noteQueues = 
    { 
        new Queue<Note>(),
        new Queue<Note>(),
        new Queue<Note>(),
        new Queue<Note>(),
    };

    private JudgeType judgeResult;

    public JudgeType JudgeResult => judgeResult;

    public Queue<Note>[] NoteQueues => noteQueues;

    private void Start()
    {
        center = GameManager.Instance.Center;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsAuto) return;

        for(int i = 0; i < noteQueues.Length; i++)
        {
            Queue<Note> queue = noteQueues[i];

            //요소가 없는 Queue 넘기기
            if(queue.Count == 0) continue;

            if (SoundManager.Instance.SongPosition >= queue.Peek().NoteHitTime)
            {
                JudgeNote(i);
            }
        }
    }

    //노트 판정
    public void JudgeNote(int lineIndex)
    {
        if (noteQueues[lineIndex].Count == 0) return;

        Note currentNote = noteQueues[lineIndex].Peek();

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
                ObjectPoolManager.Instance.SpawnObject(judgeEffect, new Vector2(GameManager.Instance.NoteSpawnTransforms[lineIndex].position.x, center.position.y), Quaternion.identity);
                Debug.Log(new Vector2(GameManager.Instance.NoteSpawnTransforms[lineIndex].position.x, center.position.y));

                //효과음 재생
                SoundManager.Instance.PlaySFX(GameManager.Instance.NoteHitSfx);

                judgeResult = judgeDatas[j].type;
                noteQueues[lineIndex].Dequeue();
                return;
            }
        }
    }

    public void JudgeNoteOutCamera(Note note)
    {
        int index = note.NoteHitLine;

        if (noteQueues[index].Count > 0 && noteQueues[index].Peek() == note)
        {
            noteQueues[index].Dequeue();

            judgeResult = JudgeType.Bad;

            AddressableManager.Instance.ReleaseObject(note.gameObject);
        }
    }
}
