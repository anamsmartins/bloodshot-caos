using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    [Header("Projectile")]
    [SerializeField] private int speed;
    [SerializeField] private int damage;

    [Header("Audio")]
    [SerializeField] private AudioClip shootAudioClip;
    private AudioSource audioSource = null;

    private Vector2 direction;
    private Animator myAnimator;

    public void Initialize(Vector2 direction) {
        SetDirection(direction);
        PlayShootAudioClip();
    }

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        if (other.CompareTag("Enemy")) {
            DamageEnemy(other);
        } else if (other.CompareTag("MapBoundary")) {
            DestroyProjectile();
        }
    }

    private void DamageEnemy(Collider2D enemyCollider) {
        Enemy enemy = enemyCollider.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);
        }
        DestroyProjectile();
    }

    private void SetDirection(Vector2 newDirection) {
        direction = newDirection.normalized;
    }

    private void DestroyProjectile() {
        Destroy(gameObject);
    }

    private void PlayShootAudioClip() {
        audioSource.PlayOneShot(shootAudioClip);
    }
}
