using System.Collections;
using UnityEngine;

public class BloodDrop : MonoBehaviour {
    [SerializeField] private int bloodAmount;

    private Animator myAnimator = null;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player player = other.GetComponent<Player>();
            if (player != null) {
                StartCoroutine(PickBloodAnimation());
                player.CollectBlood(bloodAmount);
            }
        }
    }

    private IEnumerator PickBloodAnimation()
    {
        myAnimator.SetBool("Pickup", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
