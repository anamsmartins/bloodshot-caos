using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Player player;

    [Header("Ammunition")]
    [SerializeField] private int ammoCost;

    [Header("Melee Attack")]
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private int meleeDamage = 10;

    void Update() {
        if (gameInput != null && gameInput.IsShooting()) {
            ShootOrMelee();
        }
    }

    void ShootOrMelee() {
        if (player.UseBloodForShooting(ammoCost)) {
            Shoot();
        } else {
            MeleeAttack();
        }
    }

    void Shoot() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - shootPosition.position).normalized;

        GameObject projectile = Instantiate(playerProjectilePrefab, shootPosition.position, Quaternion.identity);
        var playerProjectile = projectile.GetComponent<PlayerProjectile>();
        playerProjectile.SetDirection(direction);
    }

    void MeleeAttack() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, meleeRange);

        foreach (Collider2D enemy in hitEnemies) {
            if (enemy.CompareTag("Enemy")) {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null) {
                    enemyScript.TakeDamage(meleeDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
