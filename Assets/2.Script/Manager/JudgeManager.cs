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

            //��Ұ� ���� Queue �ѱ��
            if(queue.Count == 0) continue;

            if (SoundManager.Instance.SongPosition >= queue.Peek().NoteHitTime)
            {
                JudgeNote(i);
            }
        }
    }

    //��Ʈ ����
    public void JudgeNote(int lineIndex)
    {
        if (noteQueues[lineIndex].Count == 0) return;

        Note currentNote = noteQueues[lineIndex].Peek();

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
                ObjectPoolManager.Instance.SpawnObject(judgeEffect, new Vector2(GameManager.Instance.NoteSpawnTransforms[lineIndex].position.x, center.position.y), Quaternion.identity);
                Debug.Log(new Vector2(GameManager.Instance.NoteSpawnTransforms[lineIndex].position.x, center.position.y));

                //ȿ���� ���
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
