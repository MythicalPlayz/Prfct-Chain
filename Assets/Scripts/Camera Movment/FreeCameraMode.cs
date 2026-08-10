using UnityEngine;

public class FreeCameraMode : ICameraMode
{
    private readonly Transform cameraTransform;
    private readonly IInputProvider inputProvider;
    private readonly float moveSpeed;

    public FreeCameraMode(Transform cameraTransform, IInputProvider inputProvider, float moveSpeed)
    {
        this.cameraTransform = cameraTransform;
        this.inputProvider = inputProvider;
        this.moveSpeed = moveSpeed;
    }

    public void UpdateMode()
    {
        Vector2 input = inputProvider.MoveInput;
        Vector3 direction = new Vector3(input.x, input.y, 0f);
        cameraTransform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }
}