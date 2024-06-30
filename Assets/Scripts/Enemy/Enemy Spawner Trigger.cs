using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float delayToSpawn = 2f;
    public SceneEnemySpawnConfiguration enemySpawnConfiguration;
    private bool hasSpawned = false;

  
    void OnTriggerEnter2D(Collider2D other) {
        if (!hasSpawned && other.CompareTag("Player")) {
            StartCoroutine(SpawnEnemiesWithDelay());
            hasSpawned = true;        }
    }

    IEnumerator SpawnEnemiesWithDelay() {
        yield return new WaitForSeconds(delayToSpawn); 
        SpawnEnemies();
    }

    void SpawnEnemies() {
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
