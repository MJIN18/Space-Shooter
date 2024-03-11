using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerupPrefabs;
    [SerializeField] private float _xRange = 9.0f;
    [SerializeField] private float _ySpawnPosition = 6.0f;
    private bool _isPlayerDead = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (!_isPlayerDead)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, GenerateRandomSpawnPosition(), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (!_isPlayerDead)
        {
            int indexOfPowerupPrefab = Random.Range(0, _powerupPrefabs.Length);
            Instantiate(_powerupPrefabs[indexOfPowerupPrefab], GenerateRandomSpawnPosition(), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3,8));
        }
    }

    Vector3 GenerateRandomSpawnPosition()
    {
        float xRandomPosition = Random.Range(-_xRange, _xRange);
        Vector3 spawnPosition = new Vector3(xRandomPosition, _ySpawnPosition, transform.position.z);
        return spawnPosition;
    }

    public void OnPlayerDeath()
    {
        _isPlayerDead = true;
    }
}
