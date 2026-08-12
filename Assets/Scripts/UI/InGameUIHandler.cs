using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(1)]
public class InGameUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject inGameMenuUI;
    [SerializeField] private GameObject lvlComMenuUI;
    [SerializeField] private GameObject lvlFailMenuUI;

    private void OnEnable()
    {
        UIInstance.Instance.onPauseScene += () => SwapPanel(pauseMenuUI, inGameMenuUI);
        UIInstance.Instance.onResumeScene += () => SwapPanel(inGameMenuUI, pauseMenuUI);
        UIInstance.Instance.onReloadScene += () => LoadScene();
        UIInstance.Instance.onLoadScene += (sceneIndex) => LoadScene(sceneIndex);
        UIInstance.Instance.onLevelComplete += () => SwapPanel(lvlComMenuUI, inGameMenuUI);
        UIInstance.Instance.onLevelFailed += () => SwapPanel(lvlFailMenuUI, inGameMenuUI);
    }

    private void OnDisable()
    {
        UIInstance.Instance.onPauseScene -= () => SwapPanel(pauseMenuUI, inGameMenuUI);
        UIInstance.Instance.onResumeScene -= () => SwapPanel(inGameMenuUI, pauseMenuUI);
        UIInstance.Instance.onReloadScene -= () => LoadScene();
        UIInstance.Instance.onLoadScene -= (sceneIndex) => LoadScene(sceneIndex);
        UIInstance.Instance.onLevelComplete -= () => SwapPanel(lvlComMenuUI, inGameMenuUI);
        UIInstance.Instance.onLevelFailed -= () => SwapPanel(lvlFailMenuUI, inGameMenuUI);
    }

    private void Start()
    {
        // Ensure the in-game menu is active at the start
        inGameMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        lvlComMenuUI.SetActive(false);
        lvlFailMenuUI.SetActive(false);

        // Ensure that the game is not paused at the start
        Time.timeScale = 1f;
    }

    // Swap Panel
    // swaps two panels
    private void SwapPanel(GameObject panelToActivate, GameObject panelToDeactivate)
    {
        if (panelToActivate != null)
        {
            panelToActivate.SetActive(true);
        }
        if (panelToDeactivate != null)
        {
            panelToDeactivate.SetActive(false);
        }
    }

    // Load Scene
    // if no parameter is passed, it will reload the current scene
    // if a parameter is passed, it will load the scene with that index
    private void LoadScene(int senceIndex = -1)
    {
        if (senceIndex < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene(senceIndex);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        UIInstance.Instance.onPauseScene?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        UIInstance.Instance.onResumeScene?.Invoke();
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        UIInstance.Instance.onReloadScene?.Invoke();
    }

    public void LoadIntoAnotherScene(int sceneIndex)
    {
        Time.timeScale = 1f;
        UIInstance.Instance.onLoadScene?.Invoke(sceneIndex);
    }
}
