using UnityEngine;

public class LineDominoPlacementStrategy : IDominoPlacementStrategy
{
    private readonly DominoFactory _dominoFactory;
    private readonly DominoData _dominoData;
    private readonly DominoPlacementValidator _validator;

    private Vector3 _startPosition;
    private Vector3 _currentPosition;

    public LineDominoPlacementStrategy(
        DominoFactory dominoFactory,
        DominoData dominoData,
        DominoPlacementValidator validator)
    {
        _dominoFactory = dominoFactory;
        _dominoData = dominoData;
        _validator = validator;
    }

    public void BeginPlacement(Vector3 startPosition)
    {
        _startPosition = startPosition;
        _currentPosition = startPosition;
    }

    public void UpdatePlacement(Vector3 currentPosition)
    {
        _currentPosition = currentPosition;
    }

    public void CompletePlacement(Vector3 endPosition)
    {
        _currentPosition = endPosition;

        Vector3 direction = _currentPosition - _startPosition;

        float distance = direction.magnitude;

        if (distance < _dominoData.Spacing)
        {
            return;
        }

        direction.Normalize();

        int dominoCount = Mathf.FloorToInt(
            distance / _dominoData.Spacing
        );

        dominoCount = Mathf.Min(
            dominoCount,
            _dominoData.MaximumPlacementCount
        );

        Quaternion rotation = GetDominoRotation(direction);

        for (int i = 0; i < dominoCount; i++)
        {
            Vector3 position =
                _startPosition +
                direction * (_dominoData.Spacing * i);

            if (!_validator.IsValid(
                    position,
                    _dominoData.PlacementHalfExtents,
                    rotation))
            {
                continue;
            }

            _dominoFactory.Create(
                position,
                rotation
            );
        }
    }

    public void CancelPlacement()
    {
        _startPosition = Vector3.zero;
        _currentPosition = Vector3.zero;
    }

    private Quaternion GetDominoRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(
            direction.z,
            direction.x
        ) * Mathf.Rad2Deg;

        return Quaternion.Euler(
            0f,
            -angle + 90f,
            0f
        );
    }
}