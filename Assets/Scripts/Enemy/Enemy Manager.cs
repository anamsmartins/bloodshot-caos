using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager Instance { get; private set; }
    private List<Enemy> enemies = new List<Enemy>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(Enemy enemy) {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy) {
        enemies.Remove(enemy);
        CheckAllEnemiesDefeated();
    }

    private void CheckAllEnemiesDefeated() {
        if (enemies.Count == 0) {
            Door.Instance.Open();
        }
    }
}
