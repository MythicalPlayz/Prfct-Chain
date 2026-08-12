using UnityEngine;

public interface IDominoPlacementStrategy
{
    void BeginPlacement(Vector3 startPosition);

    void UpdatePlacement(Vector3 currentPosition);

    void CompletePlacement(Vector3 endPosition);

    void CancelPlacement();
}