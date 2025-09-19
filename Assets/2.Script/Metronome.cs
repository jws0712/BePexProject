using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private Transform noteSpawnTransform;
    [SerializeField] private GameObject notePrefab;
    

    private double beatDurationSec;
    private double currentTime;

    private int lastBeat;

    private void Start()
    {
        beatDurationSec = 60d / bpm;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= beatDurationSec)
        {
            Instantiate(notePrefab, noteSpawnTransform.position, Quaternion.identity);
            currentTime -= beatDurationSec;
        }
    }
}
