using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private float noteSpeed;

    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position += Vector3.down * noteSpeed * Time.deltaTime;
    }

    public void HideNote()
    {
        spriteRenderer.sprite = null;
    }

    //ȭ�� ������ �������� ����
    private void OnBecameInvisible()
    {
        JudgeManager.Instance.NoteList.Remove(gameObject);
        Destroy(gameObject);
    }
}
