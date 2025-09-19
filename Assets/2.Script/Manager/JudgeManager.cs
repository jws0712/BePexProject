using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class JudgeManager : MonoBehaviour
{
    [SerializeField] private Transform centerTransform;
    [SerializeField] private float[] judgeDistances;

    private Vector2[] judgeRanges;

    private List<GameObject> noteList = new();

    public List<GameObject> NoteList => noteList;


    private void Start()
    {
        judgeRanges = new Vector2[judgeDistances.Length];

        for (int i = 0; i < judgeDistances.Length; i++)
        {
            judgeRanges[i] = new Vector2(centerTransform.position.y - judgeDistances[i], centerTransform.position.y + judgeDistances[i]);
        }
    }

    private void Update()
    {
        for(int i = 0; i < noteList.Count; i++)
        {
            float notePosY = noteList[i].transform.position.y;

            for(int y = 0; y < judgeRanges.Length; y++)
            {
                //��Ʈ�� Y ���� ���� �������� �ִ��� Ȯ��
                if (judgeRanges[y].x <= notePosY && notePosY <= judgeRanges[y].y)
                {
                    Debug.Log("Hit" + y);
                    return;
                }
            }
        }

        Debug.Log("Miss");
    }
}
