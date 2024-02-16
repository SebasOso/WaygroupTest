using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] float timeToSpawn;
    [SerializeField] GameObject spawnPosition;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }
    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(timeToSpawn);
        GameObject enemy = Instantiate(enemyToSpawn, spawnPosition.transform.position, spawnPosition.transform.rotation);
    }
    public void Spawn()
    {
        StartCoroutine(SpawnEnemy());
    }
}
