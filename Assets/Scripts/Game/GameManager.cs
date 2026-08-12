using System;
using Unity.VisualScripting;
using UnityEngine;
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private float stopSpeedThreshold = 0.01f;
    [SerializeField] private float maxY = -10f;
    private GameObject ball;
    private Rigidbody ballRigidbody;

    public Action onBallStopped;
    public Action onBallReached;

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
    }

    private void FixedUpdate()
    {
        if (!ball) return;

        if (ballRigidbody.linearVelocity.magnitude < stopSpeedThreshold)
        {
            onBallStopped?.Invoke();
        }
        else if (ball.transform.position.y < maxY)
        {
            onBallStopped?.Invoke();
        }
    }

    private void GameFail()
    {
        Time.timeScale = 0f;
        UIInstance.Instance.onLevelFailed?.Invoke();
    }

    private void GameWin()
    {
        Time.timeScale = 0f;
        UIInstance.Instance.onLevelComplete?.Invoke();
    }
}
