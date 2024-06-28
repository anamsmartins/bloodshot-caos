using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Player")]
    [SerializeField] private int bloodTank;
    [SerializeField] private int moveSpeed = 7;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int healCost;

    [Header("Scoring")]
    [SerializeField] private int score;
    [SerializeField] private int healScore;
    [SerializeField] private int bloodPickupScore;

    [Header("Shooting")]
    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private int ammoCost;

    [Header("Melee Attack")]
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private int meleeDamage = 10;

    [Header("References")]
    [SerializeField] private GameInput gameInput;

    private int currentHealth;
    private bool isMoving;

    void Start() {
        currentHealth = maxHealth;
    }

    void Update() {
        HandleMovement();
        HandleHealing();
        HandleAttacks();
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movement = new Vector3(inputVector.x, inputVector.y, 0) * moveSpeed * Time.deltaTime;
        transform.position += movement;

        isMoving = inputVector != Vector2.zero;
    }

    private void HandleHealing() {
        if (gameInput.IsHealing()) {
            TryHeal();
        }
    }

    private void HandleAttacks() {
        if (gameInput.IsShooting()) {
            ShootOrMelee();
        }
    }

    private void ShootOrMelee() {
        if (UseBloodForShooting(ammoCost)) {
            Shoot();
        } else {
            MeleeAttack();
        }
    }

    private void Shoot() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - shootPosition.position).normalized;

        GameObject projectile = Instantiate(playerProjectilePrefab, shootPosition.position, Quaternion.identity);
        var playerProjectile = projectile.GetComponent<PlayerProjectile>();
        playerProjectile.SetDirection(direction);
    }

    private void MeleeAttack() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, meleeRange);

        foreach (Collider2D enemy in hitEnemies) {
            if (enemy.CompareTag("Enemy")) {
                Enemy enemyEntity = enemy.GetComponent<Enemy>();
                if (enemyEntity != null) {
                    enemyEntity.TakeDamage(meleeDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

    public bool IsMoving() {
        return isMoving;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        StartCoroutine(FlashOnDamage());

        if (currentHealth <= 0) {
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

    public bool UseBloodForShooting(int amount) {
        if (bloodTank >= amount) {
            bloodTank -= amount;
            return true;
        }
        return false;
    }

    private void TryHeal() {
        if (bloodTank >= healCost && currentHealth < maxHealth) {
            currentHealth = maxHealth;
            bloodTank -= healCost;
            AddScore(healScore);
        }
    }

    public void CollectBlood(int amount) {
        bloodTank += amount;
        AddScore(bloodPickupScore);
    }

    public void AddScore(int amount) {
        score += amount;
    }
}
