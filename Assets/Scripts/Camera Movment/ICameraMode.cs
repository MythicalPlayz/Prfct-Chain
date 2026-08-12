using UnityEngine;

public interface ICameraMode
{
    void UpdateMode();
}

public interface IInputProvider
{
    Vector2 MoveInput { get; }
    bool SimulatePressed { get; }
}