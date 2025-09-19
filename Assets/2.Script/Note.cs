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

    //화면 밖으로 나갔을때 실행
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
