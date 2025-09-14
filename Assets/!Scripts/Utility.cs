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
    public static void FindAndRetain(ref List<string> sections, char first, char second)
    {
        int? firstSection = null;
        for (int i = 0; i < sections.Count; i++)
        {
            if (sections[i].Contains(first))
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

    public static void FindEncapulasion(ref List<string> encapsulatedScript, int startPoint, char startEncapsulation, char endEncapsulation)
    {
        for (int k = startPoint + 1; k >= 0; k--)
        {
            encapsulatedScript[k] = "removed";
        }

        int encapsulations = 1;

        for (int k = 0; k < encapsulatedScript.Count; k++) //k start at start point?
        {

            if (encapsulations == 0)
            {
                encapsulatedScript[k] = "removed";

            }
            else
            {
                if (encapsulatedScript[k].Contains(startEncapsulation))
                {
                    encapsulations++;
                }
                else if (encapsulatedScript[k].Contains(endEncapsulation))
                {
                    encapsulations--;

                    if (encapsulations == 0)
                    {
                        encapsulatedScript[k] = "removed";
                    }
                }
            }
        }

        encapsulatedScript.RemoveAll(item => item == "removed");
        encapsulatedScript.RemoveAll(item => item == "");
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
