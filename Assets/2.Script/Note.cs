//System
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;

public class Note : MonoBehaviour
{
    private float noteSpeed;
    private double noteHitTime;

    public double NoteHitTime => noteHitTime;

    private void OnEnable()
    {
        noteSpeed = GameManager.Instance.NoteSpeed;
        noteHitTime = SoundManager.Instance.SongPosition + GameManager.Instance.NoteHitTime;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;
        transform.position += Vector3.down * noteSpeed * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        JudgeManager.Instance.JudgeNoteOutCamera(this);
    }
}
