//System
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private double pauseOffset;
    private double startDpsTime;
    private double songPosition;

    private float musicPitch;

    public double SongPosition => songPosition;

    public float MusicPitch => musicPitch;

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

    public override void Awake()
    {
        base.Awake();

        musicPitch = bgmSource.pitch;
    }

    private void Start()
    {
        startDpsTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;
        songPosition = ((AudioSettings.dspTime - startDpsTime) * musicPitch) - GameManager.Instance.MusicOffset;
    }

    //음악 재생
    public void PlayMusic(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlayMusic(AudioClip clip, double delayTime)
    {
        bgmSource.clip = clip;
        bgmSource.PlayScheduled(delayTime);
    }

    public void PauseMusic()
    {
        pauseOffset = AudioSettings.dspTime - startDpsTime;
        bgmSource.Pause();
    }

    //음악 일시정지 풀기
    public void UnPauseMusic()
    {
        startDpsTime = (float)AudioSettings.dspTime - pauseOffset;
        bgmSource.UnPause();
    }

    //효과음 재생
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
