using AYellowpaper.SerializedCollections;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeIsBroken.Audio
{
    [Serializable]
    public class EmitterProgression
    {
        public string paramToProgress;
        public List<Vector2> progression;
        [Range(0, 1)]
        public float targetValue;
    }
    public class ProgressionBasedAudio : MonoBehaviour
    {
        [SerializedDictionary("EventEmitter", "EmitterProgression")]
        public SerializedDictionary<StudioEventEmitter, EmitterProgression> Emitters;

        private void Start()
        {
            //emitter = GetComponent<StudioEventEmitter>();
            GridBuilder.instance.gridUpdated += UpdateProgression;
        }
        private void OnDestroy()
        {
            GridBuilder.instance.gridUpdated -= UpdateProgression;
        }

        private void UpdateProgression()
        {
            foreach (var item in Emitters)
            {
                Emitters[item.Key].targetValue = GetClosestValue(item.Value);
                Debug.Log("machines: " + GridBuilder.instance.gridObjects.Count);
            }
            float GetClosestValue(EmitterProgression progression)
            {
                Vector2? closestValue = null;

                foreach (var item in progression.progression)
                {
                    if (item.y < GridBuilder.instance.gridObjects.Count) continue;
                    if (closestValue == null)
                    {
                        closestValue = item;
                        continue;
                    }
                    if (item.y < ((Vector2)closestValue).y)
                    {
                        closestValue = item;
                    }
                }

                if (closestValue == null)
                {
                    closestValue = progression.progression[progression.progression.Count - 1];
                }

                Debug.Log("Progression " + closestValue);

                return ((Vector2)closestValue).x;
            }
        }

        private void Update()
        {
            foreach (var emitter in Emitters)
            {
                emitter.Key.EventInstance.getParameterByName(emitter.Value.paramToProgress, out float emitterValue);

                if (emitterValue < emitter.Value.targetValue)
                    emitter.Key.SetParameter(emitter.Value.paramToProgress, emitterValue + Time.deltaTime / 10);
                else if (emitterValue > emitter.Value.targetValue)
                    emitter.Key.SetParameter(emitter.Value.paramToProgress, emitterValue - Time.deltaTime / 10);
            }
        }
    }
}
