using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour
{
    public static Crafter instance;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public void CraftItem()
    {

    }
}
