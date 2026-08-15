using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public string groupID;

    public int[] allowedScenes;

    void Awake()
    {
        BackgroundMusic[] existingPlayers = FindObjectsByType<BackgroundMusic>(FindObjectsSortMode.None);

        foreach (var player in existingPlayers)
        {
            if (player != this && player.groupID == groupID)
            {
                Destroy(gameObject);
                return;
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isAllowed = false;
        foreach (int index in allowedScenes)
        {
            if (scene.buildIndex == index)
            {
                isAllowed = true;
                break;
            }
        }

        if (!isAllowed)
        {
            Destroy(gameObject);
        }
    }
}