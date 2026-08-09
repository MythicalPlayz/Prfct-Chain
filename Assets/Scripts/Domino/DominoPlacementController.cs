using UnityEngine;

public class DominoPlacementController : MonoBehaviour
{
    [Header("Domino Configuration")]
    [SerializeField] private DominoData dominoData;

    [Header("Validation")]
    [SerializeField] private LayerMask blockedLayers;

    private DominoFactory _dominoFactory;
    private DominoPlacementValidator _validator;

    private IDominoPlacementStrategy _singleStrategy;
    private IDominoPlacementStrategy _lineStrategy;

    private void Awake()
    {
        if (!ValidateConfiguration())
        {
            enabled = false;
            return;
        }

        _dominoFactory = new DominoFactory(dominoData);

        _validator = new DominoPlacementValidator(
            blockedLayers
        );

        _singleStrategy =
            new SingleDominoPlacementStrategy(
                _dominoFactory,
                _validator,
                dominoData
            );

        _lineStrategy =
            new LineDominoPlacementStrategy(
                _dominoFactory,
                dominoData,
                _validator
            );
    }

    public void PlaceSingle(Vector3 position)
    {
        _singleStrategy.BeginPlacement(position);
        _singleStrategy.CompletePlacement(position);
    }

    public void BeginLinePlacement(Vector3 startPosition)
    {
        _lineStrategy.BeginPlacement(startPosition);
    }

    public void UpdateLinePlacement(Vector3 currentPosition)
    {
        _lineStrategy.UpdatePlacement(currentPosition);
    }

    public void CompleteLinePlacement(Vector3 endPosition)
    {
        _lineStrategy.CompletePlacement(endPosition);
    }

    public void CancelLinePlacement()
    {
        _lineStrategy.CancelPlacement();
    }

    private bool ValidateConfiguration()
    {
        if (dominoData == null)
        {
            Debug.LogError(
                "DominoPlacementController: " +
                "DominoData is missing.",
                this
            );

            return false;
        }

        if (dominoData.DominoPrefab == null)
        {
            Debug.LogError(
                "DominoPlacementController: " +
                "Domino prefab is missing.",
                this
            );

            return false;
        }

        return true;
    }
}