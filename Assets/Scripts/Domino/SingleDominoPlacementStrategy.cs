using UnityEngine;

public class SingleDominoPlacementStrategy : IDominoPlacementStrategy
{
    private readonly DominoFactory _dominoFactory;
    private readonly DominoPlacementValidator _validator;
    private readonly DominoData _dominoData;

    private Vector3 _currentPosition;

    private static readonly Quaternion DominoRotation =
        Quaternion.Euler(0f, 90f, 0f);

    public SingleDominoPlacementStrategy(
        DominoFactory dominoFactory,
        DominoPlacementValidator validator,
        DominoData dominoData)
    {
        _dominoFactory = dominoFactory;
        _validator = validator;
        _dominoData = dominoData;
    }

    public void BeginPlacement(Vector3 startPosition)
    {
        _currentPosition = startPosition;
    }

    public void UpdatePlacement(Vector3 currentPosition)
    {
        _currentPosition = currentPosition;
    }

    public void CompletePlacement(Vector3 endPosition)
    {
        _currentPosition = endPosition;

        if (!_validator.IsValid(
                _currentPosition,
                _dominoData.PlacementHalfExtents,
                DominoRotation))
        {
            return;
        }

        _dominoFactory.Create(
            _currentPosition,
            DominoRotation
        );
    }

    public void CancelPlacement()
    {
        _currentPosition = Vector3.zero;
    }
}