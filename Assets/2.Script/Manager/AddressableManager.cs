//System
using System.Collections;
using System.Collections.Generic;


//UnityEngine
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : Singleton<AddressableManager>
{
    [SerializeField] private AssetReferenceGameObject noteObj;

    private AsyncOperationHandle<GameObject> noteHandle;

    private List<GameObject> loadedObjectList = new();

    private void Start()
    {
        GameManager.Instance.AddGameEventListener(GameStateType.Restart, ReleaseAllObject);
        StartCoroutine(PreloadNoteObject());
    }

    private void OnDestroy()
    {
        if(noteHandle.IsValid()) Addressables.Release(noteHandle);

        if (GameManager.Instance == null) return;
        GameManager.Instance.RemoveGameEventListener(GameStateType.Restart, ReleaseAllObject);
    }

    private IEnumerator PreloadNoteObject()
    {
        noteHandle = Addressables.LoadAssetAsync<GameObject>(noteObj);
        yield return noteHandle;

        //���� �ε� ���н� ����
        if(noteHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("��Ʈ �����ε� ����");
            yield break;
        }
    }
    
    //�޸� �ε�� ������Ʈ 
    public void LoadNoteObject(int lineIndex, Transform transform)
    {
        noteObj.InstantiateAsync(transform.position, Quaternion.identity).Completed += (obj) =>
        {
            if (obj.Result.TryGetComponent(out Note note))
            {
                note.Initialize((LineType)lineIndex, GameManager.Instance.SongPosition);
                JudgeManager.Instance.NoteQueues[lineIndex].Enqueue(note);
            }
            loadedObjectList.Add(obj.Result);
        };
    }

    //�޸� �ε�� ������Ʈ �޸� ����
    public void ReleaseObject(GameObject obj)
    {
        //�ε�� ������Ʈ�� ���ٸ�
        if (loadedObjectList.Count == 0) return;

        Addressables.ReleaseInstance(obj);
        loadedObjectList.Remove(obj);
    }

    private void ReleaseAllObject()
    {
        foreach(var obj in loadedObjectList)
        {
            Addressables.ReleaseInstance(obj);
        }

        loadedObjectList.Clear();
    }
}
