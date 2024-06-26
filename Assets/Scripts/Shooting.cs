using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
    [SerializeField] public Weapon weapon;

    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }

    public void Shoot(Vector2 mousePosition) {
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = 0f;
        
        Vector2 direction = (worldMousePosition - transform.position).normalized;

        weapon.Aim(direction);
        weapon.Shoot();
    }
}
