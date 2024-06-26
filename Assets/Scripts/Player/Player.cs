using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private bool isMoving;

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector2 moveDir = new Vector2(inputVector.x, inputVector.y);
        Vector3 movement = moveDir * moveSpeed * Time.deltaTime;
        
        // Apply the movement to the current position
        transform.position += movement;
        
        isMoving = moveDir != Vector2.zero;
    }

    public bool IsMoving() {
        return isMoving;
    }
}
