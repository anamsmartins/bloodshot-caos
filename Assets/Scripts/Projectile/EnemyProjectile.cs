using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private Vector2 direction;

    // Method to initialize the projectile's direction
    public void Initialize(Vector2 targetPosition) {
        // Calculate the direction towards the target
        direction = (targetPosition - (Vector2)transform.position).normalized;
    }

    private void Update() {
        // Move the projectile in the calculated direction
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Destroy the projectile upon colliding with the player
        if (other.CompareTag("Player")) {
            DestroyProjectile();
        }
    }

    private void DestroyProjectile() {
        // Destroy the game object (projectile)
        Destroy(gameObject);
    }
}
