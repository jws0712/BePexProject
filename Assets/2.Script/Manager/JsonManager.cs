//System
using System;
using System.IO;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;

[Serializable]
public class SongData
{
    public string songName;
    public float songBpm;
    public float songOffset;
    public NoteData[] songNotes;
}

[Serializable]
public struct NoteData
{
    public LineType hitLine;
    public float spawnTime;
}

public class JsonManager : Singleton<JsonManager>
{
    private string path;

    private SongData songData = new();
    private string keyward = "aslkf jalksfjl!@#$ #@%dfaf^&#$% @#$";

    public SongData CurrentSongData => songData;

    public override void Awake()
    {
        base.Awake();

        //경로 설정
        path = Application.persistentDataPath;

        LoadAllSaveData();
    }

    //데이터 저장
    public void SaveData(string fileName)
    {
        string savePath = path + "/" + fileName + ".json";

        string data = JsonUtility.ToJson(songData, true);

        Debug.Log(savePath);

        File.WriteAllText(savePath, data);
        //File.WriteAllText(path, EncryptAndDecrypt(data));
    }

    //데이터 삭제
    public void DeleteData(string fileName)
    {
        string deletePath = path + "/" + fileName + ".json";

        if (File.Exists(deletePath))
        {
            File.Delete(deletePath);
            LoadAllSaveData();
        }
    }

    //데이터 불러오기
    public void LoadData(string fileName)
    {
        string loadPath = path + "/" + fileName + ".json";

        if (!File.Exists(loadPath))
        {
            SaveData(fileName);
        }

        string data = File.ReadAllText(loadPath);

        songData = JsonUtility.FromJson<SongData>(data);
        //playerData = JsonUtility.FromJson<PlayerData>(EncryptAndDecrypt(data));
    }

    //경로에 저장된 모든 데이터를 불러옴
    public void LoadAllSaveData()
    {
        string[] saves = Directory.GetFiles(path, "*.json");

        foreach (string file in saves)
        {
            string data = File.ReadAllText(file);
            SongData playerData = JsonUtility.FromJson<SongData>(data);
        }
    }

    //데이터 암호화
    private string EncryptAndDecrypt(string data)
    {
        string resurt = "";

        for (int i = 0; i < data.Length; i++)
        {
            resurt += (char)(data[i] ^ keyward[i % keyward.Length]);
        }

        return resurt;
    }

    //플레이어 데이터 설정
    public void SetSongData()
    {
        
    }
}


