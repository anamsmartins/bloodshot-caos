using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;
    [SerializeField] private float retreatFromOtherEnemiesDistance;
    [SerializeField] private Transform player;

    [SerializeField] private float startTimeBetweenShots;
    [SerializeField] private GameObject projectile;

    private float timeBetweenShots;
    private List<Enemy> allEnemies;

    void Start() {
        timeBetweenShots = startTimeBetweenShots;
        allEnemies = new List<Enemy>(FindObjectsOfType<Enemy>());
    }

    void Update() {
        Vector2 direction = Vector2.zero;
        float playerDistance = Vector2.Distance(transform.position, player.position);

        // If enemy is far away it will move close to the player
        if (playerDistance > stoppingDistance) {
            direction = (player.position - transform.position).normalized;

            // If it is near but not too near it will stop moving
        } else if (playerDistance < stoppingDistance && playerDistance > retreatDistance) {
            direction = Vector2.zero;
        }

        // If it is too near it will back away
        else if (playerDistance < retreatDistance) {
            direction = (transform.position - player.position).normalized;
        }

        // Check distances to other enemies and move away if too close
        foreach (Enemy otherEnemy in allEnemies) {
            if (otherEnemy != this) {
                float otherEnemyDistance = Vector2.Distance(transform.position, otherEnemy.transform.position);
                if (otherEnemyDistance < retreatFromOtherEnemiesDistance) {
                    Vector2 awayFromEnemy = (transform.position - otherEnemy.transform.position).normalized;
                    direction += awayFromEnemy;
                }
            }
        }

        // Normalize direction to avoid faster movement diagonally
        if (direction != Vector2.zero) {
            direction = direction.normalized * speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, speed * Time.deltaTime);
        }

        if (timeBetweenShots <= 0) {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
        } else {
            timeBetweenShots -= Time.deltaTime;
        }
    }
}
