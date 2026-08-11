using UnityEngine;

[RequireComponent(typeof(FanForce))]
public class Fan : MonoBehaviour
{
    [SerializeField] private FanData fanData;

    private FanForce _fanForce;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        _fanForce = GetComponent<FanForce>();

        SetActive(false);
    }

    public void SetActive(bool active)
    {
        IsActive = active;

        _fanForce.enabled = active;
    }

    public void Enable()
    {
        SetActive(true);
    }

    public void Disable()
    {
        SetActive(false);
    }
}