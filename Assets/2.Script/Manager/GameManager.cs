using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private AudioClip music;

    private bool isGameStart;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGameStart) return;

        if(collision.CompareTag("Note"))
        {
            isGameStart = true;
            SoundManager.Instance.PlayMusic(music);
        }
    }
}
