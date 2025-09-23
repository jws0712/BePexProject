//System
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private float musicOffset;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private double pauseOffset;
    private double startDpsTime;
    private double songPosition;

    public double SongPosition => songPosition;

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

    private void Start()
    {
        //게임 시작 시간 저장
        startDpsTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;
        //이미 흐른 시간을 현재 흐르고 있는 시간을 빼서 현재 시간을 구함
        songPosition = (AudioSettings.dspTime - startDpsTime) - musicOffset * bgmSource.pitch;
    }

    //음악 재생
    public void PlayMusic(AudioClip clip, double delayTime)
    {
        bgmSource.clip = clip;
        bgmSource.PlayScheduled(delayTime);
    }

    //음악 일시정지
    public void PauseMusic()
    {
        //일시정지 버튼을 누른 시간을 계산
        pauseOffset = AudioSettings.dspTime - startDpsTime;
        bgmSource.Pause();
    }

    //음악 일시정지 풀기
    public void UnPauseMusic()
    {
        //게임 시작 시간에 일시정지 상태인 동안의 시간을 빼서 게임 시작 시간을 다시 계산
        startDpsTime = (float)AudioSettings.dspTime - pauseOffset;
        bgmSource.UnPause();
    }

    //효과음 재생
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
