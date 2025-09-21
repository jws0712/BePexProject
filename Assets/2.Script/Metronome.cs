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

        //�帥�ð��� ������ ���� ���� ��ٸ� ��Ʈ������ ���� ��Ŵ
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
