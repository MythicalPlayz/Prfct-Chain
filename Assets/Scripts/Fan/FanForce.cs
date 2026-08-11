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
            transform.position,
            fanData.Range,
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

            if (rigidbody == null)
            {
                continue;
            }

            if (rigidbody.isKinematic)
            {
                continue;
            }

            if (rigidbody.mass > fanData.MaximumAffectedMass)
            {
                continue;
            }

            Vector3 direction = GetWindDirection(
                rigidbody.position
            );

            rigidbody.AddForce(
                direction * fanData.Force,
                ForceMode.Force
            );
        }
    }

    private Vector3 GetWindDirection(Vector3 targetPosition)
    {
        Vector3 direction =
            targetPosition - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return transform.forward;
        }

        return direction.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        if (fanData == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(
            transform.position,
            fanData.Range
        );

        Gizmos.DrawRay(
            transform.position,
            transform.forward * fanData.Range
        );
    }
}