using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private GameObject notePrefab;

    private Transform noteSpwanPos;

    private float nextBeatPos;

    private float timePos;

    private int lastBeat;

    private void Start()
    {
        nextBeatPos = GameManager.Instance.BeatPerSec;

        noteSpwanPos = GameManager.Instance.NoteSpawnPos;
    }

    private void Update()
    {
        timePos = SoundManager.Instance.SongPosition;

        if (timePos >= SoundManager.Instance.MusicLength) return;
        if (GameManager.Instance.GameState == GameStateType.Pause) return;

        if (timePos >= nextBeatPos)
        {
            //노트 생성
            GameObject note = Instantiate(notePrefab, noteSpwanPos.position, Quaternion.identity);
            JudgementManager.Instance.NoteList.Enqueue(note);

            nextBeatPos += GameManager.Instance.BeatPerSec;
            lastBeat++;
        }
    }
}
