using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private float destroyTime;

    private void OnEnable()
    {
        StartCoroutine(Co_Destroy());
    }

    private IEnumerator Co_Destroy()
    {
        yield return new WaitForSeconds(destroyTime);

        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }
}
