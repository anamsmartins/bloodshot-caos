using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; } = null;

    [SerializeField] private AudioMixer mainAudioMixer = null;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void ChangeMasterVolume(float volume)
    {
        mainAudioMixer.SetFloat("MasterVolume", volume);
    }
}