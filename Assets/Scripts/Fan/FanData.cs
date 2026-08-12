using UnityEngine;

[CreateAssetMenu(
    fileName = "FanData",
    menuName = "Prfct Chain/Fan/Fan Data"
)]
public class FanData : ScriptableObject
{
    [Header("Wind Settings")]
    [Min(0f)]
    [SerializeField] private float force = 10f;

    [Min(0.1f)]
    [SerializeField] private float range = 5f;

    [Min(0.01f)]
    [SerializeField] private float radius = 2f;

    [Header("Affected Objects")]
    [Min(0f)]
    [SerializeField] private float maximumAffectedMass = 5f;

    public float Force => force;
    public float Range => range;
    public float Radius => radius;
    public float MaximumAffectedMass => maximumAffectedMass;
}