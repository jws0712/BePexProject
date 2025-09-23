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
        //���� ���� �ð� ����
        startDpsTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;
        //�̹� �帥 �ð��� ���� �帣�� �ִ� �ð��� ���� ���� �ð��� ����
        songPosition = (AudioSettings.dspTime - startDpsTime) - musicOffset * bgmSource.pitch;
    }

    //���� ���
    public void PlayMusic(AudioClip clip, double delayTime)
    {
        bgmSource.clip = clip;
        bgmSource.PlayScheduled(delayTime);
    }

    //���� �Ͻ�����
    public void PauseMusic()
    {
        //�Ͻ����� ��ư�� ���� �ð��� ���
        pauseOffset = AudioSettings.dspTime - startDpsTime;
        bgmSource.Pause();
    }

    //���� �Ͻ����� Ǯ��
    public void UnPauseMusic()
    {
        //���� ���� �ð��� �Ͻ����� ������ ������ �ð��� ���� ���� ���� �ð��� �ٽ� ���
        startDpsTime = (float)AudioSettings.dspTime - pauseOffset;
        bgmSource.UnPause();
    }

    //ȿ���� ���
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
