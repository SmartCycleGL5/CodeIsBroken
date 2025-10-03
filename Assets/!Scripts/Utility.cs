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
}
