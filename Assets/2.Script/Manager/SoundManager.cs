//UnityEngine
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private bool isPlayMusicWithDelay;

    private double delayTime;

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
        bgmSource.pitch = GameManager.Instance.GameSpeedMultiplier;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;

        //딜레이 시간만큼 멈췄다가 음악재생
        if (isPlayMusicWithDelay && delayTime <= GameManager.Instance.SongPosition)
        {
            bgmSource.Play();
            isPlayMusicWithDelay = false;
        }
    }

    //음악 재생
    public void PlayMusic(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    //딜레이 후 음악 재생
    public void PlayMusic(AudioClip clip, double delayTime)
    {
        bgmSource.clip = clip;
        this.delayTime = delayTime;
        isPlayMusicWithDelay = true;
    }

    //음악 일시정지
    public void PauseMusic()
    {
        bgmSource.Pause();
    }

    //음악 일시정지 풀기
    public void UnPauseMusic()
    {
        bgmSource.UnPause();
    }

    //효과음 재생
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
