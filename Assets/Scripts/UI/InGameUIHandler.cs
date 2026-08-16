using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class InGameUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject inGameMenuUI;
    [SerializeField] private GameObject lvlComMenuUI;
    [SerializeField] private GameObject lvlFailMenuUI;

    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;

    private void OnEnable()
    {
        if (UIInstance.Instance == null) return;

        UIInstance.Instance.onPauseScene += HandlePauseUI;
        UIInstance.Instance.onResumeScene += HandleResumeUI;
        UIInstance.Instance.onReloadScene += ReloadCurrentScene;
        UIInstance.Instance.onLoadScene += LoadSceneByIndex;
        UIInstance.Instance.onLevelComplete += HandleLevelCompleteUI;
        UIInstance.Instance.onLevelFailed += HandleLevelFailedUI;
    }

    private void OnDisable()
    {
        if (UIInstance.Instance == null) return;

        UIInstance.Instance.onPauseScene -= HandlePauseUI;
        UIInstance.Instance.onResumeScene -= HandleResumeUI;
        UIInstance.Instance.onReloadScene -= ReloadCurrentScene;
        UIInstance.Instance.onLoadScene -= LoadSceneByIndex;
        UIInstance.Instance.onLevelComplete -= HandleLevelCompleteUI;
        UIInstance.Instance.onLevelFailed -= HandleLevelFailedUI;
    }

    private void Start()
    {
        SwapPanel(inGameMenuUI, null);
        if (pauseMenuUI) pauseMenuUI.SetActive(false);
        if (lvlComMenuUI) lvlComMenuUI.SetActive(false);
        if (lvlFailMenuUI) lvlFailMenuUI.SetActive(false);

        Time.timeScale = 1f;

        CheckLevelButtons();
    }

    // Handlers
    private void HandlePauseUI() => SwapPanel(pauseMenuUI, inGameMenuUI);
    private void HandleResumeUI() => SwapPanel(inGameMenuUI, pauseMenuUI);
    
    private void HandleLevelCompleteUI()
    {
        SwapPanel(lvlComMenuUI, inGameMenuUI);

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int savedUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentScene >= savedUnlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentScene + 1);
            PlayerPrefs.Save();
        }
    }

    private void HandleLevelFailedUI() => SwapPanel(lvlFailMenuUI, inGameMenuUI);

    private void SwapPanel(GameObject panelToActivate, GameObject panelToDeactivate)
    {
        if (panelToDeactivate != null) panelToDeactivate.SetActive(false);
        if (panelToActivate != null) panelToActivate.SetActive(true);
    }


    private void CheckLevelButtons()
    {
        //int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        //if (level2Button != null) 
        //    level2Button.interactable = (unlockedLevel >= 2);

        //if (level3Button != null) 
        //    level3Button.interactable = (unlockedLevel >= 3);
    }

    public void StartGameSimulation()
    {
        GameManager.Instance.StartSimulation();
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

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1f;
        SceneTransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneTransitionManager.Instance.LoadScene(nextSceneIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneTransitionManager.Instance.LoadScene(0);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        Time.timeScale = 1f;
        SceneTransitionManager.Instance.LoadScene(sceneIndex);
    }
}