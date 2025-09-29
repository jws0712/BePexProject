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
    
    private void CreatePool(GameObject prefab)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>
        (
            createFunc: () => CreateObject(prefab),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );

        objectPoolDict.Add(prefab, pool);
    }

    private GameObject CreateObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
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

    public GameObject SpawnObject(GameObject prefab,  Vector3 spawnPos, Quaternion spawnRot)
    {
        if(!objectPoolDict.ContainsKey(prefab))
        {
            CreatePool(prefab);
        }

        GameObject obj = objectPoolDict[prefab].Get();

        if(obj != null)
        {
            if(!clonePrefabDict.ContainsKey(obj))
            {
                clonePrefabDict.Add(obj, prefab);
            }

            obj.transform.SetPositionAndRotation(spawnPos, spawnRot);

            activeObjectList.Add(obj);
            return obj;
        }

        return null;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        if(clonePrefabDict.TryGetValue(obj, out GameObject key))
        {
            if (objectPoolDict.TryGetValue(key, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }

        activeObjectList.Remove(obj);
    }

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
