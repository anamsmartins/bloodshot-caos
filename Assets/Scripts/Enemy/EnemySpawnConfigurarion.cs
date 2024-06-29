using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SceneEnemySpawnConfig", menuName = "Configurations/SceneEnemySpawnConfig")]
public class SceneEnemySpawnConfiguration : ScriptableObject {
    [System.Serializable]
    public class EnemySpawnData {
        public GameObject enemyPrefab;
        public int count;
    }

    public List<EnemySpawnData> enemiesToSpawn;
}
