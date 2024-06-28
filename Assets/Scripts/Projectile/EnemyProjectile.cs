using UnityEngine;

public class Projectile : MonoBehaviour {
    [Header("Projectile")]
    [SerializeField] private int speed;
    [SerializeField] private int damage;

    private Vector2 direction;

    public void Initialize(Vector2 targetPosition) {
        SetDirection(targetPosition);
    }

    private void SetDirection(Vector2 targetPosition) {
        direction = (targetPosition - (Vector2)transform.position).normalized;
    }

    private void Update() {
        MoveProjectile();
    }

    private void MoveProjectile() {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        HandleCollision(other);
    }

    private void HandleCollision(Collider2D other) {
        if (other.CompareTag("Player")) {
            DamagePlayer(other);
        } else if (other.CompareTag("MapBoundary")) {
            DestroyProjectile();
        }
    }

    private void DamagePlayer(Collider2D playerCollider) {
        Player player = playerCollider.GetComponent<Player>();
        if (player != null) {
            player.TakeDamage(damage);
        }
        DestroyProjectile();
    }

    private void DestroyProjectile() {
        Destroy(gameObject);
    }
}
