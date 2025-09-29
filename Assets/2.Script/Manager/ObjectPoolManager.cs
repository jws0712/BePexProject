//System
using System.Collections;
using System.Collections.Generic;

//UnityEngeine
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<string, ObjectPool<GameObject>> objectPoolDict = new();
    private Dictionary<GameObject, string> clonePrefabDict = new();

    private List<GameObject> activeObjectList = new();

    private void Start()
    {
        GameManager.Instance.AddGameEventListener(GameStateType.Restart, ReturnAllObjectToPool);
    }

    private void OnDestroy()
    {
        foreach(var pool in objectPoolDict.Values)
        {
            pool.Clear();
        }

        objectPoolDict.Clear();
        clonePrefabDict.Clear();
        activeObjectList.Clear();

        if (GameManager.Instance == null) return;
        GameManager.Instance.RemoveGameEventListener(GameStateType.Restart, ReturnAllObjectToPool);
    }
    
    private void CreatePool(string addressKey)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>
        (
            createFunc: () => CreateObject(addressKey),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );

        objectPoolDict.Add(addressKey, pool);
    }

    private GameObject CreateObject(string addressKey)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(addressKey);
        handle.WaitForCompletion();
        GameObject obj = handle.Result;
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
        if (obj == null) return;

        if(clonePrefabDict.ContainsKey(obj))
        {
            Addressables.ReleaseInstance(obj);
            clonePrefabDict.Remove(obj);
        }
    }

    public GameObject SpawnObject(string addressKey,  Vector3 spawnPos, Quaternion spawnRot)
    {
        if(!objectPoolDict.ContainsKey(addressKey))
        {
            CreatePool(addressKey);
        }

        GameObject obj = objectPoolDict[addressKey].Get();

        if(obj != null)
        {
            if(!clonePrefabDict.ContainsKey(obj))
            {
                clonePrefabDict.Add(obj, addressKey);
            }

            obj.transform.SetPositionAndRotation(spawnPos, spawnRot);
            activeObjectList.Add(obj);
            return obj;
        }

        return null;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        if(clonePrefabDict.TryGetValue(obj, out string key))
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
