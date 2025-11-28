using System;
using System.Threading.Tasks;
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
            PlayOneShot(reference, Vector3.zero, out int time);
        }
        public static void PlayOneShot(EventReference reference, out int time)
        {
            PlayOneShot(reference, Vector3.zero, out time);
        }
        public static async Task PlayOneShotAsync(EventReference reference)
        {
            PlayOneShot(reference, Vector3.zero, out int time);
            await Task.Delay(time);
        }
        static void PlayOneShot(EventReference reference, Vector3 position, out int time)
        {
            GameObject instance = new GameObject("reference.Path");
            StudioEventEmitter emitter = instance.AddComponent<StudioEventEmitter>();
            emitter.EventReference = reference;
            emitter.Play();
            emitter.EventDescription.getLength(out int length);
            time = length;

            Destroy(instance, time / 1000);
        }
    }
}
