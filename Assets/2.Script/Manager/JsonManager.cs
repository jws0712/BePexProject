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

        //��� ����
        path = Application.persistentDataPath;

        LoadAllSaveData();
    }

    //������ ����
    public void SaveData(string fileName)
    {
        string savePath = path + "/" + fileName + ".json";

        string data = JsonUtility.ToJson(songData, true);

        Debug.Log(savePath);

        File.WriteAllText(savePath, data);
        //File.WriteAllText(path, EncryptAndDecrypt(data));
    }

    //������ ����
    public void DeleteData(string fileName)
    {
        string deletePath = path + "/" + fileName + ".json";

        if (File.Exists(deletePath))
        {
            File.Delete(deletePath);
            LoadAllSaveData();
        }
    }

    //������ �ҷ�����
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

    //��ο� ����� ��� �����͸� �ҷ���
    public void LoadAllSaveData()
    {
        string[] saves = Directory.GetFiles(path, "*.json");

        foreach (string file in saves)
        {
            string data = File.ReadAllText(file);
            SongData playerData = JsonUtility.FromJson<SongData>(data);
        }
    }

    //������ ��ȣȭ
    private string EncryptAndDecrypt(string data)
    {
        string resurt = "";

        for (int i = 0; i < data.Length; i++)
        {
            resurt += (char)(data[i] ^ keyward[i % keyward.Length]);
        }

        return resurt;
    }

    //�÷��̾� ������ ����
    public void SetSongData()
    {
        
    }
}


