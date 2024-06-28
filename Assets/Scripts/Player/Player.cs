using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private GameInput gameInput;

    [Header("Player")]
    [SerializeField] private int maxHealthPoints;
    private int currentHealthPoints;
    [SerializeField] private int bloodTank;
    [SerializeField] private int healCost;
    [SerializeField] private int score = 0;

    [Header("Player Movement")]
    [SerializeField] private int moveSpeed = 7;

    [Header("Score")]
    [SerializeField] private int healScore;
    [SerializeField] private int bloodPickUpScore;

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

    public void TakeDamage(int damageAmount) {
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

    public bool UseBloodForHealing(int healCost) {
        if (bloodTank >= healCost && currentHealthPoints < maxHealthPoints) {
            currentHealthPoints = maxHealthPoints;
            bloodTank -= healCost;
            AddScore(healScore);
            return true;
        }
        return false;
    }

    public void CollectBlood(int amount) {
        bloodTank += amount;
        AddScore(bloodPickUpScore);
    }

    public void AddScore(int amount) {
        score += amount;
    }
}
