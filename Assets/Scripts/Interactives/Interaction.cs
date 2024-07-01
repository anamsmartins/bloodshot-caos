using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private List<IInteractive> interactivesInRange = new List<IInteractive>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractive>(out IInteractive interactive))
        {
            if (!interactivesInRange.Contains(interactive))
            {
                interactivesInRange.Add(interactive);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractive>(out IInteractive interactive))
        {
            interactivesInRange.Remove(interactive);
        }
    }

    public void Interact()
    {
        foreach (IInteractive interactive in interactivesInRange)
        {
            interactive.Interact();
        }
    }

    public bool CanInteract()
    {
        return interactivesInRange.Count > 0;
    }
}
