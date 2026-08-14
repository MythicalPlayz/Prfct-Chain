using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Transition UI")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(TransitionRoutine(sceneIndex));
    }

    public void Exit()
    {
        Application.Quit();
    }

private IEnumerator TransitionRoutine(int sceneIndex)
{
    if (fadeCanvasGroup != null) fadeCanvasGroup.blocksRaycasts = true;

    float timer = 0f;
    while (timer < fadeDuration)
    {
        timer += Time.unscaledDeltaTime;
        fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
        yield return null;
    }
    fadeCanvasGroup.alpha = 1f;

    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
    while (!asyncLoad.isDone)
    {
        yield return null;
    }

    timer = 0f;
    while (timer < fadeDuration)
    {
        timer += Time.unscaledDeltaTime;
        fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        yield return null;
    }
    fadeCanvasGroup.alpha = 0f;

    if (fadeCanvasGroup != null) fadeCanvasGroup.blocksRaycasts = false;
}
}