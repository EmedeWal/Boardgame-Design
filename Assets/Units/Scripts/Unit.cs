using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [Header("ENUM")]
    public UnitState State = UnitState.Idle;

    [Header("STATS")]
    [SerializeField] private UnitData _unitData;

    [Header("TARGETING")]
    [SerializeField] private float _autoRange;
    public LayerMask _targetLayer;
    public Transform _currentTarget;
    private bool _canAttack = true;

    [Header("OTHER")]
    [SerializeField] private Image _portrait;

    public void SetLayerAndTargetLayer(int layerIndex, LayerMask targetLayer)
    {
        gameObject.layer = layerIndex;
        _targetLayer = targetLayer;
    }

    public void SetPortraitColor(Color color)
    {
        _portrait.color = color;
    }

    public void AutoAttack()
    {
        Transform nearestEnemy = FindNearestTarget();

        if (nearestEnemy != null)
        {
            if (_currentTarget == nearestEnemy) return;

            SetTarget(nearestEnemy);
        }
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;
        State = UnitState.Chasing;

        StopAllCoroutines();
        StartCoroutine(MoveToTargetCoroutine(target));
    }

    public void SetTargetPosition(Vector2 targetPosition)
    {
        _currentTarget = null;
        State = UnitState.Moving;

        StopAllCoroutines();
        StartCoroutine(MoveToTargetPositionCoroutine(targetPosition));
    }

    private void Attack()
    {
        if (!_canAttack) return;

        State = UnitState.Attacking;
        _canAttack = false;

        if (_currentTarget.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(_unitData.Damage);
        }

        Invoke(nameof(ResetCanAttack), _unitData.AttackSpeed);
    }

    private void ResetCanAttack()
    {
        _canAttack = true;

        if (_currentTarget != null)
        {
            SetTarget(_currentTarget);
        }
        else
        {
            State = UnitState.Idle;
        }
    }

    private void Move(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _unitData.MovementSpeed * Time.deltaTime);
    }

    private IEnumerator MoveToTargetCoroutine(Transform target)
    {
        while (GetDistance(target.position) > _unitData.Range)
        {
            Move(target.position);
            yield return null;
        }

        Attack();
    }

    private IEnumerator MoveToTargetPositionCoroutine(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            Move(targetPosition);
            yield return null;
        }

        State = UnitState.Idle;
    }

    private Transform FindNearestTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _autoRange, _targetLayer);

        Transform nearestTarget = null;
        float smallestDistance = _autoRange;

        foreach (Collider target in targets)
        {
            float distanceToTarget = GetDistance(target.transform.position);

            if (distanceToTarget <= smallestDistance)
            {
                smallestDistance = distanceToTarget;
                nearestTarget = target.transform;
            }
        }

        return nearestTarget;
    }

    private float GetDistance(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition);
    }

    public int GetCost()
    {
        return _unitData.Cost;
    }
}
