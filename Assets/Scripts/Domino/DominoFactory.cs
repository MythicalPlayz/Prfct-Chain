using UnityEngine;

public class DominoFactory
{
    private readonly DominoData _dominoData;

    public DominoFactory(DominoData dominoData)
    {
        _dominoData = dominoData;
    }

    public Domino Create(
        Vector3 position,
        Quaternion rotation,
        Transform parent = null)
    {
        if (_dominoData == null)
        {
            Debug.LogError(
                "DominoFactory: DominoData is missing."
            );

            return null;
        }

        if (_dominoData.DominoPrefab == null)
        {
            Debug.LogError(
                "DominoFactory: Domino prefab is missing."
            );

            return null;
        }

        Domino domino = Object.Instantiate(
            _dominoData.DominoPrefab,
            position,
            rotation,
            parent
        );

        domino.SaveInitialState();

        return domino;
    }
}