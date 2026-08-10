using UnityEngine;

// واجهة لكل نمط حركة
public interface ICameraMode
{
    void UpdateMode();
}

// واجهة لقراءة المدخلات
public interface IInputProvider
{
    Vector2 MoveInput { get; }
    bool SimulatePressed { get; }
}