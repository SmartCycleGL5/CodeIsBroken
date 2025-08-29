using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuildingInput : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Vector3 _lastPosition;
    [SerializeField] private LayerMask placementLayermask;

    public static event Action OnClickedLeft, OnExitLeft, OnClickedRight, Rotate, DisableBuilding;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)&& !IsPointerOverUI()){OnClickedLeft?.Invoke();}
        if (Input.GetMouseButtonUp(0)){OnExitLeft?.Invoke();}
        if (Input.GetMouseButtonDown(1)){OnClickedRight?.Invoke();}
        if (Input.GetKeyDown(KeyCode.E)){Rotate?.Invoke();}
        if (Input.GetKeyDown(KeyCode.Escape)){DisableBuilding?.Invoke();}
    }
    
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    
    public Vector3 GetMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            _lastPosition = hit.point;
        }
        return _lastPosition;
    }
    
}
