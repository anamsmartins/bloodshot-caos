using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("Enemy")]
    [SerializeField] private int maxHealth;
    private float currentHealth;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;
    [SerializeField] private float avoidanceDistance;

    [Header("Shooting")]
    [SerializeField] private float shotCooldown;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Scoring")]
    [SerializeField] private int scorePerHit;
    [SerializeField] private int scorePerKill;

    [Header("References")]
    [SerializeField] private Player player;
    private Transform playerTransform;

    [Header("Effects")]
    [SerializeField] private GameObject bloodDropPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip hitAudioClip;
    [SerializeField] private AudioClip deathAudioClip;
    private AudioSource audioSource = null;

    private float shotTimer;
    private List<Enemy> allEnemies;
    private Vector2 movementDirection;

    private float movementErrorInterval = 0.001f;

    private Rigidbody2D rb;

    private Animator myAnimator;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        myAnimator = GetComponent<Animator>();
    }

    void Start() {
        FindPlayer();
        shotTimer = shotCooldown;
        currentHealth = maxHealth;
        allEnemies = new List<Enemy>(FindObjectsOfType<Enemy>());
    }
 
    void Update() {
        if (ShouldFlip())
        {
            Flip();
        }

        UpdateMovement();
        HandleShooting();
    }

    private void FindPlayer() {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) {
            playerTransform = playerObject.transform;
            player = playerObject.GetComponent<Player>();
        } else {
            Debug.LogError("Player not found! Make sure the player is tagged 'Player'.");
        }
    }

    private bool ShouldFlip()
    {
        return transform.right.x * movementDirection.x < 0;
    }

    private void Flip()
    {
        transform.right = -transform.right;
    }


    private void UpdateMovement() {
        float previousMovimentPosition = movementDirection.x;
        movementDirection = CalculateMovementDirection();
        MoveAwayFromOtherEnemies();

        if (movementDirection != Vector2.zero && (Mathf.Abs(previousMovimentPosition - movementDirection.x) > movementErrorInterval)) {
            myAnimator.SetBool("IsMoving", true);
            movementDirection.Normalize();
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + movementDirection, speed * Time.deltaTime);
        } else
        {
            myAnimator.SetBool("IsMoving", false);
        }
    }

    private Vector2 CalculateMovementDirection() {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > stoppingDistance)
            return (playerTransform.position - transform.position).normalized;

        if (distanceToPlayer < retreatDistance)
            return (transform.position - playerTransform.position).normalized;

        return Vector2.zero;
    }

    private void MoveAwayFromOtherEnemies() {
        foreach (Enemy otherEnemy in allEnemies) {
            if (otherEnemy == this || otherEnemy == null)
                continue;

            float distanceToEnemy = Vector2.Distance(transform.position, otherEnemy.transform.position);

            if (distanceToEnemy < avoidanceDistance) {
                movementDirection += (Vector2)(transform.position - otherEnemy.transform.position).normalized;
            }
        }
    }

    private void HandleShooting() {
        if (shotTimer <= 0f) {
            Shoot();
            shotTimer = shotCooldown;
        } else {
            shotTimer -= Time.deltaTime;
        }
    }

    private void Shoot() {
        Vector3 adjustedPosition = playerTransform.position + new Vector3(0, 0.5f, 0);
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        EnemyProjectile projectileScript = projectileInstance.GetComponent<EnemyProjectile>();
        projectileScript.Initialize(adjustedPosition);
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        player.AddScore(scorePerHit);
        SpawnBloodDrop();
        PlayHitAudioClip();

        if (currentHealth <= 0) {
            Die();
        } else {
            StartCoroutine(FlashOnDamage());
        }
    }

    private void SpawnBloodDrop() {
        Vector3 positionBehindEnemy = transform.position + new Vector3(0, 0, 1);
        Instantiate(bloodDropPrefab, positionBehindEnemy, Quaternion.identity);
    }

    private IEnumerator FlashOnDamage() {

        myAnimator.SetBool("WasHit", true);
        yield return new WaitForSeconds(0.2f);
        myAnimator.SetBool("WasHit", false);
    }

    private void Die()
    {
        player.AddScore(scorePerKill);
        PlayDeathAudioClip();
        StartCoroutine(AnimationDie());
    }

    private IEnumerator AnimationDie()
    {
        myAnimator.SetBool("IsDead", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void PlayDeathAudioClip() {
        if (deathAudioClip != null) {
            GameObject tempAudio = new GameObject("TempAudio");
            AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
            audioSource.clip = deathAudioClip;
            audioSource.Play();
            Destroy(tempAudio, deathAudioClip.length);
        }
    }

    private void PlayHitAudioClip() {
        audioSource.PlayOneShot(hitAudioClip);
    }
}
