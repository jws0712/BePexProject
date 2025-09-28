//System
using System;
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;

public enum GameStateType
{
    Play,
    Pause,
    Restart,
    End,
}  

public enum LineType
{
    One = 0,
    Two = 1,
    Three = 2,
    Four = 3,
} 

public class GameManager : Singleton<GameManager>
{
    [Header("GameSetting")]
    [SerializeField] private float musicBpm;
    [SerializeField] private float noteSpeed;
    [SerializeField] private float musicOffset;
    [SerializeField] private float gameSpeedMultiplier;

    [SerializeField] private int gameFrameRate;

    [Header("Clip")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip noteHitSfx;

    [Header("Transform")]
    [SerializeField] private Transform judgeLineCenter;
    [SerializeField] private Transform noteSpawnTransformCenter;
    [SerializeField] private Transform[] noteSpawnTransforms;

    [Header("AutoMode")]
    [SerializeField] private bool isAuto;

    private double secPerBeat;
    private double nextBeatPos;
    private double noteTravelTime;
    private double songPosition;
    private double startDspTime;
    private double pauseOffset;

    private event Action pauseEvent;
    private event Action restartEvent;

    private GameStateType gameState;

    public bool IsAuto => isAuto;
    public float GameSpeedMultiplier => gameSpeedMultiplier;
    public double NoteTravelTime => noteTravelTime;
    public double SongPosition => songPosition;
    public AudioClip NoteHitSfx => noteHitSfx;
    public Transform JudgeLineCenter => judgeLineCenter;
    public Transform NoteSpawnTransformCenter => noteSpawnTransformCenter;
    public Transform[] NoteSpawnTransforms => noteSpawnTransforms;
    public GameStateType GameState => gameState;

    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = gameFrameRate;
    }

    private void Start()
    {
        Init();
        AddGameEventListener(GameStateType.Restart, Init);
    }

    private void OnDestroy()
    {
        if (Instance == null) return;
        RemoveGameEventListener(GameStateType.Restart, Init);
    }

    private void Update()
    {
        if (gameState == GameStateType.Pause) return;

        songPosition = ((AudioSettings.dspTime - startDspTime) * gameSpeedMultiplier) - musicOffset;

        //박자 마다 노트 소환
        if (songPosition > nextBeatPos && songPosition < SoundManager.Instance.MusicLength - noteTravelTime)
        {
            for(int i = 0; i < JudgeManager.Instance.NoteQueues.Length; i++)
            {
                AddressableManager.Instance.LoadNoteObject(i, noteSpawnTransforms[i]);
            }

            nextBeatPos += secPerBeat;
        }
    }

    private void Init()
    {
        SoundManager.Instance.StopMusic();

        gameState = GameStateType.Play;

        secPerBeat = 60 / musicBpm;
        songPosition = 0;
        nextBeatPos = 0;
        startDspTime = AudioSettings.dspTime;
        noteTravelTime = (noteSpawnTransformCenter.position.y - judgeLineCenter.position.y) / (noteSpeed * gameSpeedMultiplier);

        //딜레이 시간만큼 멈췄다가 음악 실행
        SoundManager.Instance.PlayMusic(music, noteTravelTime);
    }

    public void AddGameEventListener(GameStateType state, Action listener)
    {
        switch(state)
        {
            case GameStateType.Pause:
                {
                    pauseEvent += listener;
                    break;
                }
            case GameStateType.Restart:
                {
                    restartEvent += listener;
                    break;
                }
        }
    }

    public void RemoveGameEventListener(GameStateType state, Action listener)
    {
        switch (state)
        {
            case GameStateType.Pause:
                {
                    pauseEvent -= listener;
                    break;
                }
            case GameStateType.Restart:
                {
                    restartEvent -= listener;
                    break;
                }
        }
    }

    //일시정지
    public void Pause()
    {
        pauseEvent?.Invoke();

        gameState = GameStateType.Pause;
        pauseOffset = AudioSettings.dspTime - startDspTime;
        SoundManager.Instance.PauseMusic();
    }

    //계속하기
    public void UnPause()
    {
        gameState = GameStateType.Play;
        startDspTime = AudioSettings.dspTime - pauseOffset;
        SoundManager.Instance.UnPauseMusic();
    }

    //다시하기
    public void ReStart()
    {
        restartEvent?.Invoke();
    }
}
