using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private GameObject selectionHighlight;

    private int _slotIndex;

    public void Initialize(int slotIndex)
    {
        _slotIndex = slotIndex;
    }

    public void Refresh(InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty)
        {
            Clear();
            return;
        }

        itemIcon.sprite = slot.Item.Icon;
        itemIcon.enabled = slot.Item.Icon != null;

        amountText.text = slot.Amount.ToString();
        amountText.enabled = true;
    }

    public void SetSelected(bool selected)
    {
        if (selectionHighlight != null)
        {
            selectionHighlight.SetActive(selected);
        }
    }

    private void Clear()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;

        amountText.text = string.Empty;
        amountText.enabled = false;
    }
}