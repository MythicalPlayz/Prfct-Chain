using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory;

    private InventoryInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new InventoryInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Inventory.slot1.performed += OnSlot1Pressed;
        _inputActions.Inventory.slot2.performed += OnSlot2Pressed;
        _inputActions.Inventory.slot3.performed += OnSlot3Pressed;
        _inputActions.Inventory.slot4.performed += OnSlot4Pressed;
        _inputActions.Inventory.slot5.performed += OnSlot5Pressed;

        _inputActions.Inventory.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Inventory.slot1.performed -= OnSlot1Pressed;
        _inputActions.Inventory.slot2.performed -= OnSlot2Pressed;
        _inputActions.Inventory.slot3.performed -= OnSlot3Pressed;
        _inputActions.Inventory.slot4.performed -= OnSlot4Pressed;
        _inputActions.Inventory.slot5.performed -= OnSlot5Pressed;

        _inputActions.Inventory.Disable();
    }

    private void OnSlot1Pressed(InputAction.CallbackContext context)
    {
        SelectSlot(0);
    }

    private void OnSlot2Pressed(InputAction.CallbackContext context)
    {
        SelectSlot(1);
    }

    private void OnSlot3Pressed(InputAction.CallbackContext context)
    {
        SelectSlot(2);
    }

    private void OnSlot4Pressed(InputAction.CallbackContext context)
    {
        SelectSlot(3);
    }

    private void OnSlot5Pressed(InputAction.CallbackContext context)
    {
        SelectSlot(4);
    }

    private void SelectSlot(int slotIndex)
    {
        if (inventory == null)
        {
            return;
        }

        inventory.SelectSlot(slotIndex);
    }
}