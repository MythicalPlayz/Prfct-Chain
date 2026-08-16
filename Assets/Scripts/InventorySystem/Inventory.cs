using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private InventoryData inventoryData;

    private readonly List<InventorySlot> _slots = new();

    private int _selectedSlotIndex = -1;

    public int SelectedSlotIndex => _selectedSlotIndex;

    public IReadOnlyList<InventorySlot> Slots => _slots;

    public InventorySlot SelectedSlot
    {
        get
        {
            if (!IsValidSlotIndex(_selectedSlotIndex))
            {
                return null;
            }

            return _slots[_selectedSlotIndex];
        }
    }

    public event Action<int> SlotSelected;
    public event Action<int, int> ItemAmountChanged;
    public event Action InventoryChanged;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _slots.Clear();

        if (inventoryData == null)
        {
            Debug.LogError("Inventory: InventoryData is missing.", this);
            return;
        }

        foreach (InventoryData.InventorySlotData slotData in inventoryData.Slots)
        {
            _slots.Add(
                new InventorySlot(
                    slotData.Item,
                    slotData.Amount
                )
            );
        }
    }

    public bool SelectSlot(int slotIndex)
    {
        if (!IsValidSlotIndex(slotIndex))
        {
            return false;
        }

        _selectedSlotIndex = slotIndex;

        SlotSelected?.Invoke(slotIndex);

        return true;
    }

    public bool TryConsumeSelectedItem()
    {
        if (!IsValidSlotIndex(_selectedSlotIndex))
        {
            return false;
        }

        return TryConsumeItem(_selectedSlotIndex);
    }

    public bool TryConsumeItem(int slotIndex)
    {
        if (!IsValidSlotIndex(slotIndex))
        {
            return false;
        }

        InventorySlot slot = _slots[slotIndex];

        if (!slot.TryConsume())
        {
            return false;
        }

        ItemAmountChanged?.Invoke(
            slotIndex,
            slot.Amount
        );

        InventoryChanged?.Invoke();

        return true;
    }

    public InventorySlot GetSlot(int slotIndex)
    {
        if (!IsValidSlotIndex(slotIndex))
        {
            return null;
        }

        return _slots[slotIndex];
    }

    private bool IsValidSlotIndex(int slotIndex)
    {
        return slotIndex >= 0 &&
               slotIndex < _slots.Count;
    }

    // --- MOVED THIS HERE ---
    public void SwapSlots(int index1, int index2)
    {
        // Safety check to prevent out-of-bounds errors
        if (!IsValidSlotIndex(index1) || !IsValidSlotIndex(index2)) return;

        // Temporarily store the first slot's data
        InventorySlot temp = _slots[index1];

        // Swap them in the backend list
        _slots[index1] = _slots[index2];
        _slots[index2] = temp;

        // Let the rest of the game know the inventory updated
        InventoryChanged?.Invoke();
    }
}


[Serializable]
public class InventorySlot
{
    public InventoryItemData Item { get; }

    public int Amount { get; private set; }

    public bool IsEmpty =>
        Item == null || Amount <= 0;

    public InventorySlot(
        InventoryItemData item,
        int amount)
    {
        Item = item;
        Amount = Mathf.Max(0, amount);
    }

    public bool TryConsume()
    {
        if (IsEmpty)
        {
            return false;
        }

        Amount--;

        return true;
    }
}