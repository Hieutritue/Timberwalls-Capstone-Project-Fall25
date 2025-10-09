using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.PlaceableInstances;
using DefaultNamespace.PlacementStates;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using StateMachine = _Scripts.StateMachine.StateMachine;

public class PlacementSystem : MonoBehaviour
{
    public PlaceableCollectionSO PlaceableCollectionSo;

    private Vector3Int _lastGridPosition = new(-1, -1, -1);
    private bool _isInPlacementMode;
    private GridData _roomGridData = new();
    private GridData _itemsGridData = new();
    private Renderer _previewRenderer;

    private StateMachine _stateMachine = new StateMachine();
    private DeleteModePlacementState _deleteModePlacementState;
    private PreviewPlacementState _previewPlacementState;
    private IdlePlacementState _idlePlacementState;
    
    public List<RoomInstance> PlacedRooms => _roomGridData.PlacedInstances.Values
        .Distinct()
        .Select(pi => pi as RoomInstance)
        .Where(ri => ri != null)
        .ToList();
    public List<ItemInstance> PlacedItems => _itemsGridData.PlacedInstances.Values
        .Distinct()
        .Select(pi => pi as ItemInstance)
        .Where(ii => ii != null)
        .ToList();
    private void InitStateMachine()
    {
        _deleteModePlacementState = new DeleteModePlacementState(this);
        _previewPlacementState = new PreviewPlacementState(this);
        _idlePlacementState = new IdlePlacementState(this);

        _stateMachine.Initialize(_idlePlacementState);
    }

    private void Start()
    {
        BuildingSystemManager.Instance.CellIndicator.SetActive(false);
        _previewRenderer = BuildingSystemManager.Instance.CellIndicator.GetComponentInChildren<Renderer>();

        InitStateMachine();
    }

    public Vector3 MousePosition => BuildingSystemManager.Instance.InputManager.GetSelectedMapPosition();

    public Vector3Int GridPositionOfMouse(Vector3 mousePosition) =>
        BuildingSystemManager.Instance.Grid.WorldToCell(mousePosition);

    [Button]
    public void ResetLastGridPosition() => _lastGridPosition = new Vector3Int(-1, -1, -1);

    private void Update()
    {
        var mousePosition = MousePosition;
        var gridPosition = GridPositionOfMouse(mousePosition);
        if (gridPosition == _lastGridPosition) return;
        _lastGridPosition = gridPosition;
        _stateMachine.Update();
    }

    public void EnterPlacementMode(int id)
    {
        ResetLastGridPosition();
        _previewPlacementState.ChangeCurrentPlaceableIndex(id - 1);
        _stateMachine.TransitionTo(_previewPlacementState);
    }

    public void ExitPlacementMode()
    {
        _stateMachine.TransitionTo(_idlePlacementState);
    }

    public GridData GetGridData(PlaceableType placeableType)
    {
        return placeableType switch
        {
            PlaceableType.Room => _roomGridData,
            _ => _itemsGridData
        };
    }

    public void EnterDeleteMode(PlaceableType placeableType)
    {
        _deleteModePlacementState.ChangePlaceableType(placeableType);
        _stateMachine.TransitionTo(_deleteModePlacementState);
    }

    public void InstantiatePreviewObject(GameObject prefab)
    {
        var preview = Instantiate(prefab,
            BuildingSystemManager.Instance.CellIndicator.transform);
        BuildingSystemManager.Instance.CellIndicator.SetActive(true);
    }

    public void DestroyPreviewObject()
    {
        if (BuildingSystemManager.Instance.CellIndicator.transform.childCount > 0)
            Destroy(BuildingSystemManager.Instance.CellIndicator.transform.GetChild(0)?.gameObject);
    }
}