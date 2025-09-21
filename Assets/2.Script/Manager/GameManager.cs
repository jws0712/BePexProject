using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private AudioClip music;
    [SerializeField] private Transform center;
    [SerializeField] private bool isAuto;

    private bool isGameStart;

    public bool IsGameStart => isGameStart;
    public bool IsAuto => isAuto;

    public Transform Center => center;


    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 120;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAuto) JudgementManager.Instance.JudgeNote();

        if (isGameStart) return;

        if (collision.CompareTag("Note"))
        {
            isGameStart = true;
            SoundManager.Instance.PlayMusic(music);
        }
    }
}
