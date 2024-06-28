using UnityEngine;

public class BloodDrop : MonoBehaviour {
    [SerializeField] private int bloodAmount;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player player = other.GetComponent<Player>();
            if (player != null) {
                player.CollectBlood(bloodAmount);
                Destroy(gameObject);
            }
        }
    }
}
