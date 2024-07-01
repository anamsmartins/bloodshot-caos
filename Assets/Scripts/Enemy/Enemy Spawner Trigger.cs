using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float delayToSpawn = 2f;
    [SerializeField] private float delayBetweenSpawns = 10f;
    public SceneEnemySpawnConfiguration enemySpawnConfiguration;
    private bool hasSpawned = false;
    [SerializeField] private string bossSceneName = "MapBoss";

    void OnTriggerEnter2D(Collider2D other) {
        if (!hasSpawned && other.CompareTag("Player")) {
            StartCoroutine(SpawnEnemiesWithDelay());
            hasSpawned = true;
        }
    }

    IEnumerator SpawnEnemiesWithDelay() {
        yield return new WaitForSeconds(delayToSpawn);
        if (SceneManager.GetActiveScene().name == bossSceneName) {
            StartCoroutine(SpawnEnemiesOneByOne());
        } else {
            SpawnEnemiesAllAtOnce();
        }
    }

    IEnumerator SpawnEnemiesOneByOne() {
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        foreach (var enemySpawnData in enemySpawnConfiguration.enemiesToSpawn) {
            for (int i = 0; i < enemySpawnData.count; i++) {
                if (availableSpawnPoints.Count == 0) {
                    availableSpawnPoints = new List<Transform>(spawnPoints);
                }

                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[randomIndex];
                availableSpawnPoints.RemoveAt(randomIndex);

                Instantiate(enemySpawnData.enemyPrefab, spawnPoint.position, Quaternion.identity);

                yield return new WaitForSeconds(delayBetweenSpawns);
            }
        }
    }

    void SpawnEnemiesAllAtOnce() {
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        foreach (var enemySpawnData in enemySpawnConfiguration.enemiesToSpawn) {
            for (int i = 0; i < enemySpawnData.count; i++) {
                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[randomIndex];
                availableSpawnPoints.RemoveAt(randomIndex);

                Instantiate(enemySpawnData.enemyPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }
}
