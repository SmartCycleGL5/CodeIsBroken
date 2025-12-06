using FMODUnity;
using System;
using UnityEngine;

public class AudioOnPlay : MonoBehaviour
{
    [SerializeField] StudioEventEmitter emitter;

    void Start()
    {
        GameManager.onStart += Start;
        GameManager.onStop += Stop;
    }

    void Play()
    {
        emitter.Play();
    }
    void Stop()
    {
        emitter.Stop();
    }
    
}
