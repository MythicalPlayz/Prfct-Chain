using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f;
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadScene(1);
        }
    }

    public void QuitGame()
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.Exit();
        }
    }
}