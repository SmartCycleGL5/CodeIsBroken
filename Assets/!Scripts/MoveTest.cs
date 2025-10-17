using UnityEngine;
using DG.Tweening;

public class MoveTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.DOMove(new Vector3(10,10,10), 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
