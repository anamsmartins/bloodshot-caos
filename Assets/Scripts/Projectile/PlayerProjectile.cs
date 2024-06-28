using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    [SerializeField] private int speed;
    [SerializeField] private int damage;
    private Vector2 direction;

    private void Awake()
    {
    }

    private void Update() {
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // add destroy the projectile after it hits the map wall
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) {
                // Deal damage to the enemy
                enemy.TakeDamage(damage);
            }
            DestroyProjectile();
        } else if (other.CompareTag("MapBoundary")) {
            DestroyProjectile();
        }
    }

    // Method to set the direction
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }

    private void DestroyProjectile() {
        Destroy(gameObject);
    }
}
