using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("UNIT COLLECTION")]
    [SerializeField] private List<Unit> _units = new List<Unit>();

    [Header("SPAWN LOCATION")]
    [SerializeField] private float _upperLimitY;
    [SerializeField] private float _lowerLimitY;
    [SerializeField] private float _xValue;

    [Header("STATS")]
    [SerializeField] private int _baseWaveValue = 4;
    [SerializeField] private float _waveDuration = 20f;
    private int _currentWaveValue;

    private List<Unit> _unitsToSpawn = new List<Unit>();
    private float _spawnInterval;

    private List<Unit> _unitsSpawned = new List<Unit>();

    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        foreach (Unit unit in _unitsSpawned)
        {
            if (unit.State == UnitState.Idle)
            {
                unit.SetTargetPosition(transform.position + new Vector3(-30, 0, 0));
            }
            else if (unit.State != UnitState.Attacking)
            {
                unit.AutoAttack();
            }
        }
    }

    private void StartWave()
    {
        _currentWaveValue = _baseWaveValue;

        StartCoroutine(GenerateEnemiesCoroutine());
    }

    private IEnumerator GenerateEnemiesCoroutine()
    {
        List<Unit> generatedUnits = new List<Unit>();

        while (_currentWaveValue > 0)
        {
            int randomUnitID = Random.Range(0, _units.Count);
            int randomUnitCost = _units[randomUnitID].GetCost();

            if (_currentWaveValue >= randomUnitCost)
            {
                generatedUnits.Add(_units[randomUnitID]);
                _currentWaveValue -= randomUnitCost;
            }

            yield return null;
        }

        _unitsToSpawn = generatedUnits;
        _spawnInterval = _waveDuration / _unitsToSpawn.Count;

        StartCoroutine(SpawnEnemiesCoroutine());
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (_unitsToSpawn.Count > 0)
        {
            yield return new WaitForSeconds(_spawnInterval);

            SpawnUnit(_unitsToSpawn[0]);
        }

        StartWave();
    }

    private Vector3 GetValidSpawnPoint()
    {
        float yValue = Random.Range(_lowerLimitY, _upperLimitY);
        return new Vector3(_xValue, yValue, 0);
    }

    private void SpawnUnit(Unit unit)
    {
        Unit spawnedUnit = Instantiate(unit, GetValidSpawnPoint(), Quaternion.identity);
        spawnedUnit.SetLayerAndTargetLayer(LayerMask.NameToLayer("Hostile"), LayerMask.GetMask("Allied"));

        Health health = spawnedUnit.GetComponent<Health>();
        health.Death += EnemyManager_Death;

        _unitsSpawned.Add(spawnedUnit);
        _unitsToSpawn.RemoveAt(0);
    }

    private void EnemyManager_Death(GameObject unitObject)
    {
        _unitsSpawned.Remove(unitObject.GetComponent<Unit>());
        unitObject.GetComponent<Health>().Death -= EnemyManager_Death;
        Destroy(unitObject);
    }
}
