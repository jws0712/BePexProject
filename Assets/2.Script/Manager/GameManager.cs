//System
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;

public enum GameStateType
{
    Play,
    Pause,
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
        gameState = GameStateType.Play;

        secPerBeat = 60 / musicBpm;
        startDspTime = AudioSettings.dspTime;
        noteTravelTime = (noteSpawnTransformCenter.position.y - judgeLineCenter.position.y) / (noteSpeed * gameSpeedMultiplier);

        //������ �ð���ŭ ����ٰ� ���� ����
        SoundManager.Instance.PlayMusic(music, noteTravelTime);
    }

    private void Update()
    {
        if(songPosition > SoundManager.Instance.MusicLength - noteTravelTime)
        {
            gameState = GameStateType.End;
            return;
        }
        if (gameState == GameStateType.Pause) return;

        songPosition = ((AudioSettings.dspTime - startDspTime) * gameSpeedMultiplier) - musicOffset;

        //���� ���� ��Ʈ ��ȯ
        if (songPosition > nextBeatPos)
        {
            for(int i = 0; i < JudgeManager.Instance.NoteQueues.Length; i++)
            {
                AddressableManager.Instance.LoadNoteObject(i, noteSpawnTransforms[i]);
            }

            nextBeatPos += secPerBeat;
        }
    }

    //�Ͻ�����
    public void Pause()
    {
        gameState = GameStateType.Pause;
        pauseOffset = AudioSettings.dspTime - startDspTime;
        SoundManager.Instance.PauseMusic();
    }

    //����ϱ�
    public void UnPause()
    {
        gameState = GameStateType.Play;
        startDspTime = AudioSettings.dspTime - pauseOffset;
        SoundManager.Instance.UnPauseMusic();
    }

    //�ٽ��ϱ�
    public void ReStart()
    {
        //�ٽ��ϱ�
    }
}
