using FMODUnity;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace CodeIsBroken.Audio
{
    public class AdaptiveMusic : MonoBehaviour
    {
        [SerializeField] int machinesNeededForMax;
        StudioEventEmitter emitter;

        [SerializeField, ReadOnly, Range(0, 1)] float TargetValue;
        private void Start()
        {
            emitter = GetComponent<StudioEventEmitter>();
            GridBuilder.instance.gridUpdated += UpdateMusic;
        }
        private void OnDestroy()
        {
            GridBuilder.instance.gridUpdated -= UpdateMusic;
        }

        private void UpdateMusic()
        {
            TargetValue = Mathf.Clamp01(GridBuilder.instance.gridObjects.Count / (float)machinesNeededForMax);
        }

        private void Update()
        {
            emitter.EventInstance.getParameterByName("MusicProg", out float emitterValue);

            if (emitterValue < TargetValue)
                emitter.SetParameter("MusicProg", emitterValue + Time.deltaTime/10);
            else if(emitterValue > TargetValue) 
                emitter.SetParameter("MusicProg", emitterValue - Time.deltaTime/10);
        }
    }
}
