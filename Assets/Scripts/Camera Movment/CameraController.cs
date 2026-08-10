using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float freeMoveSpeed = 10f;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 followOffset = new Vector3(0, 2, -5);

    [Header("Dependencies")]
    [SerializeField] private Transform ballTarget;
    [SerializeField] private MonoBehaviour inputReaderMono;

    private IInputProvider inputProvider;
    private ICameraMode currentMode;
    private FreeCameraMode freeCameraMode;
    private BallFollowMode ballFollowMode;

    private void Awake()
    {
        inputProvider = inputReaderMono as IInputProvider;

        // إنشاء الأنماط (Dependency Injection)
        freeCameraMode = new FreeCameraMode(transform, inputProvider, freeMoveSpeed);
        ballFollowMode = new BallFollowMode(transform, ballTarget, followOffset, smoothTime);

        currentMode = freeCameraMode;
    }

    private void Update()
    {
        if (inputProvider.SimulatePressed)
        {
            SwitchToSimulate();
        }

        currentMode?.UpdateMode();
    }

    public void SwitchToSimulate()
    {
        currentMode = ballFollowMode;
    }

    public void SwitchToFree()
    {
        currentMode = freeCameraMode;
    }
}