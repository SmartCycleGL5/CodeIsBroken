using System;
using FMODUnity;
using UnityEngine;

namespace CodeIsBroken.Audio
{
    [Serializable]
    public struct VolumeSettings
    {
        [Range(0, 100)]
        public float masterVolume;
        [Range(0, 100)]
        public float musicVolume;
        [Range(0, 100)]
        public float sfxVolume;
    }
    public class AudioManager : MonoBehaviour
    {
        public VolumeSettings settings;
        public static AudioManager instance;

        public static void PlayOneShot(EventReference reference)
        {
            PlayOneShot(reference, Vector3.zero);
        }
        public static void PlayOneShot(EventReference reference, Vector3 position)
        {
            GameObject instance = new GameObject(reference.Path);
            StudioEventEmitter emitter = instance.AddComponent<StudioEventEmitter>();
            emitter.EventReference = reference;
            emitter.Play();
            emitter.EventDescription.getLength(out int length);
            Debug.Log("length: " + length);
            Destroy(instance, length / 1000);
        }
    }
}
