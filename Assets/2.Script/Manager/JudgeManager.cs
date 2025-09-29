//System
using FMODUnity;
using System;
using System.Collections.Generic;


//UnityEngine
using UnityEngine;

//Project
using static AddressableKey;

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
    [SerializeField] private EventReference noteHitSFX;
    [SerializeField] private JudgeData[] judgeDatas;
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
        GameManager.Instance.AddGameEventListener(GameStateType.Restart, ResetJudgeState);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.RemoveGameEventListener(GameStateType.Restart, ResetJudgeState);
    }

    private void ResetJudgeState()
    {
        comboCount = 0;

        currentJudgeEffect = null;

        foreach(var noteQueue in noteQueues)
        {
            noteQueue.Clear();
        }
    }

    private void Update()
    {
        NoteHitAuto();
    }

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

    public void JudgeNote(int lineIndex)
    {
        if (noteQueues[lineIndex].Count == 0) return;

        Note currentNote = noteQueues[lineIndex].Peek();

        float notePosY = currentNote.gameObject.transform.position.y;

        for (int j = 0; j < judgeDatas.Length; j++)
        {
            if ((center.position.y + judgeDatas[j].distance) >= notePosY
             && (center.position.y - judgeDatas[j].distance) <= notePosY)
            {
                comboCount++;

                ObjectPoolManager.Instance.ReturnObjectToPool(currentNote.gameObject);

                ObjectPoolManager.Instance.SpawnObject(HitEffectPrefab, new Vector2(GameManager.Instance.NoteSpawnTransforms[lineIndex].position.x, center.position.y), Quaternion.identity);

                SpawnJudgeEffect(judgeDatas[j].type);

                SoundManager.Instance.PlaySFX(noteHitSFX);

                noteQueues[lineIndex].Dequeue();
                return;
            }
        }

        comboCount = 0;

        noteQueues[lineIndex].Dequeue();

        SpawnJudgeEffect(JudgeType.Bad);

        ObjectPoolManager.Instance.ReturnObjectToPool(currentNote.gameObject);
    }

    private void SpawnJudgeEffect(JudgeType judge)
    {
        judgeResult = judge;

        if (currentJudgeEffect != null)
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(currentJudgeEffect);
            currentJudgeEffect = null;
        }

        currentJudgeEffect = ObjectPoolManager.Instance.SpawnObject(JudgeEffectPrefab, canvas.transform.position, Quaternion.identity);
        currentJudgeEffect.transform.SetParent(canvas);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if(collision.TryGetComponent(out Note note))
            {
                JudgeNote(note.NoteHitLine);
            }
        }
    }
}
