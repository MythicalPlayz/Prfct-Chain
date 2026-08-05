using UnityEngine;

public abstract class TriggerableObject : MonoBehaviour, ITriggerable
{
    [SerializeField] protected bool isTriggered = false;

    public bool IsTriggered => isTriggered;

    public virtual void Trigger()
    {
        if (isTriggered)
            return;

        isTriggered = true;
    }

    public virtual void ResetTrigger()
    {
        isTriggered = false;
    }
}