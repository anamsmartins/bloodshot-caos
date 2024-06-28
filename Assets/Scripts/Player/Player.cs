using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private GameInput gameInput;

    [Header("Player")]
    [SerializeField] private float maxHealthPoints;
    private float currentHealthPoints;
    [SerializeField] private float bloodTank;
    [SerializeField] private float healCost;

    [Header("Player Movement")]
    [SerializeField] private float moveSpeed = 7f;

    private bool isMoving;

    void Start() {
        currentHealthPoints = maxHealthPoints;
    }

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector2 moveDir = new Vector2(inputVector.x, inputVector.y);
        Vector3 movement = moveDir * moveSpeed * Time.deltaTime;

        // Apply the movement to the current position
        transform.position += movement;

        isMoving = moveDir != Vector2.zero;

        if (gameInput.IsHealing()) {
            UseBloodForHealing(healCost);
        }
    }

    public bool IsMoving() {
        return isMoving;
    }

    public void TakeDamage(float damageAmount) {
        currentHealthPoints -= damageAmount;
        StartCoroutine(FlashOnDamage());
        if (currentHealthPoints <= 0) {
            Die();
        }
    }

    private IEnumerator FlashOnDamage() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) {
            renderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            renderer.color = Color.white;
        }
    }

    private void Die() {
        Destroy(gameObject);
    }

    public bool UseBloodForShooting(int amount) {
        if (bloodTank >= amount) {
            bloodTank -= amount;
            return true;
        }
        return false;
    }

    public bool UseBloodForHealing(float healCost) {
        if (bloodTank >= healCost && currentHealthPoints < maxHealthPoints) {
            currentHealthPoints = maxHealthPoints;
            bloodTank -= healCost;
            return true;
        }
        return false;
    }
}
