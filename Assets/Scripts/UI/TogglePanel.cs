using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleElementVisibility : MonoBehaviour
{
    [SerializeField] private GameObject ScoreTextPanel; // Reference to the panel you want to show/hide

    public void ToggleVisibility()
    {
        ScoreTextPanel.SetActive(!ScoreTextPanel.activeSelf);
    }
}
