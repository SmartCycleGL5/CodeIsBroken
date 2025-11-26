using FMODUnity;
using System;
using UnityEngine;

namespace CodeIsBroken.Audio
{
    public class PowerUpDown : MonoBehaviour
    {
        StudioEventEmitter emitter;

        [SerializeField] EventReference powerUp;
        [SerializeField] EventReference powerDown;
        private void Start()
        {
            emitter = GetComponent<StudioEventEmitter>();

            ScriptManager.onRun += PowerToggle;
        }

        private void PowerToggle(bool obj)
        {
            if (obj) AudioManager.PlayOneShot(powerUp);
            else AudioManager.PlayOneShot(powerDown);
        }
        
    }
}
