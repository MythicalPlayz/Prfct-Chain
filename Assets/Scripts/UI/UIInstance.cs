using System;
using UnityEngine;

public class UIInstance : MonoBehaviour
{
    // Singleton instance of the UIInstance class
    public static UIInstance Instance { get; private set; }

    // Actions for ui management
    public Action onPauseScene;
    public Action onResumeScene;
    public Action onReloadScene;
    public Action<int> onLoadScene;

    // Actions for Level completion/failure
    public Action onLevelComplete;
    public Action onLevelFailed;

    // Awake method to ensure only one instance of UIInstance exists
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
