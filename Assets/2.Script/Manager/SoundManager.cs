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
                return length / 1000f; //ms 변환
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

        //딜레이 시간만큼 멈췄다가 음악재생
        if (isPlayMusicWithDelay && delayTime <= GameManager.Instance.SongPosition)
        {
            currentEventInstance.start();
            isPlayMusicWithDelay = false;
        }
    }

    //음악 재생
    public void PlayMusic(EventReference eventRef)
    {
        currentEventInstance = RuntimeManager.CreateInstance(eventRef);
        currentEventInstance.setPitch(GameManager.Instance.GameSpeedMultiplier);
        currentEventInstance.start();
    }

    //딜레이 후 음악 재생
    public void PlayMusic(EventReference eventRef, double delayTime)
    {
        currentEventInstance = RuntimeManager.CreateInstance(eventRef);
        currentEventInstance.setPitch(GameManager.Instance.GameSpeedMultiplier);
        this.delayTime = delayTime;
        isPlayMusicWithDelay = true;
    }

    //음악 정지
    public void StopMusic()
    {
        if (!currentEventInstance.isValid()) return;
        currentEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    //음악 일시정지
    public void PauseMusic()
    {
        if (!currentEventInstance.isValid()) return;
        currentEventInstance.setPaused(true);
    }

    //음악 일시정지 풀기
    public void UnPauseMusic()
    {
        if (!currentEventInstance.isValid()) return;
        currentEventInstance.setPaused(false);
    }

    //효과음 재생
    public void PlaySFX(EventReference eventRef)
    {
        RuntimeManager.PlayOneShot(eventRef);
    }

}
