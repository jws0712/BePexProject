//System
using System.Collections;
using System.Collections.Generic;

//UnityEngeine
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<GameObject, ObjectPool<GameObject>> objectPoolDict = new();
    private Dictionary<GameObject, GameObject> clonePrefabDict = new();

    private List<GameObject> activeObjectList = new();

    private void Start()
    {
        GameManager.Instance.AddGameEventListener(GameStateType.Restart, ReturnAllObjectToPool);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.RemoveGameEventListener(GameStateType.Restart, ReturnAllObjectToPool);
    }
    
    private void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>
        (
            createFunc: () => CreateObject(prefab, pos, rot),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );

        objectPoolDict.Add(prefab, pool);
    }

    private GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        prefab.SetActive(false);
        GameObject obj = Instantiate(prefab, pos, rot);
        return obj;
    }

    private void OnGetObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyObject(GameObject obj)
    {
        if(clonePrefabDict.ContainsKey(obj))
        {
            clonePrefabDict.Remove(obj);
        }
    }

    //오브젝트를 풀에서 꺼내 배치
    public GameObject SpawnObject(GameObject spawnObj,  Vector3 spawnPos, Quaternion spawnRot)
    {
        if(!objectPoolDict.ContainsKey(spawnObj))
        {
            CreatePool(spawnObj, spawnPos, spawnRot);
        }

        GameObject obj = objectPoolDict[spawnObj].Get();

        if(obj != null)
        {
            if(!clonePrefabDict.ContainsKey(obj))
            {
                clonePrefabDict.Add(obj, spawnObj);
            }

            obj.transform.SetPositionAndRotation(spawnPos, spawnRot);

            activeObjectList.Add(obj);
            return obj;
        }

        return null;
    }

    //오브젝트를 풀로 되돌림
    public void ReturnObjectToPool(GameObject obj)
    {
        if(clonePrefabDict.TryGetValue(obj, out GameObject prefab))
        {
            if (objectPoolDict.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }

        activeObjectList.Remove(obj);
    }

    //활성화된 모든 오브젝트를 비활성화 함
    private void ReturnAllObjectToPool()
    {
        List<GameObject> copy = new List<GameObject>(activeObjectList);

        foreach (var obj in copy)
        {
            ReturnObjectToPool(obj);
        }

        activeObjectList.Clear();
    }
}
