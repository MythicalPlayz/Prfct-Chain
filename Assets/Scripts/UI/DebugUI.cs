using UnityEngine;

public class DebugUI : MonoBehaviour
{
    // Purpose is to test UI
    public void CompleteLevel()
    {
        UIInstance.Instance.onLevelComplete?.Invoke();
    }

    public void FailLevel()
    {
        UIInstance.Instance.onLevelFailed?.Invoke();
    }
}
