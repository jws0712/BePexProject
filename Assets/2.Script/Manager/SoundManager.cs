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

        //������ �ð���ŭ ����ٰ� �������
        if (isPlayMusicWithDelay && delayTime <= GameManager.Instance.SongPosition)
        {
            bgmSource.Play();
            isPlayMusicWithDelay = false;
        }
    }

    //���� ���
    public void PlayMusic(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    //������ �� ���� ���
    public void PlayMusic(AudioClip clip, double delayTime)
    {
        bgmSource.clip = clip;
        this.delayTime = delayTime;
        isPlayMusicWithDelay = true;
    }

    //���� ����
    public void StopMusic()
    {
        bgmSource.Stop();
    }

    //���� �Ͻ�����
    public void PauseMusic()
    {
        bgmSource.Pause();
    }

    //���� �Ͻ����� Ǯ��
    public void UnPauseMusic()
    {
        bgmSource.UnPause();
    }

    //ȿ���� ���
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
