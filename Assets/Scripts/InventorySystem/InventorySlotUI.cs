using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerDownHandler
{
    [Header("UI")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private GameObject selectionHighlight;

    public int SlotIndex { get; private set; }
    private InventoryUI _inventoryUI;

    public void Initialize(int slotIndex, InventoryUI inventoryUI)
    {
        SlotIndex = slotIndex;
        _inventoryUI = inventoryUI;
    }

    public void Refresh(InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty)
        {
            Clear();
            return;
        }

        itemIcon.sprite = slot.Item.Icon;
        itemIcon.enabled = true;
        amountText.text = slot.Amount.ToString();
        amountText.enabled = true;
    }

    public void SetSelected(bool selected)
    {
        if (selectionHighlight != null) selectionHighlight.SetActive(selected);
    }

    private void Clear()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        amountText.text = string.Empty;
        amountText.enabled = false;
    }

    // THE ONLY INPUT WE NEED
    public void OnPointerDown(PointerEventData eventData)
    {
        _inventoryUI.HandleSlotClicked(this);
    }
}