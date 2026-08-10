using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputReader : MonoBehaviour, IInputProvider
{
    private InputSystem_Actions inputActions;

    public Vector2 MoveInput => inputActions.Camera.Move.ReadValue<Vector2>();
    public bool SimulatePressed => inputActions!= null && inputActions.Camera.Simulate.triggered;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
}