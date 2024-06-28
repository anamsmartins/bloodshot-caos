using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameInput gameInput;

    void Start() {
        if (playerProjectilePrefab == null) {
            Debug.LogError("PlayerProjectilePrefab is not assigned!");
        }
        if (shootPosition == null) {
            Debug.LogError("ShootPosition is not assigned!");
        }
        if (gameInput == null) {
            Debug.LogError("GameInput is not assigned!");
        }
    }

    void Update() {
        if (gameInput != null && gameInput.IsShooting()) {
            Shoot();
        }
    }

    void Shoot() {
        if (shootPosition == null) {
            Debug.LogError("ShootPosition is null in Shoot() method!");
            return;
        }
        if (playerProjectilePrefab == null) {
            Debug.LogError("PlayerProjectilePrefab is null in Shoot() method!");
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - shootPosition.position);

        GameObject projectile = Instantiate(playerProjectilePrefab, shootPosition.position, Quaternion.identity);
        var playerProjectile = projectile.GetComponent<PlayerProjectile>();
        if (playerProjectile == null) {
            Debug.LogError("PlayerProjectile component is missing on the projectile prefab!");
            return;
        }
        playerProjectile.SetDirection(direction);
    }
}
