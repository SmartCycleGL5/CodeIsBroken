using UnityEngine;

public class CellPosition : MonoBehaviour
{

    public Vector2 GetCellPosition()
    {
        return new Vector2(transform.position.x, transform.position.z);
    }
}
