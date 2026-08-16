using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Camera gameplayCamera;

    [Header("UI System")]
    [SerializeField] private GameObject inventoryMenuPanel; // Drag your whole menu background here to hide it
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private InventorySlotUI slotPrefab;

    [Header("World Placement Cursor")]
    [SerializeField] private Image floatingCursorIcon; // A single UI Image on your Canvas for the mouse

    private readonly List<InventorySlotUI> _slotViews = new();
    private int _heldSlotIndex = -1;
    private float _pickupTime; // Prevents instantly dropping the item on the same click

    private void Start()
    {
        CreateSlotViews();
        if (floatingCursorIcon != null) floatingCursorIcon.enabled = false;
    }

    // Called by the slot when clicked
    public void HandleSlotClicked(InventorySlotUI clickedSlot)
    {
        InventorySlot slotData = inventory.GetSlot(clickedSlot.SlotIndex);

        // If the slot actually has an item in it
        if (slotData != null && !slotData.IsEmpty && _heldSlotIndex == -1)
        {
            // 1. Pick it up
            _heldSlotIndex = clickedSlot.SlotIndex;
            floatingCursorIcon.sprite = slotData.Item.Icon;
            floatingCursorIcon.enabled = true;

            // 2. Hide the menu
            if (inventoryMenuPanel != null) inventoryMenuPanel.SetActive(false);

            // 3. Mark the time so we don't accidentally drop it in the exact same frame
            _pickupTime = Time.time;
        }
    }

    private void Update()
    {
        // --- PRESS SPACE TO TOGGLE OR CANCEL ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleInventory();
        }

        // If we are holding an item, attach it to the mouse
        if (_heldSlotIndex != -1 && floatingCursorIcon.enabled)
        {
            floatingCursorIcon.transform.position = Input.mousePosition;

            // Wait for the next mouse click to place it
            if (Input.GetMouseButtonDown(0) && Time.time > _pickupTime + 0.2f)
            {
                PlaceItemInWorld();
            }
        }
    }

    private void ToggleInventory()
    {
        if (inventoryMenuPanel != null)
        {
            bool isActive = inventoryMenuPanel.activeSelf;

            if (isActive)
            {
                // If the menu is open and we are holding an item, DUMP IT / CANCEL HELD STATE
                if (_heldSlotIndex != -1)
                {
                    _heldSlotIndex = -1;
                    if (floatingCursorIcon != null) floatingCursorIcon.enabled = false;
                    RefreshAll();
                    Debug.Log("Canceled placement and returned item to inventory.");
                }

                // Close the menu
                inventoryMenuPanel.SetActive(false);
            }
            else
            {
                // Open the menu
                //inventoryMenuPanel.SetActive(true);
                //RefreshAll();
            }
        }
    }



    private void PlaceItemInWorld()
    {
        // 1. Shoot the laser from the camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPosition = Vector3.zero;
        bool hitSomething = false;

        // 2. PHYSICS RAYCAST: Check if the laser hits a physical 3D object in the scene
        // (1000f is just the maximum distance the laser will travel)
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f))
        {
            // The laser hit an object! The exact impact point is our spawn location.
            worldPosition = hitInfo.point;

            // Optional: If you still want to force it to exactly Z = 0 after hitting the background
            //worldPosition.z = 0;

            hitSomething = true;
            Debug.Log($"Raycast hit '{hitInfo.collider.gameObject.name}' exactly at {worldPosition}");
        }
        else
        {
            Debug.LogWarning("Click missed! The raycast didn't hit any Colliders. Placement canceled.");
        }

        // 3. Only consume and spawn if we actually hit a valid surface
        if (hitSomething)
        {
            InventorySlot slotData = inventory.GetSlot(_heldSlotIndex);
            GameObject prefabToSpawn = null;

            if (slotData != null && !slotData.IsEmpty)
            {
                prefabToSpawn = slotData.Item.Prefab;
            }

            if (inventory.TryConsumeItem(_heldSlotIndex))
            {
                if (prefabToSpawn != null)
                {
                    Quaternion spawnRotation = Quaternion.Euler(0f, slotData.Item.SpawnYRotation, 0f);
                    Instantiate(prefabToSpawn, worldPosition, spawnRotation);
                    Debug.Log($"Successfully spawned {prefabToSpawn.name} at {worldPosition}!");
                }
            }
        }

        // 4. Reset the cursor and show the menu again (whether we successfully placed it or canceled)
        _heldSlotIndex = -1;
        floatingCursorIcon.enabled = false;

        if (inventoryMenuPanel != null) inventoryMenuPanel.SetActive(true);

        RefreshAll();
    }

    // --- STANDARD UI SETUP LOGIC ---

    private void OnEnable()
    {
        if (inventory == null) return;
        inventory.SlotSelected += HandleSlotSelected;
        inventory.ItemAmountChanged += HandleItemAmountChanged;
        RefreshAll();
    }

    private void OnDisable()
    {
        if (inventory == null) return;
        inventory.SlotSelected -= HandleSlotSelected;
        inventory.ItemAmountChanged -= HandleItemAmountChanged;
    }

    private void CreateSlotViews()
    {
        if (inventory == null || slotsContainer == null || slotPrefab == null) return;
        ClearExistingViews();

        for (int i = 0; i < inventory.Slots.Count; i++)
        {
            InventorySlotUI slotView = Instantiate(slotPrefab, slotsContainer);
            slotView.Initialize(i, this); // Hook up the reference
            _slotViews.Add(slotView);
            slotView.Refresh(inventory.GetSlot(i));
        }
    }

    private void RefreshAll()
    {
        for (int i = 0; i < _slotViews.Count; i++) RefreshSlot(i);
    }

    private void HandleSlotSelected(int slotIndex) { }
    private void HandleItemAmountChanged(int slotIndex, int amount) => RefreshSlot(slotIndex);

    private void RefreshSlot(int slotIndex)
    {
        if (!IsValidViewIndex(slotIndex)) return;
        _slotViews[slotIndex].Refresh(inventory.GetSlot(slotIndex));
    }

    private void ClearExistingViews()
    {
        foreach (Transform child in slotsContainer) Destroy(child.gameObject);
        _slotViews.Clear();
    }

    private bool IsValidViewIndex(int index) => index >= 0 && index < _slotViews.Count;
}