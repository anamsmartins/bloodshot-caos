using UnityEngine;

public class BossActivator : MonoBehaviour {
    [SerializeField] private GameObject boss; 
    [SerializeField] private float bossSpeed;
    [SerializeField] private float bossShotCooldown;

    private Enemy bossEnemyScript;

    private void Start() {
        bossEnemyScript = boss.GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            bossEnemyScript.SetSpeed(bossSpeed);
            bossEnemyScript.CanShoot(true);
        }
    }
}
