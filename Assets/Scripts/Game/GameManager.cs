using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Ball Physics Settings")]
    [SerializeField] private float stopSpeedThreshold = 0.05f;
    [SerializeField] private float maxY = -10f;
    [SerializeField] private float startDelay = 1.5f;

    private GameObject ball;
    private Rigidbody ballRigidbody;

    public Action onBallStopped;
    public Action onBallReached;

    private bool isGameOver = false;
    private bool isSimulationStarted = false;
    private float timer = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        onBallStopped += GameFail;
        onBallReached += GameWin;
    }

    private void OnDisable()
    {
        onBallStopped -= GameFail;
        onBallReached -= GameWin;
    }

    public void SetBall(GameObject ballObject)
    {
        ball = ballObject;
        ballRigidbody = ball.GetComponent<Rigidbody>();
        isGameOver = false;
        isSimulationStarted = false;
        timer = 0f;
    }

    public void StartSimulation()
    {
        isSimulationStarted = true;
        timer = 0f;
    }

    private void FixedUpdate()
    {
        if (!ball || !ballRigidbody || !isSimulationStarted || isGameOver) return;

        timer += Time.fixedDeltaTime;

        if (timer < startDelay) return;

        if (ballRigidbody.linearVelocity.magnitude < stopSpeedThreshold || ball.transform.position.y < maxY)
        {
            isGameOver = true;
            onBallStopped?.Invoke();
        }
    }

    private void GameFail()
    {
        Time.timeScale = 0f;
        UIInstance.Instance?.onLevelFailed?.Invoke();
    }

    private void GameWin()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;
        UIInstance.Instance?.onLevelComplete?.Invoke();
    }
}