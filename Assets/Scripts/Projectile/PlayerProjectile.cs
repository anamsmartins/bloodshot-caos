using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    [SerializeField] private float speed;
    private Vector2 direction;

    // Method to set the direction
    public void SetDirection(Vector2 direction) {
        this.direction = direction.normalized;
    }

    private void Update() {
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // add destroy the projectile after it hits the map wall
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            DestroyProjectile();
            // Add logic for what happens when the projectile hits an enemy
        }
    }

    private void DestroyProjectile() {
        Destroy(gameObject);
    }
}
