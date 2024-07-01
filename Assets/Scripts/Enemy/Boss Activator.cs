using UnityEngine;

public class BossActivator : MonoBehaviour {
    [SerializeField] private GameObject boss; 
    [SerializeField] private float bossSpeed;
    [SerializeField] private float bossShotCooldown;
    [SerializeField] private Door door;

    private Enemy bossEnemyScript;

    private void Start() {
        bossEnemyScript = boss.GetComponent<Enemy>();
        door.SetBossGameObject(boss);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            bossEnemyScript.SetSpeed(bossSpeed);
            bossEnemyScript.CanShoot(true);
        }
    }
}
