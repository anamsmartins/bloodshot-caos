using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Enemy")]
    [SerializeField] private float maxHealthPoints;
    private float currentHealthPoints;

    [Header("Enemy Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;
    [SerializeField] private float retreatFromOtherEnemiesDistance;

    [Header("Enemy Shooting")]
    [SerializeField] private float startTimeBetweenShots;
    [SerializeField] private GameObject projectile;

    [Header("")]
    [SerializeField] private Transform player;

    private float timeBetweenShots;
    private List<Enemy> allEnemies;
    private Vector2 direction;
    private float playerDistance;

    void Start() {
        timeBetweenShots = startTimeBetweenShots;
        currentHealthPoints = maxHealthPoints;
        // Initialize the list of all enemies in the scene
        allEnemies = new List<Enemy>(FindObjectsOfType<Enemy>());
    }

    void Update() {
        Move();
        MoveAwayFromOtherEnemies();

        // Normalize direction to avoid faster movement diagonally
        if (direction != Vector2.zero) {
            direction = direction.normalized * speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, speed * Time.deltaTime);
        }

        Shoot();
        
    }

    private void Move() {
        // Calculate movement direction
        direction = Vector2.zero;
        playerDistance = Vector2.Distance(transform.position, player.position);

        // If enemy is far away, move close to the player
        if (playerDistance > stoppingDistance) {
            direction = (player.position - transform.position).normalized;
        }
        // If enemy is too close, move away from the player
        else if (playerDistance < retreatDistance) {
            direction = (transform.position - player.position).normalized;
        }
        // If enemy is near but not too near, stop moving
        else if (playerDistance <= stoppingDistance && playerDistance > retreatDistance) {
            direction = Vector2.zero;
        }
    }

    private void MoveAwayFromOtherEnemies() {
        // Temporary list to hold enemies that are still active
        List<Enemy> activeEnemies = new List<Enemy>();

        // Populate the temporary list with active enemies
        foreach (Enemy enemy in allEnemies) {
            if (enemy != null && enemy.gameObject != null) {
                activeEnemies.Add(enemy);
            }
        }

        // Move away from other enemies if too close
        foreach (Enemy otherEnemy in activeEnemies) {
            if (otherEnemy != this && otherEnemy != null && otherEnemy.gameObject != null) {
                float otherEnemyDistance = Vector2.Distance(transform.position, otherEnemy.transform.position);
                if (otherEnemyDistance < retreatFromOtherEnemiesDistance) {
                    Vector2 awayFromEnemy = (transform.position - otherEnemy.transform.position).normalized;
                    direction += awayFromEnemy;
                }
            }
        }
    }


    private void Shoot() {
        if (timeBetweenShots <= 0) {
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
            projectileScript.Initialize(player.position);
            timeBetweenShots = startTimeBetweenShots;
        } else {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damageAmount) {
        currentHealthPoints -= damageAmount;
        StartCoroutine(FlashOnDamage());
        if (currentHealthPoints <= 0) {
            Die();
        }
    }

    private IEnumerator FlashOnDamage() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) {
            renderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            renderer.color = Color.white;
        }
    }

    private void Die() {
        Destroy(gameObject);
    }

}
