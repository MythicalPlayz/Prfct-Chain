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

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _slots.Clear();

        if (inventoryData == null)
        {
            Debug.LogError(
                "Inventory: InventoryData is missing.",
                this
            );

            return;
        }

        foreach (InventoryData.InventorySlotData slotData
                 in inventoryData.Slots)
        {
            if (slotData.Item == null)
            {
                continue;
            }

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

        InventorySlot slot = _slots[slotIndex];

        if (slot.IsEmpty)
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
}

public class InventorySlot
{
    public InventoryItemData Item { get; }

    public int Amount { get; private set; }

    public bool IsEmpty => Amount <= 0;

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