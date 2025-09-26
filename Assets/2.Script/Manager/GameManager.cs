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
    [SerializeField] private float gameSpeed;
    [SerializeField] private int gameFrameRate = 60;

    [Header("Clip")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip noteHitSfx;

    [Header("Transform")]
    [SerializeField] private Transform center;
    [SerializeField] private Transform noteSpawnTransform;

    [SerializeField] private bool isAuto;

    private float secPerBeat;
    private float nextBeatPos;
    private float noteTravelTime;

    private double songPos;

    private GameStateType gameState;

    public float GameSpeed => gameSpeed;
    public bool IsAuto => isAuto;
    public double NoteTravelTime => noteTravelTime;
    public AudioClip NoteHitSfx => noteHitSfx;
    public Transform Center => center;
    public Transform NoteSpawnTransform => noteSpawnTransform;
    public GameStateType GameState => gameState;


    public override void Awake()
    {
        base.Awake();


        Application.targetFrameRate = gameFrameRate;
    }
    private void Start()
    {
        gameState = GameStateType.Play;

        //�� ���ڿ� ���� �ɸ����� ���
        secPerBeat = 60 / musicBpm;

        noteTravelTime = (noteSpawnTransform.position.y - center.position.y) / (gameSpeed * SoundManager.Instance.MusicPitch);

        double delayTime = AudioSettings.dspTime + noteTravelTime;

        //������ �ð���ŭ ����ٰ� ���� ����
        SoundManager.Instance.PlayMusic(music, delayTime);
    }

    private void Update()
    {
        songPos = SoundManager.Instance.SongPosition;

        if (GameState == GameStateType.Pause || songPos >= SoundManager.Instance.MusicLength) return;

        if (songPos >= nextBeatPos)
        {
            //��Ʈ ����
            AddressableManager.Instance.LoadNoteObject(noteSpawnTransform);

            nextBeatPos += secPerBeat;
        }
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
}
