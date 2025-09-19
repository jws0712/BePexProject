using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private float noteSpeed;

    private void Update()
    {
        transform.position += Vector3.down * noteSpeed * Time.deltaTime;
    }

    //ȭ�� ������ �������� ����
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
