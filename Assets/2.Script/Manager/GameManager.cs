//System
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;

public enum GameStateType
{
    Idle,
    Play,
    Pause,
    End,
}  

public enum LineType
{
    One,
    Two,
    Three,
    Four
} 
public class GameManager : Singleton<GameManager>
{
    [Header("GameSetting")]
    [SerializeField] private float musicBpm;
    [SerializeField] private float gameSpeed;
    [SerializeField] private float musicOffset;
    [SerializeField] private int gameFrameRate = 60;

    [Header("Clip")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip noteHitSfx;

    [Header("Transform")]
    [SerializeField] private Transform center;
    [SerializeField] private Transform noteSpawnTransformCenter;
    [SerializeField] private Transform[] noteSpawnTransforms;

    [SerializeField] private bool isAuto;

    private float secPerBeat;
    private float nextBeatPos;
    private float noteTravelTime;

    private double songPos;

    private GameStateType gameState;
    public float MusicOffset => musicOffset;
    public float GameSpeed => gameSpeed;
    public bool IsAuto => isAuto;
    public double NoteTravelTime => noteTravelTime;
    public AudioClip NoteHitSfx => noteHitSfx;
    public Transform Center => center;
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
        gameState = GameStateType.Play;

        //한 박자에 몇초 걸리는지 계산
        secPerBeat = 60 / musicBpm;

        noteTravelTime = (noteSpawnTransformCenter.position.y - center.position.y) / (gameSpeed * SoundManager.Instance.MusicPitch);

        double delayTime = AudioSettings.dspTime + noteTravelTime;

        //딜레이 시간만큼 멈췄다가 음악 실행
        SoundManager.Instance.PlayMusic(music, delayTime);
    }

    private void Update()
    {
        songPos = SoundManager.Instance.SongPosition;

        if (GameState == GameStateType.Pause || songPos > SoundManager.Instance.MusicLength) return;

        if (songPos > nextBeatPos)
        {
            for(int i = 0; i < JudgeManager.Instance.NoteQueues.Length; i++)
            {
                AddressableManager.Instance.LoadNoteObject(i, noteSpawnTransforms[i]);
            }

            nextBeatPos += secPerBeat;
        }
    }

    //일시정지
    public void Pause()
    {
        gameState = GameStateType.Pause;
        SoundManager.Instance.PauseMusic();
    }

    //계속하기
    public void Continue()
    {
        gameState = GameStateType.Play;
        SoundManager.Instance.UnPauseMusic();
    }
}
