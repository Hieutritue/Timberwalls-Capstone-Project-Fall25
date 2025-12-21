using System;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoSingleton<InputManager>
{
    [SerializeField] private LayerMask _placementLayerMask;

    private Camera _mainCamera;
    private Vector3 _lastMousePosition;

    public Action
        OnMouseLeftClick;

    public Action<PlaceableType> OnClickRemovePlaceable;
    public Action OnClickCancelKey;
    public Action<int> OnClickNum;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _continueButton;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseLeftClick?.Invoke();
        // if (Input.GetMouseButtonDown(1))
        //     OnMouseRightClick?.Invoke();
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
        if (Input.GetKeyDown(KeyCode.C))
            OnClickCancelKey?.Invoke();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_continueButton.gameObject.activeInHierarchy)
                _pauseButton.onClick.Invoke();
            else
            {
                _continueButton.onClick.Invoke();
            }
        }
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