using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractive
{
    [Header("Audio")]
    [SerializeField] private AudioClip interactionSound = null;

    private AudioSource myAudioSource = null;
    private Canvas textCanvas = null;

    private void Awake()
    {
        textCanvas = gameObject.transform.GetChild(0).GetComponent<Canvas>();
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        HideInteractionOpportunity();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       ShowInteractionOpportunity();

    }

    private void OnTriggerExit2D(Collider2D other)
    {
       HideInteractionOpportunity();
    }

    public void Interact()
    {
        myAudioSource.PlayOneShot(interactionSound);
        // DO SOMETHING
    }

    public void ShowInteractionOpportunity()
    {
        textCanvas.enabled = true;
    }

    public void HideInteractionOpportunity()
    {
        textCanvas.enabled = false;
    }
}
