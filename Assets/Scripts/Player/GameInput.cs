using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public Vector2 GetShootVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.ShootJoystick.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public bool IsShooting() {
        return playerInputActions.Player.Shoot.triggered;
    }

    public bool IsShootingJoystick()
    {
        return playerInputActions.Player.ShootJoystick.IsPressed();
    }

    public Vector2 GetMousePosition() {
        return Mouse.current.position.ReadValue();
    }

    public bool IsHealing() {
        return playerInputActions.Player.Heal.triggered;
    }
}
