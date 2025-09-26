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

        //프리 로드 실패시 실행
        if(noteHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("노트 프리로드 실패");
            yield break;
        }
    }
    
    //메모리 로드된 오브젝트 
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

    //메모리 로드된 오브젝트 메모리 해제
    public void ReleaseObject(GameObject obj)
    {
        //로드된 오브젝트가 없다면
        if (loadedObjectList.Count == 0) return;

        Addressables.ReleaseInstance(obj);
        loadedObjectList.Remove(obj);
    }
}
