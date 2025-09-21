using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum GameStateType
{
    Idle,
    Play,
    Pause,
    End,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("GameSetting")]
    [SerializeField] private float musicBpm;
    [SerializeField] private float noteSpeed;

    [Header("Clip")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip noteHitSfx;

    [Header("Transform")]
    [SerializeField] private Transform center;
    [SerializeField] private Transform noteSpawnPos;

    [SerializeField] private bool isAuto;

    private float beatPerSec;

    private GameStateType gameState;

    public GameStateType GameState => gameState;

    public float BeatPerSec => beatPerSec;
    public float NoteSpeed => noteSpeed;
    public AudioClip NoteHitSfx => noteHitSfx;
    public Transform Center => center;
    public Transform NoteSpawnPos => noteSpawnPos;

    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 120;

    }

    private void Start()
    {
        beatPerSec = 60 / musicBpm;

        float noteTravelTime = (noteSpawnPos.position.y - center.position.y) / noteSpeed;
        float delayTime = (float)AudioSettings.dspTime + (noteTravelTime + beatPerSec);

        SoundManager.Instance.PlayMusic(music, delayTime);
    }

    public void Pause()
    {
        gameState = GameStateType.Pause;
        SoundManager.Instance.PauseMusic();
    }

    public void Continue()
    {
        gameState = GameStateType.Play;
        SoundManager.Instance.UnPauseMusic();
    }

}
