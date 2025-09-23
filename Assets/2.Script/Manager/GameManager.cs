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

public class GameManager : Singleton<GameManager>
{
    [Header("GameSetting")]
    [SerializeField] private float musicBpm;
    [SerializeField] private float noteSpeed;
    [SerializeField] private int gameFrameRate = 60;

    [Header("Clip")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip noteHitSfx;

    [Header("Transform")]
    [SerializeField] private Transform center;
    [SerializeField] private Transform noteSpawnPos;

    [SerializeField] private bool isAuto;

    private float beatPerSec;
    private float noteHitTime;

    private GameStateType gameState;

    public float BeatPerSec => beatPerSec;
    public float NoteSpeed => noteSpeed;
    public float NoteHitTime => noteHitTime;
    public bool IsAuto => isAuto;
    public AudioClip NoteHitSfx => noteHitSfx;
    public Transform Center => center;
    public Transform NoteSpawnPos => noteSpawnPos;
    public GameStateType GameState => gameState;


    public override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = gameFrameRate;
    }
    private void Start()
    {
        //한 박자에 몇초 걸리는지 계산
        beatPerSec = 60 / musicBpm;

        StartMusic();
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

    //딜레이 후 음악 재생
    private void StartMusic()
    {
        //노트 스폰 위치에서 판정선 까지 가는데 걸리는 시간을 계산
        noteHitTime = (noteSpawnPos.position.y - center.position.y) / noteSpeed;

        //현재까지 오디오 엔진이 실행된 시간에 노트가 판정선 까지 가는데 걸리는 시간과 한 박자에 걸리는 시간을 더해 음악 딜레이 시간을 구함
        double delayTime = AudioSettings.dspTime + (noteHitTime + beatPerSec);

        //딜레이 시간만큼 멈췄다가 음악 실행
        SoundManager.Instance.PlayMusic(music, delayTime);
    }
}
