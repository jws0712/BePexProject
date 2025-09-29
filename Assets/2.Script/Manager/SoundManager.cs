//UnityEngine
using UnityEngine;

//FMOD
using FMOD.Studio;
using FMODUnity;

public class SoundManager : Singleton<SoundManager>
{
    private bool isPlayMusicWithDelay;

    private double delayTime;

    private EventInstance currentEventInstance;

    public float MusicLength
    {
        get
        {
            if(currentEventInstance.isValid())
            {
                currentEventInstance.getDescription(out EventDescription description);
                description.getLength(out int length);
                return length / 1000f; //ms ��ȯ
            }

            return 0;
        }
    }

    private void OnDestroy()
    {
        StopMusic();
        currentEventInstance.release();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;

        //������ �ð���ŭ ����ٰ� �������
        if (isPlayMusicWithDelay && delayTime <= GameManager.Instance.SongPosition)
        {
            currentEventInstance.start();
            isPlayMusicWithDelay = false;
        }
    }

    //���� ���
    public void PlayMusic(EventReference eventRef)
    {
        currentEventInstance = RuntimeManager.CreateInstance(eventRef);
        currentEventInstance.setPitch(GameManager.Instance.GameSpeedMultiplier);
        currentEventInstance.start();
    }

    //������ �� ���� ���
    public void PlayMusic(EventReference eventRef, double delayTime)
    {
        currentEventInstance = RuntimeManager.CreateInstance(eventRef);
        currentEventInstance.setPitch(GameManager.Instance.GameSpeedMultiplier);
        this.delayTime = delayTime;
        isPlayMusicWithDelay = true;
    }

    //���� ����
    public void StopMusic()
    {
        if (!currentEventInstance.isValid()) return;
        currentEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    //���� �Ͻ�����
    public void PauseMusic()
    {
        if (!currentEventInstance.isValid()) return;
        currentEventInstance.setPaused(true);
    }

    //���� �Ͻ����� Ǯ��
    public void UnPauseMusic()
    {
        if (!currentEventInstance.isValid()) return;
        currentEventInstance.setPaused(false);
    }

    //ȿ���� ���
    public void PlaySFX(EventReference eventRef)
    {
        RuntimeManager.PlayOneShot(eventRef);
    }

}
