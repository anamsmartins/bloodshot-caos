using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public void LoadRetryScene(string sceneName) {
        PlayerStats.Score = 0;
        PlayerStats.BloodTank = 30;
        PlayerStats.Health = 5;

        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
