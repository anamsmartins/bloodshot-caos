using UnityEngine;

public class Door : MonoBehaviour {
    public static Door Instance { get; private set; }

    [SerializeField] private GameObject closedDoorPrefab;
    [SerializeField] private GameObject openDoorPrefab;

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

        currentDoor = Instantiate(closedDoorPrefab, transform.position, transform.rotation, transform);
    }

    public void Open() {
        if (currentDoor != null) {
            Destroy(currentDoor);
        }

        currentDoor = Instantiate(openDoorPrefab, transform.position, transform.rotation, transform);
    }
}
