using UnityEngine;

public class TriggerSource : MonoBehaviour
{
    [SerializeField] private TriggerableObject target;

    public void Activate()
    {
        if (target != null)
        {
            target.Trigger();
        }
    }
}