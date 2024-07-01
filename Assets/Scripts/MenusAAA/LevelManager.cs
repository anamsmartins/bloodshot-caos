using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadWinningScreen()
    {
        //SceneManager.LoadScene("WinningScreen");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
    }

    public void ResumeGame()
    {
        SceneManager.UnloadSceneAsync("PauseMenu");
        Time.timeScale = 1;
    }
}
