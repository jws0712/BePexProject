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

public class JudgementManager : MonoBehaviour
{
    public static JudgementManager Instance;

    [SerializeField] private JudgeData[] judgeDatas;
    [SerializeField] private GameObject judgeEffect;

    private Transform center;
    private Queue<GameObject> noteQueue = new();
    private JudgeType judgeResult;

    public JudgeType JudgeResult => judgeResult;

    public Queue<GameObject> NoteList => noteQueue;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        center = GameManager.Instance.Center;
    }

    public void JudgeNote()
    {
        if (noteQueue.Count == 0) return;

        GameObject currentNote = noteQueue.Peek();

        float notePosY = currentNote.transform.position.y;

        for (int j = 0; j < judgeDatas.Length; j++)
        {
            //노트의 Y 값이 판정 범위내에 있는지 확인
            if ((center.position.y + judgeDatas[j].distance) >= notePosY
             && (center.position.y - judgeDatas[j].distance) <= notePosY)
            {
                if (currentNote.TryGetComponent(out Note note))
                {
                    note.HideNote();

                    //이펙트 소환
                    Instantiate(judgeEffect, center.position, Quaternion.identity);
                }

                judgeResult = judgeDatas[j].type;
                noteQueue.Dequeue();
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if (noteQueue.Count > 0 && noteQueue.Peek() == collision.gameObject)
            {
                noteQueue.Dequeue();
                judgeResult = JudgeType.Bad;
            }

            Destroy(collision.gameObject);
        }
    }
}
