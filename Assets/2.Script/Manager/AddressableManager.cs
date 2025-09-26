//System
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;


//UnityEngine
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : Singleton<AddressableManager>
{
    [SerializeField] private AssetReferenceGameObject noteObj;

    private AsyncOperationHandle<GameObject> noteHandle;

    private List<GameObject> loadedObjectList = new();

    private IEnumerator Start()
    {
        yield return PreloadNoteObject();
    }

    private void OnDestroy()
    {
        if(noteHandle.IsValid()) Addressables.Release(noteHandle);
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
                note.Initialize(lineIndex, SoundManager.Instance.SongPosition);
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
}
