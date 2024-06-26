using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private float weaponDistanceFromPlayer;

    private Shooting shooting;
    private bool isMoving;

    private void Awake() {
        shooting = GetComponent<Shooting>();
    }

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector2 moveDir = new Vector2(inputVector.x, inputVector.y);
        Vector3 movement = moveDir * moveSpeed * Time.deltaTime;

        // Apply the movement to the current position
        transform.position += movement;

        isMoving = moveDir != Vector2.zero;

        // Update the weapon's position and rotation based on mouse position
        Vector2 mousePosition = gameInput.GetMousePosition();
        AimWeapon(mousePosition);

        if (gameInput.IsShooting()) {
            shooting.Shoot(mousePosition);
        }
    }

    private void AimWeapon(Vector2 mousePosition) {
        // Convert mouse position to world space
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = 0f;

        // Calculate direction from player to mouse position
        Vector2 direction = (worldMousePosition - transform.position).normalized;

        // Rotate the weapon to face the mouse position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Set the weapon's position relative to the player
        weaponTransform.position = transform.position + (Vector3)(direction * weaponDistanceFromPlayer);
    }



    public bool IsMoving() {
        return isMoving;
    }
}
