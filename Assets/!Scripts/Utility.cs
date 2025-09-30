using Coding;
using Coding.SharpCube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Coding.SharpCube.Syntax;

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
            if (sections[i].Contains(first) && firstSection == null)
            {
                firstSection = i;
                continue;
            }
            else if (sections[i].Contains(second) && firstSection != null)
            {
                for (int j = (int)firstSection + 1; j < i + 1; j++)
                {
                    sections[(int)firstSection] += " " + sections[j];

                    sections[j] = null;
                }

                sections.RemoveAll(x => x == null);
                return;
            }
        }
    }

    public static List<string> FindEncapulasion(List<string> encapsulatedScript, int startPoint, out int endPoint, char startEncapsulation, char endEncapsulation)
    {
        endPoint = startPoint;
        for (int i = startPoint + 1; i >= 0; i--)
        {
            encapsulatedScript[i] = "removed";
        }

        int encapsulations = 1;

        for (int i = startPoint; i < encapsulatedScript.Count; i++)
        {
            if (encapsulations == 0) // if encapsulation is found, will set all values to be "removed"
            {
                encapsulatedScript[i] = "removed";
                continue;
            }

            if (encapsulatedScript[i].Contains(startEncapsulation))
            {
                encapsulations++;
            }
            else if (encapsulatedScript[i].Contains(endEncapsulation))
            {
                encapsulations--;

                if (encapsulations == 0)
                {
                    endPoint = i;
                    encapsulatedScript[i] = "removed";
                }
            }
        }

        encapsulatedScript.RemoveAll(item => item == "removed");
        encapsulatedScript.RemoveAll(item => item == "");

        return encapsulatedScript;
    }

    public static object ConvertStringToObject(string item)
    {
        try
        {
            return int.Parse(item);
        }
        catch
        {
            try
            {
                return float.Parse(item);
            }
            catch
            {
                try
                {
                    return bool.Parse(item);
                }
                catch
                {
                    return item;
                }
            }
        }
    }

    public static bool LineIsType(key key, List<string> sections, out int index)
    {
        index = Array.IndexOf(sections.ToArray(), keywords[key].word);
        return index >= 0;
    }

    public static List<string> SplitLineIntoSections(string line)
    {
        List<string> sections = line.Split(" ").ToList();
        FindAndRetain(ref sections, '"', '"');
        FindAndRetain(ref sections, '(', ')');

        return sections;
    }

    public static List<string> ExtractLines(string raw)
    {
        //removes enter
        string modified = raw.Replace("\n", "");
        //removes tab
        modified = modified.Replace("\t", "");
        //splits it into a string array while keeping ; { and }
        List<string> list = Regex.Split(modified, "(;|{|})").ToList();
        //removes ;
        list.RemoveAll(item => item == ";");

        return list;
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
