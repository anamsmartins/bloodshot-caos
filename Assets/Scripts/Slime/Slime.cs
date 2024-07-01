using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveDuration = 4f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 5f;

    private Collider2D collider2D = null;
    private Rigidbody2D rigidbody2D = null;
    private Animator myAnimator = null;

    private bool isSleeping = true;

    private float moveEndTime;
    private float moveSpeed;
    public Vector3 moveDirection = Vector3.zero;


    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        moveEndTime = -1f;
    }

    private void Update()
    {
        if (!isSleeping)
        {
            if (ShouldFlip())
            {
                Flip();
            }
            MoveAround();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isSleeping = false;
            moveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            moveSpeed = Random.Range(minSpeed, maxSpeed);

            moveEndTime = Time.time + moveDuration;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MapBoundary"))
        {
            moveDirection = -moveDirection;
            Flip();
        }
    }

    private bool ShouldFlip()
    {
        return transform.right.x * moveDirection.x > 0;
    }

    private void Flip()
    {
        transform.right = -transform.right;
    }

    private void MoveAround()
    {
        myAnimator.SetBool("IsMoving", true);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (Time.time >= moveEndTime)
        {
            myAnimator.SetBool("IsMoving", false);
            isSleeping = true;
            moveDirection = Vector3.zero;
        }
    }
}
