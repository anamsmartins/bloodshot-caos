using System.Collections;
using UnityEngine;

public class BloodDrop : MonoBehaviour {
    [SerializeField] private int bloodAmount;
    [SerializeField] private AudioClip bloodPickUpAudioClip;

    private Animator myAnimator = null;
    private AudioSource audioSource = null;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player player = other.GetComponent<Player>();
            if (player != null) {
                StartCoroutine(PickBloodAnimation());
                player.CollectBlood(bloodAmount);
                PlayBloodPickUpAudioClip();
            }
        }
    }

    private IEnumerator PickBloodAnimation()
    {
        myAnimator.SetBool("Pickup", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void PlayBloodPickUpAudioClip() {
        audioSource.PlayOneShot(bloodPickUpAudioClip);
    }
}
