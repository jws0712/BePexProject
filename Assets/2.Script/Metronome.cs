using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private Transform noteSpawnTransform;
    [SerializeField] private GameObject notePrefab;

    private double beatDurationSec;
    private double nextBeatPos;
    private double timePosMs;
    private double dspSongTime;

    private int lastBeat;

    private double currentTime;

    private void Start()
    {
        beatDurationSec = 60d / bpm;
        nextBeatPos = beatDurationSec;

        dspSongTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        timePosMs = AudioSettings.dspTime - dspSongTime;

        if(timePosMs >= nextBeatPos)
        {
            GameObject note = Instantiate(notePrefab, noteSpawnTransform.position, Quaternion.identity);
            JudgeManager.Instance.NoteList.Add(note);

            nextBeatPos += beatDurationSec;
            lastBeat++;
        }
    }
}
