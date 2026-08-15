using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory;

    [Header("UI")]
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private InventorySlotUI slotPrefab;

    private readonly List<InventorySlotUI> _slotViews = new();

    private void Start()
    {
        CreateSlotViews();
    }

    private void OnEnable()
    {
        if (inventory == null)
        {
            return;
        }

        inventory.SlotSelected += HandleSlotSelected;
        inventory.ItemAmountChanged += HandleItemAmountChanged;

        RefreshAll();
    }

    private void OnDisable()
    {
        if (inventory == null)
        {
            return;
        }

        inventory.SlotSelected -= HandleSlotSelected;
        inventory.ItemAmountChanged -= HandleItemAmountChanged;
    }

    private void CreateSlotViews()
    {
        if (inventory == null ||
            slotsContainer == null ||
            slotPrefab == null)
        {
            return;
        }

        ClearExistingViews();

        for (int i = 0; i < inventory.Slots.Count; i++)
        {
            InventorySlotUI slotView =
                Instantiate(slotPrefab, slotsContainer);

            slotView.Initialize(i);

            _slotViews.Add(slotView);
        }
    }

    private void RefreshAll()
    {
        for (int i = 0; i < _slotViews.Count; i++)
        {
            RefreshSlot(i);
        }

        UpdateSelection();
    }

    private void HandleSlotSelected(int slotIndex)
    {
        UpdateSelection();
    }

    private void HandleItemAmountChanged(
        int slotIndex,
        int amount)
    {
        RefreshSlot(slotIndex);
    }

    private void RefreshSlot(int slotIndex)
    {
        if (!IsValidViewIndex(slotIndex))
        {
            return;
        }

        InventorySlot slot = inventory.GetSlot(slotIndex);

        _slotViews[slotIndex].Refresh(slot);
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < _slotViews.Count; i++)
        {
            bool selected =
                i == inventory.SelectedSlotIndex;

            _slotViews[i].SetSelected(selected);
        }
    }

    private void ClearExistingViews()
    {
        foreach (Transform child in slotsContainer)
        {
            Destroy(child.gameObject);
        }

        _slotViews.Clear();
    }

    private bool IsValidViewIndex(int index)
    {
        return index >= 0 &&
               index < _slotViews.Count;
    }
}