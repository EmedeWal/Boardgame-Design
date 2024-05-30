using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    #region Enum
    public enum UnitState
    {
        Idle,
        Moving,
        Attacking
    }

    [Header("ENUM")]
    public UnitState State = UnitState.Idle;
    #endregion

    [Header("STATS")]
    [SerializeField] private UnitData _unitData;

    public Transform CurrentTarget;
    private bool _canAttack = true;

    [Header("OTHER")]
    [SerializeField] private Image _portrait;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _autoRange;

    //public void OnUpdate()
    //{
    //    AutoAttack();
    //}

    private void Update()
    {
        if (State == UnitState.Idle && CurrentTarget == null)
        {
            AutoAttack();
        }
    }

    public void SetPortraitColor(Color color)
    {
        _portrait.color = color;
    }

    public void SetTarget(Transform target)
    {
        CurrentTarget = target;

        State = UnitState.Moving;
        StopAllCoroutines();
        StartCoroutine(MoveToTargetCoroutine(target));
    }

    private IEnumerator MoveToTargetCoroutine(Transform target)
    {
        while (GetDistance(target.position) > _unitData.Range)
        {
            Move(target.position);
            yield return null;
        }

        State = UnitState.Attacking;
        StartAttacking();
    }

    public void SetTargetPosition(Vector2 targetPosition)
    {
        CurrentTarget = null;

        State = UnitState.Moving;
        StopAllCoroutines();
        StartCoroutine(MoveToTargetPositionCoroutine(targetPosition));
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

    private void AutoAttack()
    {
        Transform nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            SetTarget(nearestEnemy);
        }
    }

    private void StartAttacking()
    {
        Invoke(nameof(Attack), _unitData.InitialAttackDelay);
    }

    private void Attack()
    {
        if (!_canAttack) return;

        _canAttack = false;

        if (CurrentTarget.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(_unitData.Damage);
        }

        Invoke(nameof(ResetCanAttack), _unitData.AttackSpeed);
    }

    private void ResetCanAttack()
    {
        _canAttack = true;

        if (CurrentTarget != null)
        {
            Attack();
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

    private Transform FindNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, _autoRange, _enemyLayer);

        Transform nearestEnemy = null;
        float smallestDistance = _autoRange;

        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = GetDistance(enemy.transform.position);

            if (distanceToEnemy <= smallestDistance)
            {
                smallestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }

    private float GetDistance(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition);
    }

    public int GetCost()
    {
        return _unitData.Cost;
    }

    public GameObject GetPrefab()
    {
        return _unitData.Prefab;
    }
}
