using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunePillar : MonoBehaviour, IInteractive
{
    [Header("Audio")]
    [SerializeField] private AudioClip interactionSound = null;

    [Header("Player")]
    [SerializeField] private Player player = null;

    private AudioSource myAudioSource = null;
    private Canvas textCanvas = null;
    private SpriteRenderer glowObject = null;
    private bool hasBeenUsed = false;

    private void Awake()
    {
        glowObject = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        textCanvas = gameObject.transform.GetChild(2).GetComponent<Canvas>();
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        HideInteractionOpportunity();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenUsed)
        {
            ShowInteractionOpportunity();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!hasBeenUsed)
        {
            HideInteractionOpportunity();
        }
    }

    public void Interact()
    {
        Debug.Log(name + ": Interact");
        myAudioSource.PlayOneShot(interactionSound);
        hasBeenUsed = true;
        HideInteractionOpportunity();
        player.UpdateBloodTankToMax();
    }

    public void ShowInteractionOpportunity()
    {
        textCanvas.enabled = true;
        glowObject.enabled = true;
    }

    public void HideInteractionOpportunity()
    {
        textCanvas.enabled = false;
        glowObject.enabled = false;
    }
}
