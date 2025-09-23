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
        //�� ���ڿ� ���� �ɸ����� ���
        beatPerSec = 60 / musicBpm;

        StartMusic();
    }

    //�Ͻ�����
    public void Pause()
    {
        gameState = GameStateType.Pause;
        SoundManager.Instance.PauseMusic();
    }

    //����ϱ�
    public void Continue()
    {
        gameState = GameStateType.Play;
        SoundManager.Instance.UnPauseMusic();
    }

    //������ �� ���� ���
    private void StartMusic()
    {
        //��Ʈ ���� ��ġ���� ������ ���� ���µ� �ɸ��� �ð��� ���
        noteHitTime = (noteSpawnPos.position.y - center.position.y) / noteSpeed;

        //������� ����� ������ ����� �ð��� ��Ʈ�� ������ ���� ���µ� �ɸ��� �ð��� �� ���ڿ� �ɸ��� �ð��� ���� ���� ������ �ð��� ����
        double delayTime = AudioSettings.dspTime + (noteHitTime + beatPerSec);

        //������ �ð���ŭ ����ٰ� ���� ����
        SoundManager.Instance.PlayMusic(music, delayTime);
    }
}
