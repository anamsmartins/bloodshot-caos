using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {
    public static Door Instance { get; private set; }

    [SerializeField] private GameObject closedDoorPrefab;
    [SerializeField] private GameObject openDoorPrefab;
    [SerializeField] private bool open = false;

    private GameObject currentDoor;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            InitializeDoor();
        } else {
            Destroy(gameObject);
        }
    }

    private void InitializeDoor() {
        if (currentDoor != null) {
            Destroy(currentDoor);
        }

        if (!open) {
            currentDoor = Instantiate(closedDoorPrefab, transform.position, transform.rotation, transform);
        } else {
            currentDoor = Instantiate(openDoorPrefab, transform.position, transform.rotation, transform);
        }
    }

    public void Open() {
        if (currentDoor != null) {
            Destroy(currentDoor);
        }

        open = true;
        currentDoor = Instantiate(openDoorPrefab, transform.position, transform.rotation, transform);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (open) {
                SavePlayerStats(collision.GetComponent<Player>());
                LoadNextScene();
            }
        }
    }

    private void SavePlayerStats(Player player) {
        player.SendMessage("SavePlayerStats");
    }

    private void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
