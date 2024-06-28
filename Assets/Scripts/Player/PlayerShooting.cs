using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameInput gameInput;

    void Start() {
    }

    void Update() {
        if (gameInput != null && gameInput.IsShooting()) {
            Shoot();
        }
    }

    void Shoot() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - shootPosition.position);

        GameObject projectile = Instantiate(playerProjectilePrefab, shootPosition.position, Quaternion.identity);
        var playerProjectile = projectile.GetComponent<PlayerProjectile>();
        playerProjectile.SetDirection(direction);
    }
}
