using System;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    private void Update()
    {
        Vector3 direction = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
