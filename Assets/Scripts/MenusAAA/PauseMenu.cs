using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Close()
    {
        levelManager.ResumeGame();
    }

    public void QuitToMainMenu()
    {
        levelManager.LoadMainMenu();
    }
}
