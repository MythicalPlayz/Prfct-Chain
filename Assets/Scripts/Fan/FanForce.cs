using UnityEngine;

public class FanForce : MonoBehaviour
{
    [SerializeField] private FanData fanData;

    private readonly Collider[] _affectedColliders = new Collider[32];

    private void FixedUpdate()
    {
        ApplyWindForce();
    }

    private void ApplyWindForce()
    {
        if (fanData == null)
        {
            return;
        }

        int colliderCount = Physics.OverlapSphereNonAlloc(
            transform.position + transform.forward * (fanData.Range * 0.5f),
            fanData.Range * 0.5f,
            _affectedColliders
        );

        for (int i = 0; i < colliderCount; i++)
        {
            Collider currentCollider = _affectedColliders[i];

            if (currentCollider == null)
            {
                continue;
            }

            Rigidbody rigidbody = currentCollider.attachedRigidbody;

            if (rigidbody == null || rigidbody.isKinematic)
            {
                continue;
            }

            if (rigidbody.mass > fanData.MaximumAffectedMass)
            {
                continue;
            }

            if (!IsInFanDirection(rigidbody.position))
            {
                continue;
            }

            rigidbody.AddForce(
                transform.forward * fanData.Force,
                ForceMode.Force
            );
        }
    }

    private bool IsInFanDirection(Vector3 targetPosition)
    {
        Vector3 directionToTarget =
            targetPosition - transform.position;

        directionToTarget.y = 0f;

        if (directionToTarget.sqrMagnitude <= 0.001f)
        {
            return true;
        }

        float dotProduct = Vector3.Dot(
            transform.forward,
            directionToTarget.normalized
        );

        return dotProduct > 0f;
    }

    private void OnDrawGizmosSelected()
    {
        if (fanData == null)
        {
            return;
        }

        Vector3 center =
            transform.position +
            transform.forward * (fanData.Range * 0.5f);

        Gizmos.DrawWireSphere(
            center,
            fanData.Range * 0.5f
        );

        Gizmos.DrawRay(
            transform.position,
            transform.forward * fanData.Range
        );
    }
}