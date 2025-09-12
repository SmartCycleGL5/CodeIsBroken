using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public static class Utility
{
    /// <summary>
    /// combines "" into a single section
    /// </summary>
    /// <param name="sections"></param>
    public static void FindAndRetainStrings(ref List<string> sections)
    {
        int? firstSection = null;
        for (int i = 0; i < sections.Count; i++)
        {
            if (sections[i].Contains('"'))
            {
                if (firstSection == null)
                    firstSection = i;
                else
                {
                    for (int j = (int)firstSection + 1; j < i + 1; j++)
                    {
                        sections[(int)firstSection] += " " + sections[j];

                        sections[j] = null;
                    }

                    sections[(int)firstSection] = sections[(int)firstSection].Substring(1, sections[(int)firstSection].Length - 2);

                    firstSection = null;
                }
            }
        }

        sections.RemoveAll(x => x == null);
    }

    public static class Addressable
    {
        public static async Task<T> ReturnAdressableAsset<T>(string path)
        {
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(path);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return (T)handle.Result;
            }

            Debug.LogError(handle.Status);
            return default;
        }
    }
}
