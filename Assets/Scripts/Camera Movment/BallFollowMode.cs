using UnityEngine;

public class BallFollowMode : ICameraMode
{
    private readonly Transform cameraTransform;
    private readonly Transform target;
    private readonly Vector3 offset;
    private readonly float smoothTime;
    private Vector3 currentVelocity = Vector3.zero;

    public BallFollowMode(Transform cameraTransform, Transform target, Vector3 offset, float smoothTime)
    {
        this.cameraTransform = cameraTransform;
        this.target = target;
        this.offset = offset;
        this.smoothTime = smoothTime;
    }

    public void UpdateMode()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;
        cameraTransform.position = Vector3.SmoothDamp(
            cameraTransform.position, 
            targetPosition, 
            ref currentVelocity, 
            smoothTime
        );

        cameraTransform.LookAt(target);
    }
}