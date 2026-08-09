using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Domino : MonoBehaviour
{
    public enum State
    {
        Standing,
        Falling,
        Fallen
    }

    private Rigidbody _rigidbody;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    private State _currentState = State.Standing;

    public State CurrentState => _currentState;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SaveInitialState()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;

        _currentState = State.Standing;
    }

    public void EnablePhysics()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.WakeUp();
    }

    public void ResetState()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _rigidbody.isKinematic = true;

        transform.SetPositionAndRotation(
            _initialPosition,
            _initialRotation
        );

        _currentState = State.Standing;

        _rigidbody.Sleep();
    }

    public void SetFalling()
    {
        if (_currentState == State.Fallen)
        {
            return;
        }

        _currentState = State.Falling;
    }

    public void SetFallen()
    {
        _currentState = State.Fallen;
    }
}