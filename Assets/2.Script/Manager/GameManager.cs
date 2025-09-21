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
    [SerializeField] private int gameFrameRate = 60;

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
        float noteTravelTime = (noteSpawnPos.position.y - center.position.y) / noteSpeed;

        //������� ����� ������ ����� �ð��� ��Ʈ�� ������ ���� ���µ� �ɸ��� �ð��� �� ���ڿ� �ɸ��� �ð��� ���� ���� ������ �ð��� ����
        float delayTime = (float)AudioSettings.dspTime + (noteTravelTime + beatPerSec);

        //������ �ð���ŭ ����ٰ� ���� ����
        SoundManager.Instance.PlayMusic(music, delayTime);
    }
}
