using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("UNIT COLLECTION")]
    [SerializeField] private List<Unit> _units = new List<Unit>();

    [Header("SPAWN LOCATION")]
    [SerializeField] private List<Transform> _spawnLocations = new List<Transform>();

    [Header("STATS")]
    [SerializeField] private int _waveValueBase = 4;
    [SerializeField] private float _waveDuration = 20f;
    private int _waveValue;

    private List<GameObject> _unitsToSpawn = new List<GameObject>();
    private float _spawnInterval;

    private List<GameObject> _unitsSpawned = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(ManageWavesCoroutine());
    }

    private IEnumerator ManageWavesCoroutine()
    {
        while (true)
        {
            _waveValue = _waveValueBase;

            StartCoroutine(GenerateEnemiesCoroutine());

            yield return new WaitForSeconds(_waveDuration + 1);
        }
    }

    private IEnumerator GenerateEnemiesCoroutine()
    {
        List<GameObject> generatedUnits = new List<GameObject>();

        while (_waveValue > 0)
        {
            int randomUnitID = Random.Range(0, _units.Count);
            int randomUnitCost = _units[randomUnitID].GetCost();

            if (_waveValue >= randomUnitCost)
            {
                generatedUnits.Add(_units[randomUnitID].GetPrefab());
                _waveValue -= randomUnitCost;
            }

            yield return null;
        }

        _unitsSpawned.Clear();
        _unitsToSpawn = generatedUnits;
        _spawnInterval = (_waveDuration / _unitsToSpawn.Count);

        StartCoroutine(SpawnEnemiesCoroutine());
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (_unitsToSpawn.Count > 0)
        {
            yield return new WaitForSeconds(_spawnInterval);

            GameObject currentUnit = Instantiate(_unitsToSpawn[0], GetValidSpawnPoint(), Quaternion.identity);
            currentUnit.layer = transform.gameObject.layer;
            currentUnit.transform.SetParent(transform, false);

            _unitsSpawned.Add(currentUnit);
            _unitsToSpawn.RemoveAt(0);
        }
    }

    private Vector3 GetValidSpawnPoint()
    {
        return _spawnLocations[Random.Range(0, _spawnLocations.Count)].position;
    }
}
