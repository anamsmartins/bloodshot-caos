using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleElementVisibility : MonoBehaviour
{
    [SerializeField] private GameObject ScoreTextPanel;
    [SerializeField] private GameObject PauseButton;

    private bool isShowing = false;

    public void ToggleVisibility()
    {
        isShowing = !isShowing;
        ScoreTextPanel.SetActive(isShowing);
        PauseButton.SetActive(isShowing);
    }
}
