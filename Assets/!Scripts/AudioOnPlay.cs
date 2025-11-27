using FMODUnity;
using System;
using UnityEngine;

public class AudioOnPlay : MonoBehaviour
{
    [SerializeField] StudioEventEmitter emitter;

    void Start()
    {
        ScriptManager.onRun += toggle;
    }

    private void toggle(bool obj)
    {
        if(obj)
            emitter.Play();
        else
            emitter.Stop();
    }
}
