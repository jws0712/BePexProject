//System
using System;

//UnityEngine
using UnityEngine;

//FMOD
using FMOD;
using FMODUnity;

//Project
using static AddressableKey;

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
    [SerializeField] private EventReference music;

    [Header("Transform")]
    [SerializeField] private Transform judgeLineCenter;
    [SerializeField] private Transform noteSpawnTransformCenter;
    [SerializeField] private Transform[] noteSpawnTransforms;

    [Header("AutoMode")]
    [SerializeField] private bool isAuto;

    private float secPerBeat;
    private float nextBeatPos;
    private float noteTravelTime;

    private double songPosition;

    private ulong startDspTime;
    private ulong pauseOffset;

    private event Action pauseEvent;
    private event Action restartEvent;

    private FMOD.System coreSystem;
    private ChannelGroup masterChannelGroup;

    private GameStateType gameState;

    public bool IsAuto => isAuto;
    public float GameSpeedMultiplier => gameSpeedMultiplier;
    public double NoteTravelTime => noteTravelTime;
    public double SongPosition => songPosition;
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

    private void OnDisable()
    {
        if (Instance == null) return;
        RemoveGameEventListener(GameStateType.Restart, Init);
    }

    private void Update()
    {
        if (gameState == GameStateType.Pause) return;

        masterChannelGroup.getDSPClock(out ulong currentDspTime, out _);
        coreSystem.getSoftwareFormat(out int sampleRate, out _, out _);
        
        songPosition = (((double)(currentDspTime - startDspTime) / sampleRate) * gameSpeedMultiplier);

        if (songPosition > nextBeatPos && songPosition < SoundManager.Instance.MusicLength)
        {
            GenerateNnote();

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

        coreSystem = RuntimeManager.CoreSystem;
        coreSystem.getMasterChannelGroup(out masterChannelGroup);

        masterChannelGroup.getDSPClock(out startDspTime, out _);
        noteTravelTime = (noteSpawnTransformCenter.position.y - judgeLineCenter.position.y) / (noteSpeed * gameSpeedMultiplier);

        float delayTime = noteTravelTime - musicOffset;


        //딜레이 시간만큼 멈췄다가 음악 실행
        SoundManager.Instance.PlayMusic(music, delayTime);
    }

    private void GenerateNnote()
    {
        int spawnCount = UnityEngine.Random.Range(1, JudgeManager.Instance.NoteQueues.Length + 1);


        for(int i = 0; i < spawnCount; i++)
        {
            GameObject obj = ObjectPoolManager.Instance.SpawnObject(NotePrefab, noteSpawnTransforms[i].position, Quaternion.identity);

            if(obj.TryGetComponent(out Note note))
            {
                note.Init((LineType)i, SongPosition);
                JudgeManager.Instance.NoteQueues[i].Enqueue(note);
            }
        }
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
        masterChannelGroup.getDSPClock(out ulong currentDspTime, out _);
        pauseOffset = currentDspTime - startDspTime;


        SoundManager.Instance.PauseMusic();
    }

    //계속하기
    public void UnPause()
    {
        gameState = GameStateType.Play;
        masterChannelGroup.getDSPClock(out ulong currentDspTime, out _);
        startDspTime = currentDspTime - pauseOffset;
        SoundManager.Instance.UnPauseMusic();
    }

    //다시하기
    public void ReStart()
    {
        restartEvent?.Invoke();
    }
}
