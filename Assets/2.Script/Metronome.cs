using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] private GameObject notePrefab;

    private int lastBeat;

    private float nextBeatPos;
    private float beatPerSec;
    private float musicLength;

    private double songPos;

    private Transform noteSpwanPos;

    private void Start()
    {
        nextBeatPos = GameManager.Instance.BeatPerSec;
        noteSpwanPos = GameManager.Instance.NoteSpawnPos;
        beatPerSec = GameManager.Instance.BeatPerSec;

        musicLength = SoundManager.Instance.MusicLength;
    }

    private void Update()
    {
        songPos = SoundManager.Instance.SongPosition;

        if (GameManager.Instance.GameState == GameStateType.Pause || songPos >= musicLength) return;

        if (songPos >= nextBeatPos)
        {
            //노트 생성
            AddressableManager.Instance.LoadNoteObject(noteSpwanPos);

            nextBeatPos += beatPerSec;
            lastBeat++;
        }
    }
}