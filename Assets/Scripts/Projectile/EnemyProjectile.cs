using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
    [Header("Projectile")]
    [SerializeField] private int speed;
    [SerializeField] private int damage;

    [Header("Audio")]
    [SerializeField] private AudioClip shootAudioClip;
    private AudioSource audioSource = null;

    private Vector2 direction;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(Vector2 targetPosition) {
        SetDirection(targetPosition);
        PlayShootAudioClip();
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

    private void PlayShootAudioClip() {
        audioSource.PlayOneShot(shootAudioClip);
    }
}
