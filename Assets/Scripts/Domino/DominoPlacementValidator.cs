using UnityEngine;

public class DominoPlacementValidator
{
    private readonly LayerMask _blockedLayers;

    public DominoPlacementValidator(LayerMask blockedLayers)
    {
        _blockedLayers = blockedLayers;
    }

    public bool IsValid(
        Vector3 position,
        Vector3 halfExtents,
        Quaternion rotation)
    {
        return !Physics.CheckBox(
            position,
            halfExtents,
            rotation,
            _blockedLayers,
            QueryTriggerInteraction.Ignore
        );
    }
}