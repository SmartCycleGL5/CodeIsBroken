using FMODUnity;
using System;
using UnityEngine;

namespace CodeIsBroke.Audio
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
            if (obj) PowerUp();
            else PowerDown();
        }

        public void PowerUp()
        {
            emitter.Stop();
            emitter.EventReference = powerUp;
            emitter.Play();
        }
        public void PowerDown()
        {
            emitter.Stop();
            emitter.EventReference = powerDown;
            //emitter
            emitter.Play();
        }
    }
}
