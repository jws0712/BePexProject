using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private Transform noteSpawnTransform;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject notePrefab2;

    private float beatDurationSec;
    private float nextBeatPos;

    private double timePosMs;
    private double startDspTime;

    private int lastBeat;

    private void Start()
    {
        beatDurationSec = 60f / bpm;
        nextBeatPos = beatDurationSec;

        startDspTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        timePosMs = AudioSettings.dspTime - startDspTime;

        //흐른시간이 음악의 길이 보다 길다면 노트생성을 정지 시킴
        if(timePosMs >= SoundManager.Instance.MusicLength)
        {
            return;
        }

        if (timePosMs >= nextBeatPos)
        {
            GameObject note = Instantiate(notePrefab, noteSpawnTransform.position, Quaternion.identity);
            JudgementManager.Instance.NoteList.Enqueue(note);

            nextBeatPos += beatDurationSec;
            lastBeat++;
        }
    }
}
