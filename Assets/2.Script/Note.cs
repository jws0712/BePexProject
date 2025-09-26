//System
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Build.Content;
using UnityEditor.U2D;



//UnityEngine
using UnityEngine;

public class Note : MonoBehaviour
{
    private double spawnTime;
    private double noteHitTime;

    private bool isQuit;

    public double NoteHitTime => noteHitTime;

    private void OnEnable()
    {
        Debug.Log(SoundManager.Instance.SongPosition);

        spawnTime = SoundManager.Instance.SongPosition;
        noteHitTime = spawnTime + GameManager.Instance.NoteTravelTime;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;

        double t = (SoundManager.Instance.SongPosition - spawnTime) / GameManager.Instance.NoteTravelTime;

        transform.position = Vector3.LerpUnclamped(GameManager.Instance.NoteSpawnTransform.position, GameManager.Instance.Center.position, (float)t);
    }

    private void OnApplicationQuit()
    {
        isQuit = true;
    }

    private void OnBecameInvisible()
    {
        if (isQuit) return;
        JudgeManager.Instance.JudgeNoteOutCamera(this);
    }
}
