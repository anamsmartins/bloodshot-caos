using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Player")]
    [SerializeField] private float healthPoints;
    [SerializeField] private GameInput gameInput;

    [Header("Player Movement")]
    [SerializeField] private float moveSpeed = 7f;

    private bool isMoving;
    private Animator myAnimator = null;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector2 moveDir = new Vector2(inputVector.x, inputVector.y);
        Vector3 movement = moveDir * moveSpeed * Time.deltaTime;

        // Apply the movement to the current position
        transform.position += movement;

        isMoving = moveDir != Vector2.zero;

        myAnimator.SetBool("IsMoving", isMoving);

    }


    public bool IsMoving() {
        return isMoving;
    }
}
