using UnityEngine;

[CreateAssetMenu(
    fileName = "DominoData",
    menuName = "Prfct Chain/Domino/Domino Data"
)]
public class DominoData : ScriptableObject
{
    [Header("Prefab")]
    [SerializeField] private Domino dominoPrefab;

    [Header("Placement")]
    [Min(0.01f)]
    [SerializeField] private float spacing = 0.5f;

    [Min(1)]
    [SerializeField] private int maximumPlacementCount = 10;

    [Header("Collision")]
    [SerializeField]
    private Vector3 placementHalfExtents =
        new Vector3(0.15f, 0.5f, 0.05f);

    public Domino DominoPrefab => dominoPrefab;

    public float Spacing => spacing;

    public int MaximumPlacementCount => maximumPlacementCount;

    public Vector3 PlacementHalfExtents => placementHalfExtents;
}