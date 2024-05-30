using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("LAYERMASKS")]
    [SerializeField] private LayerMask _unitLayer;
    [SerializeField] private LayerMask _enemyLayer;

    public event Action<Unit> SelectUnit;
    public event Action DeselectUnits;

    public event Action<Transform> SetTarget;
    public event Action<Vector2> SetTargetPosition;

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftMouseClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleRightMouseClick();
        }
    }

    private void HandleLeftMouseClick()
    {
        if (RaycastAtMousePosition(_unitLayer, out RaycastHit hitInfo))
        {
            OnSelectUnit(hitInfo.transform.GetComponent<Unit>());
        }
        else
        {
            OnDeselectUnits();
        }
    }

    private void OnSelectUnit(Unit unit)
    {
        SelectUnit?.Invoke(unit);
    }

    private void OnDeselectUnits()
    {
        DeselectUnits?.Invoke();
    }

    private void HandleRightMouseClick()
    {
        if (RaycastAtMousePosition(_enemyLayer, out RaycastHit hitInfo))
        {
            OnSetTarget(hitInfo.transform);
        }
        else
        {
            OnSetTargetPosition(GetMousePosition());
        }
    }

    private void OnSetTarget(Transform target)
    {
        SetTarget?.Invoke(target);
    }

    private void OnSetTargetPosition(Vector3 mousePosition)
    {
        SetTargetPosition?.Invoke(mousePosition);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = -1;
        return mousePosition;
    }

    private bool RaycastAtMousePosition(LayerMask layerMask, out RaycastHit hitInfo)
    {
        Vector3 mousePosition = GetMousePosition();
        return Physics.Raycast(mousePosition, Vector3.forward, out hitInfo, Mathf.Infinity, layerMask);
    }
}
