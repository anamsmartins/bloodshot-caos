using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Player player;

    [Header("Ammunition")]
    [SerializeField] private int shotCost;
    [SerializeField] private int shotScore;

    void Update() {
        if (gameInput != null && gameInput.IsShooting()) {
            Shoot();
        }
    }

    void Shoot() {
        if (player != null && player.UseBloodForShooting(shotCost)) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - shootPosition.position).normalized;

            GameObject projectile = Instantiate(playerProjectilePrefab, shootPosition.position, Quaternion.identity);
            var playerProjectile = projectile.GetComponent<PlayerProjectile>();
            playerProjectile.SetDirection(direction);

            player.AddScore(shotScore);
        } else {
            Debug.Log("Not enough blood in the tank to shoot!");
        }
    }
}
