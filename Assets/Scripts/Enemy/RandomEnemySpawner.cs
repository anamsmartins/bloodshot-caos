using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private Transform[] spawnPoints;
    public SceneEnemySpawnConfiguration enemySpawnConfiguration;

    void Start() {
        SpawnEnemies();
    }

    void SpawnEnemies() {
        if (enemySpawnConfiguration == null) {
            Debug.LogError("SceneEnemySpawnConfiguration not assigned in EnemySpawner!");
            return;
        }

        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        foreach (var enemySpawnData in enemySpawnConfiguration.enemiesToSpawn) {
            for (int i = 0; i < enemySpawnData.count; i++) {
                if (availableSpawnPoints.Count == 0) {
                    Debug.LogWarning("Not enough spawn points for all enemies!");
                    return;
                }

                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[randomIndex];
                availableSpawnPoints.RemoveAt(randomIndex);

                Instantiate(enemySpawnData.enemyPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }
}
