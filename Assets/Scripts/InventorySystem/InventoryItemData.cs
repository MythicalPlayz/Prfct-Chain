using UnityEngine;

[CreateAssetMenu(
    fileName = "InventoryItemData",
    menuName = "Prfct Chain/Inventory/Item Data"
)]
public class InventoryItemData : ScriptableObject
{
    [Header("Item Information")]
    [SerializeField] private string itemName;

    [SerializeField] private GameObject prefab;

    [SerializeField] private Sprite icon;

    [Header("Placement Settings")]
    [SerializeField] private float spawnYRotation = 0f;

    public string ItemName => itemName;
    public GameObject Prefab => prefab;
    public Sprite Icon => icon;

    public float SpawnYRotation => spawnYRotation;
}