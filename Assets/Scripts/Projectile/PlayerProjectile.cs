using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    [Header("Projectile")]
    [SerializeField] private int speed;
    [SerializeField] private int damage;

    private Vector2 direction;
    private Animator animator;

    private void Awake() {
        InitializeAnimator();
    }

    private void InitializeAnimator() {
        animator = GetComponent<Animator>();
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

    public void SetDirection(Vector2 newDirection) {
        direction = newDirection.normalized;
    }

    private void DestroyProjectile() {
        Destroy(gameObject);
    }
}
