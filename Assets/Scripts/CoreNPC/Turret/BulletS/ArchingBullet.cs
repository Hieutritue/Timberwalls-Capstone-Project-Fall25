using UnityEngine;

/// <summary>
/// Bullet subclass that calculates a realistic parabolic (arching) trajectory
/// from muzzle to target using ballistic motion equations.
/// </summary>
public class ArchingBullet : Bullet
{
    [Header("Ballistic Settings")]
    [SerializeField] private bool useHighArc = false;  // false = low angle, true = high arc
    [SerializeField] private float gravity = 9.81f;

    /// <summary>
    /// Launches the bullet toward the target with a calculated arc based on gravity and speed.
    /// </summary>
    public void LaunchAtTarget(Vector3 startPos, Vector3 targetPos, float launchSpeed)
    {
        useGravity = true;
        rb.useGravity = true;
        rb.isKinematic = false;

        Vector3 velocity;
        if (ComputeLaunchVelocity(startPos, targetPos, launchSpeed, out velocity))
        {
            rb.linearVelocity = velocity;
        }
        else
        {
            // Fallback to straight line if arc is not possible
            Vector3 dir = (targetPos - startPos).normalized;
            rb.linearVelocity = dir * launchSpeed;
        }
    }

    /// <summary>
    /// Computes ballistic launch velocity needed to hit the target position.
    /// Returns false if no valid solution exists.
    /// </summary>
    private bool ComputeLaunchVelocity(Vector3 start, Vector3 target, float speed, out Vector3 velocity)
    {
        Vector3 toTarget = target - start;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);

        float dxz = toTargetXZ.magnitude;
        float dy = toTarget.y;
        float g = gravity * gravityScale;

        float v2 = speed * speed;
        float underRoot = v2 * v2 - g * (g * dxz * dxz + 2 * dy * v2);

        if (underRoot < 0f)
        {
            velocity = Vector3.zero;
            return false; // No valid ballistic arc
        }

        float root = Mathf.Sqrt(underRoot);

        float tanTheta = useHighArc
            ? (v2 + root) / (g * dxz)
            : (v2 - root) / (g * dxz);

        float angle = Mathf.Atan(tanTheta);

        Vector3 dirXZ = toTargetXZ.normalized;
        velocity = dirXZ * Mathf.Cos(angle) * speed;
        velocity.y = Mathf.Sin(angle) * speed;

        return true;
    }

    protected override void OnDeactivate()
    {
        base.OnDeactivate();
        rb.useGravity = false;
    }
}

