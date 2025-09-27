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
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject judgeEffect;
    [SerializeField] private Transform canvas;

    private int comboCount;

    private Transform center;

    private GameObject currentJudgeEffect;

    private Queue<Note>[] noteQueues = 
    { 
        new Queue<Note>(),
        new Queue<Note>(),
        new Queue<Note>(),
        new Queue<Note>(),
    };

    private JudgeType judgeResult;

    public int ComboCount => comboCount;

    public JudgeType JudgeResult => judgeResult;


    public Queue<Note>[] NoteQueues => noteQueues;

    private void Start()
    {
        center = GameManager.Instance.JudgeLineCenter;
    }

    private void Update()
    {
        NoteHitAuto();

        if(GameManager.Instance.GameState == GameStateType.Pause)
        {
            currentJudgeEffect = null;
        }
    }

    //�ڵ����� ��Ʈ ����
    private void NoteHitAuto()
    {
        if (!GameManager.Instance.IsAuto) return;

        for (int i = 0; i < noteQueues.Length; i++)
        {
            Queue<Note> queue = noteQueues[i];

            if (queue.Count == 0) continue;

            if (GameManager.Instance.SongPosition >= queue.Peek().NoteHitTime)
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
                comboCount++;

                //��Ʈ ������Ʈ �޸� ����
                AddressableManager.Instance.ReleaseObject(currentNote.gameObject);


                //����Ʈ ��ȯ
                ObjectPoolManager.Instance.SpawnObject(hitEffect, new Vector2(GameManager.Instance.NoteSpawnTransforms[lineIndex].position.x, center.position.y), Quaternion.identity);

                SpawnJudgeEffect(judgeDatas[j].type);

                //ȿ���� ���
                SoundManager.Instance.PlaySFX(GameManager.Instance.NoteHitSfx);

                noteQueues[lineIndex].Dequeue();
                return;
            }
        }
    }

    //ī�޶󿡼� �������� ��Ʈ ����
    public void JudgeNoteOutCamera(Note note)
    {
        int index = note.NoteHitLine;

        if (noteQueues[index].Count > 0 && noteQueues[index].Peek() == note)
        {
            comboCount = 0;

            noteQueues[index].Dequeue();

            SpawnJudgeEffect(JudgeType.Bad);

            AddressableManager.Instance.ReleaseObject(note.gameObject);
        }
    }

    //��������Ʈ ������ ��ȯ
    private void SpawnJudgeEffect(JudgeType judge)
    {
        judgeResult = judge;

        if (currentJudgeEffect != null)
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(currentJudgeEffect);
            currentJudgeEffect = null;
        }

        currentJudgeEffect = ObjectPoolManager.Instance.SpawnObject(judgeEffect, canvas.transform.position, Quaternion.identity);
        currentJudgeEffect.transform.SetParent(canvas);
    }
}
