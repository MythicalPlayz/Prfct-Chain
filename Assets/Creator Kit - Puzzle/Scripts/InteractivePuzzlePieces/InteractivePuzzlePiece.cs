using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractivePuzzlePiece<TComponent> : BaseInteractivePuzzlePiece
where TComponent : Component
{
    public TComponent physicsComponent;
}


public abstract class BaseInteractivePuzzlePiece : MonoBehaviour
{
    public InputAction interactAction;
    public Rigidbody rb;
    public AudioClip activateSound;
    public AudioClip deactivateSound;
    public AudioSource puzzleAudioSource;

    public bool m_IsControlable;

    private void OnEnable()
    {
        interactAction.Enable();
        interactAction.started += OnInteractStarted;
        interactAction.canceled += OnInteractCanceled;
    }

    private void OnDisable()
    {
        interactAction.started -= OnInteractStarted;
        interactAction.canceled -= OnInteractCanceled;
        interactAction.Disable();
    }

    protected void FixedUpdate()
    {
        if (interactAction.IsPressed() && m_IsControlable)
        {
            ApplyActiveState();
        }
        else
        {
            ApplyInactiveState();
        }
    }

    private void OnInteractStarted(InputAction.CallbackContext ctx)
    {
        if (activateSound != null)
        {
            puzzleAudioSource.pitch = Random.Range(0.8f, 1.2f);
            puzzleAudioSource.PlayOneShot(activateSound);
        }
    }

    private void OnInteractCanceled(InputAction.CallbackContext ctx)
    {
        if (deactivateSound != null)
        {
            puzzleAudioSource.pitch = Random.Range(0.8f, 1.2f);
            puzzleAudioSource.PlayOneShot(deactivateSound);
        }
    }
    
    protected abstract void ApplyActiveState ();

    protected abstract void ApplyInactiveState ();

    public void EnableControl ()
    {
        m_IsControlable = true;
    }
}