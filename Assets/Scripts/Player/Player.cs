using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class Player : MonoBehaviour {
    [Header("Player")]
    [SerializeField] private int moveSpeed = 7;

    [Header("Blood Tank")]
    [SerializeField] private float currentBloodTank = 30f;
    [SerializeField] private float maxBloodTank = 30f;
    [SerializeField] private Image bloodTankImage = null;

    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int healCost;
    [SerializeField] private GameObject heartsGameObject = null;

    [Header("Scoring")]
    [SerializeField] private int score = 0;
    [SerializeField] private int healScore;
    [SerializeField] private int bloodPickupScore;
    [SerializeField] private GameObject gameScorePanel = null;
    [SerializeField] private float scoreShowHideInterval = 1f;

    [Header("Shooting")]
    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private int ammoCost = 5;
    [SerializeField] public float shootingInterval = 0.3f;

    [Header("Melee Attack")]
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private int meleeDamage = 10;

    [Header("References")]
    [SerializeField] private GameInput gameInput;

    [Header("Audio")]
    [SerializeField] private AudioClip hitAudioClip;
    [SerializeField] private AudioClip deathAudioClip;
    [SerializeField] private AudioClip healAudioClip;
    private AudioSource audioSource = null;

    private int currentHealth;

    private bool isMoving;
    private bool isMovingHorizontally;
    private bool isMovingUp;

    private Animator myAnimator;
    private float horizontalDirection = 0;
    private float verticalDirection = 0;

    private bool canShoot = true;

    private TMP_Text gameScoreText = null;
    private Coroutine fadeGameScoreCoroutine = null;

    private Rigidbody2D rb;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        gameScoreText = gameScorePanel.GetComponentInChildren<TMP_Text>();

        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


    void Start() {
        LoadPlayerStats();
        UpdateBloodTankUI(currentBloodTank);
        UpdateScoreText(score);
        UpdateHearts();
    }

    private void Update() {
        HandleMovement();
        HandleHealing();
        HandleAttacks();
        HandleAnimate();
    }

    private void HandleAnimate() {
        myAnimator.SetBool("IsMoving", isMoving);
        myAnimator.SetBool("IsMovingHorizontally", isMovingHorizontally);
        myAnimator.SetBool("IsMovingUp", isMovingUp);
    }


    private bool ShouldFlip()
    {
        return transform.right.x * horizontalDirection < 0;
    }

    private void Flip()
    {
        transform.right = -transform.right;
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        UpdateDirection(inputVector);
        MoveCharacter(inputVector);
        UpdateMovementStates(inputVector);

        if (ShouldFlipCharacter()) {
            Flip();
        }
    }

    private void UpdateDirection(Vector2 inputVector) {
        horizontalDirection = inputVector.x;
        verticalDirection = inputVector.y;
    }

    private void MoveCharacter(Vector2 inputVector) {
        Vector3 movement = new Vector3(inputVector.x, inputVector.y, 0) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void UpdateMovementStates(Vector2 inputVector) {
        isMoving = inputVector != Vector2.zero;
        isMovingHorizontally = Mathf.Abs(inputVector.y) < Mathf.Abs(inputVector.x);
        isMovingUp = (Mathf.Abs(inputVector.y) > Mathf.Abs(inputVector.x)) && inputVector.y > 0;
    }

    private bool ShouldFlipCharacter() {
        return isMovingHorizontally && ShouldFlip();
    }

    private void UpdateBloodTankUI(float currentAmount)
    {
        bloodTankImage.fillAmount = currentAmount / maxBloodTank;
    }

    private IEnumerator UpdateBloodTankOverTime(float amount, string method)
    {

        float elapsedTime = 0f;
        float targetBloodTank = currentBloodTank;
        float previousBloodTank;
        if (method == "decrease")
        {
            previousBloodTank = Mathf.Max(0f, targetBloodTank + amount);
        } else
        {
            previousBloodTank = Mathf.Min(30f, targetBloodTank - amount);
        }
        float duration = shootingInterval;

        while (elapsedTime < duration)
        {
            previousBloodTank = Mathf.Lerp(previousBloodTank, targetBloodTank, elapsedTime / duration);
            
            UpdateBloodTankUI(previousBloodTank);

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        //previousBloodTank = targetBloodTank;
    }

    private void HandleHealing() {
        if (gameInput.IsHealing()) {
            TryHeal();
        }
    }

    private void HandleAttacks() {
        if (canShoot)
        {
            if (gameInput.IsShooting()) {
                StartCoroutine(ShootOrMelee("button"));
            } else if (gameInput.IsShootingJoystick())
            {
                StartCoroutine(ShootOrMelee("joystick"));
            }
        }
        
    }

    private IEnumerator ShootOrMelee(string method) {
        canShoot = false;

        if (UseBloodForShooting(ammoCost)) {
            myAnimator.SetBool("IsShooting", true);
            if (method == "button")
            {
                Shoot();
            }
            else
            {
                ShootJoystick();    
            }
        }
        else {
            MeleeAttack();
        }

        yield return new WaitForSeconds(shootingInterval);

        canShoot = true;
    }

    private void Shoot() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        GameObject projectileInstance = Instantiate(playerProjectilePrefab, spawnPosition, Quaternion.identity);
        PlayerProjectile projectileScript = projectileInstance.GetComponent<PlayerProjectile>();
        projectileScript.Initialize(direction);
    }

    private void ShootJoystick() {
        Vector2 shootDirections = gameInput.GetShootVectorNormalized();
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        GameObject projectileInstance = Instantiate(playerProjectilePrefab, spawnPosition, Quaternion.identity);
        PlayerProjectile projectileScript = projectileInstance.GetComponent<PlayerProjectile>();
        projectileScript.Initialize(shootDirections);
    }


    private void MeleeAttack() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, meleeRange);

        foreach (Collider2D enemy in hitEnemies) {
            if (enemy.CompareTag("Enemy")) {
                Enemy enemyEntity = enemy.GetComponent<Enemy>();
                if (enemyEntity != null) {
                    enemyEntity.TakeDamage(meleeDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = UnityEngine.Color.red;
        Vector3 newPosition = transform.position + new Vector3(0, 0.5f, 0);
        Gizmos.DrawWireSphere(newPosition, meleeRange);
    }

    public bool IsMoving() {
        return isMoving;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        UpdateHearts();
        StartCoroutine(FlashOnDamage());


        if (currentHealth <= 0) {
            Die();
        }

        PlayHitAudioClip();
    }

    private IEnumerator FlashOnDamage() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) {
            renderer.color = UnityEngine.Color.red;
            yield return new WaitForSeconds(0.1f);
            renderer.color = UnityEngine.Color.white;
        }
    }

    private IEnumerator DieAnimation()
    {
        myAnimator.SetBool("Died", true);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void Die() {
        //Destroy(gameObject);
        PlayDeathAudioClip();
        StartCoroutine(DieAnimation());
    }

    private void PlayDeathAudioClip()
    {
        if (deathAudioClip != null)
        {
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

    private void playHealAudioClip() {
        audioSource.PlayOneShot(healAudioClip);
    }

    private void UpdateScoreText(int score)
    {
        gameScoreText.text = "Score: " + score.ToString();

        if (!gameScorePanel.activeSelf || fadeGameScoreCoroutine != null)
        {
            float startingAlpha = 0f;
            if (fadeGameScoreCoroutine != null)
            {
                StopCoroutine(fadeGameScoreCoroutine);
                startingAlpha = gameScoreText.GetComponent<TextMeshProUGUI>().alpha;
            }
            fadeGameScoreCoroutine = StartCoroutine(ShowScoreTextForTimeInterval(startingAlpha));
        }
    }

    private IEnumerator ShowScoreTextForTimeInterval(float startingAlpha)
    {   
        gameScorePanel.SetActive(true);

        TextMeshProUGUI gameScoreUGUI = gameScoreText.GetComponent<TextMeshProUGUI>();
        gameScoreUGUI.CrossFadeAlpha(startingAlpha, 0f, false);
        gameScoreUGUI.CrossFadeAlpha(1f, scoreShowHideInterval/2, false);
        yield return new WaitForSeconds(scoreShowHideInterval);

        gameScoreUGUI.CrossFadeAlpha(0f, scoreShowHideInterval/2, false);
        yield return new WaitForSeconds(scoreShowHideInterval);
        
        gameScorePanel.SetActive(false);
        gameScoreUGUI.CrossFadeAlpha(1f, 0f, false);
        fadeGameScoreCoroutine = null;
    }

    private void UpdateHearts()
    {
        Image[] heartImages = heartsGameObject.GetComponentsInChildren<Image>();

        for (int i = maxHealth-1; i > -1; i--)
        {
            if (i < currentHealth)
            {
                ShowHeart(heartImages[i], 1);
            }
            else
            {
                ShowHeart(heartImages[i], 0);
            }
        }
    }

    private void ShowHeart(Image heartImage, int alpha)
    {
        UnityEngine.Color invisibleColor = heartImage.color;
        invisibleColor.a = alpha;
        heartImage.color = invisibleColor;
    }

    public bool UseBloodForShooting(int amount) {
        if (currentBloodTank >= amount) {
            currentBloodTank -= amount;
            StartCoroutine(UpdateBloodTankOverTime(amount, "decrease"));
            return true;
        }
        return false;
    }

    private void TryHeal() {
        if (currentBloodTank >= healCost && currentHealth < maxHealth) {
            currentHealth = maxHealth;
            currentBloodTank -= healCost;
            AddScore(healScore);
            playHealAudioClip();
            UpdateHearts();
        }
    }

    public void CollectBlood(int amount) {
        var previousBloodTank = currentBloodTank;
        currentBloodTank = Mathf.Min(maxBloodTank, currentBloodTank + amount);
        AddScore(bloodPickupScore);
        StartCoroutine(UpdateBloodTankOverTime(currentBloodTank-previousBloodTank, "increase"));
    }

    public void AddScore(int amount) {
        score += amount;
        UpdateScoreText(score);
    }

    private void SavePlayerStats() {
        PlayerStats.Score = score;
        PlayerStats.Health = currentHealth;
        PlayerStats.BloodTank = currentBloodTank;
    }

    private void LoadPlayerStats() {
        score = PlayerStats.Score;
        currentHealth = PlayerStats.Health > 0 ? PlayerStats.Health : maxHealth;
        currentBloodTank = PlayerStats.BloodTank > 0 ? PlayerStats.BloodTank : maxBloodTank;
    }

    private void OnDisable() {
        SavePlayerStats();
    }
}
