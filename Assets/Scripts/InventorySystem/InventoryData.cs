using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "InventoryData",
    menuName = "Prfct Chain/Inventory/Inventory Data"
)]
public class InventoryData : ScriptableObject
{
    [Serializable]
    public class InventorySlotData
    {
        [SerializeField] private InventoryItemData item;

        [Min(0)]
        [SerializeField] private int amount;

        public InventoryItemData Item => item;
        public int Amount => amount;
    }

    [SerializeField] private List<InventorySlotData> slots = new();

    public IReadOnlyList<InventorySlotData> Slots => slots;
}