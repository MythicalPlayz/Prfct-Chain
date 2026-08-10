using UnityEngine;

public class CameraController : TriggerableObject
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

        freeCameraMode = new FreeCameraMode(transform, inputProvider, freeMoveSpeed);
        ballFollowMode = new BallFollowMode(transform, ballTarget, followOffset, smoothTime);

        currentMode = freeCameraMode;
    }

    private void Update()
    {
        if (inputProvider != null && inputProvider.SimulatePressed)
        {
            Trigger();
        }

        currentMode?.UpdateMode();
    }

    public override void Trigger()
    {
        base.Trigger();
        currentMode = ballFollowMode;
    }

    public override void ResetTrigger()
    {
        base.ResetTrigger();
        currentMode = freeCameraMode;
    }
}