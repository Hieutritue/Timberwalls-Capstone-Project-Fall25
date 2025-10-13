using System;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask _placementLayerMask;
    
    private Camera _mainCamera; 
    private Vector3 _lastMousePosition;

    public Action
        OnMouseLeftClick,
        OnMouseRightClick;
    public Action<PlaceableType> OnClickRemovePlaceable;
    public Action<int> OnClickNum;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            OnMouseLeftClick?.Invoke();
        if (Input.GetMouseButtonDown(1))
            OnMouseRightClick?.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            OnClickNum?.Invoke(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            OnClickNum?.Invoke(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            OnClickNum?.Invoke(3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            OnClickNum?.Invoke(4);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            OnClickNum?.Invoke(5);
        if (Input.GetKeyDown(KeyCode.R))
            OnClickRemovePlaceable?.Invoke(PlaceableType.Room);
        if (Input.GetKeyDown(KeyCode.F))
            OnClickRemovePlaceable?.Invoke(PlaceableType.Furniture);
    }

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _mainCamera.nearClipPlane;
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, _placementLayerMask))
        {
            _lastMousePosition = hit.point;
        }

        return _lastMousePosition;
    }
    
}
