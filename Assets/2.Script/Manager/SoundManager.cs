//UnityEngine
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private AudioClip currentSFXClip;

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

        //µÙ∑π¿Ã Ω√∞£∏∏≈≠ ∏ÿ√Ë¥Ÿ∞° ¿Ωæ«¿Áª˝
        if (isPlayMusicWithDelay && delayTime <= GameManager.Instance.SongPosition)
        {
            bgmSource.Play();
            isPlayMusicWithDelay = false;
        }
    }

    //¿Ωæ« ¿Áª˝
    public void PlayMusic(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    //µÙ∑π¿Ã »ƒ ¿Ωæ« ¿Áª˝
    public void PlayMusic(AudioClip clip, double delayTime)
    {
        bgmSource.clip = clip;
        this.delayTime = delayTime;
        isPlayMusicWithDelay = true;
    }

    //¿Ωæ« ¡§¡ˆ
    public void StopMusic()
    {
        bgmSource.Stop();
    }

    //¿Ωæ« ¿œΩ√¡§¡ˆ
    public void PauseMusic()
    {
        bgmSource.Pause();
    }

    //¿Ωæ« ¿œΩ√¡§¡ˆ «Æ±‚
    public void UnPauseMusic()
    {
        bgmSource.UnPause();
    }

    //»ø∞˙¿Ω ¿Áª˝
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
