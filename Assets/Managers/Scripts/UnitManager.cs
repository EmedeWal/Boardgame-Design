using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private InputManager _inputManager;

    [Header("UNITS")]
    public Unit _selectedUnit;

    [Header("OTHER")]
    [SerializeField] private Color _selectedColor;

    //[SerializeField] private List<Unit> _placedUnits = new List<Unit>();

    //private void Awake()
    //{
    //    Unit[] units = FindObjectsOfType<Unit>();

    //    foreach (Unit unit in units)
    //    {
    //        _placedUnits.Add(unit);
    //    }
    //}

    //private void Update()
    //{
    //    foreach (Unit unit in _placedUnits)
    //    {
    //        if (unit.State == Unit.UnitState.Idle && unit.CurrentTarget == null)
    //        {
    //            unit.OnUpdate();
    //        }
    //    }
    //}

    private void OnEnable()
    {
        _inputManager.SelectUnit += UnitManager_SelectUnit;
    }

    private void OnDisable()
    {
        _inputManager.SelectUnit -= UnitManager_SelectUnit;
    }

    private void UnitManager_SelectUnit(Unit unit)
    {
        _selectedUnit = unit;
        _selectedUnit.SetPortraitColor(_selectedColor);

        _inputManager.DeselectUnits += UnitManager_DeselectUnits;
        _inputManager.SetTarget += UnitManager_SetTarget;
        _inputManager.SetTargetPosition += UnitManager_SetTargetPosition;
    }

    private void UnitManager_DeselectUnits()
    {
        _selectedUnit.SetPortraitColor(Color.white);
        _selectedUnit = null;

        _inputManager.DeselectUnits -= UnitManager_DeselectUnits;
        _inputManager.SetTarget -= UnitManager_SetTarget;
        _inputManager.SetTargetPosition -= UnitManager_SetTargetPosition;
    }

    private void UnitManager_SetTarget(Transform target)
    {
        _selectedUnit.SetTarget(target);
    }

    private void UnitManager_SetTargetPosition(Vector2 position)
    {
        _selectedUnit.SetTargetPosition(position);
    }
}
