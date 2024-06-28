using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("Enemy")]
    [SerializeField] private int maxHealth;
    private float currentHealth;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;
    [SerializeField] private float avoidanceDistance;

    [Header("Shooting")]
    [SerializeField] private float shotCooldown;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Scoring")]
    [SerializeField] private int scorePerHit;
    [SerializeField] private int scorePerKill;

    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Player player;

    [Header("Effects")]
    [SerializeField] private GameObject bloodDropPrefab;

    private float shotTimer;
    private List<Enemy> allEnemies;
    private Vector2 movementDirection;

    void Start() {
        shotTimer = shotCooldown;
        currentHealth = maxHealth;
        allEnemies = new List<Enemy>(FindObjectsOfType<Enemy>());
    }

    void Update() {
        UpdateMovement();
        HandleShooting();
    }

    private void UpdateMovement() {
        movementDirection = CalculateMovementDirection();
        MoveAwayFromOtherEnemies();

        if (movementDirection != Vector2.zero) {
            movementDirection.Normalize();
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + movementDirection, speed * Time.deltaTime);
        }
    }

    private Vector2 CalculateMovementDirection() {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > stoppingDistance)
            return (playerTransform.position - transform.position).normalized;

        if (distanceToPlayer < retreatDistance)
            return (transform.position - playerTransform.position).normalized;

        return Vector2.zero;
    }

    private void MoveAwayFromOtherEnemies() {
        foreach (Enemy otherEnemy in allEnemies) {
            if (otherEnemy == this || otherEnemy == null)
                continue;

            float distanceToEnemy = Vector2.Distance(transform.position, otherEnemy.transform.position);

            if (distanceToEnemy < avoidanceDistance) {
                movementDirection += (Vector2)(transform.position - otherEnemy.transform.position).normalized;
            }
        }
    }

    private void HandleShooting() {
        if (shotTimer <= 0f) {
            Shoot();
            shotTimer = shotCooldown;
        } else {
            shotTimer -= Time.deltaTime;
        }
    }

    private void Shoot() {
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
        projectileScript.Initialize(playerTransform.position);
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        player.AddScore(scorePerHit);
        SpawnBloodDrop();

        if (currentHealth <= 0) {
            Die();
        } else {
            StartCoroutine(FlashOnDamage());
        }
    }

    private void SpawnBloodDrop() {
        Vector3 spawnPosition = transform.position + Vector3.back;
        Instantiate(bloodDropPrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator FlashOnDamage() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }

    private void Die() {
        player.AddScore(scorePerKill);
        Destroy(gameObject);
    }
}
