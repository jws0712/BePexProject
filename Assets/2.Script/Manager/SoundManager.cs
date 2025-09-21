using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private float musicOffset;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private float pauseOffset;
    private float startDpsTime;
    private float songPosition;

    public float SongPosition => songPosition;

    public float MusicLength
    {
        get
        {
            if(bgmSource.clip != null)
            {
                return bgmSource.clip.length;
            }

            return 0;
        }
    }     


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //이미 흐르고 있던 시간을 저장함
        startDpsTime = (float)AudioSettings.dspTime;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;
        //이미 흐른 시간을 현재 흐르고 있는 시간을 빼서 현재 시간을 구함
        songPosition = (float)(AudioSettings.dspTime - startDpsTime) - musicOffset * bgmSource.pitch;
    }

    public void PlayMusic(AudioClip clip, float delayTime)
    {
        bgmSource.clip = clip;
        bgmSource.PlayScheduled(delayTime);
    }

    public void PauseMusic()
    {
        pauseOffset = (float)AudioSettings.dspTime - startDpsTime;
        bgmSource.Pause();
    }

    public void UnPauseMusic()
    {
        startDpsTime = (float)AudioSettings.dspTime - pauseOffset;
        bgmSource.UnPause();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
