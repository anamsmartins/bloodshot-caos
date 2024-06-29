using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; } = null;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}