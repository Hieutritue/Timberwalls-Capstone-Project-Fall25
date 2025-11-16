using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Turret base class capable of firing both straight and arching projectiles.
/// </summary>
public class Turret : MonoBehaviour
{
    public enum LaunchMode { Straight, Arching }
    public enum RotationAxis
    {
        X, // rotate around X (2D “flip up/down” on YZ plane)
        Y, // rotate around Y (typical ground turret on XZ plane)
        Z  // rotate around Z (classic 2D XY rotation)
    }


    [Header("Aiming")]
    [SerializeField] protected RotationAxis rotationAxis = RotationAxis.Y;
    [SerializeField] protected Vector3 barrelRotationOffsetEuler = Vector3.zero;

    [Header("Turret Settings")]
    [SerializeField] protected TurretSO stats;
    [SerializeField] protected LaunchMode launchMode = LaunchMode.Straight;
    [SerializeField] protected Transform barrel;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Bullet bulletPrefab;

    protected IObjectPool<Bullet> bulletPool;
    protected Enemy currentTarget;
    protected float lastFireTime;
    protected float turningDir;

    [Header("Aiming Dynamics")]
    [SerializeField] private float aimAcceleration = 4f;     // how fast rotation speeds up
    [SerializeField] private float aimDeceleration = 4f;     // how fast rotation slows down
    [SerializeField] private float maxRotationSpeed = 360f;  // degrees per second

    private Vector3 smoothedAimDir;
    private bool aimInitialized = false;
    private float currentTurnSpeed = 0f;


    // ============================================================
    // INITIALIZATION
    // ============================================================
    protected virtual void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(
            CreateBullet, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            true, 20, 100);

        aimAcceleration = stats.traverseSpeed/2;
        aimDeceleration = 14;
        maxRotationSpeed = stats.traverseSpeed;
    }

    // ============================================================
    // UPDATE LOOP
    // ============================================================
    protected virtual void Update()
    {
        FindTarget();

        if (currentTarget != null)
        {
            RotateTowards(currentTarget.transform.position);
            TryFire();
        }
    }

    // ============================================================
    // TARGETING
    // ============================================================
    protected virtual void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, stats.attackRange, enemyLayer);
        if (hits.Length == 0)
        {
            currentTarget = null;
            return;
        }

        float closest = float.MaxValue;
        Enemy nearest = null;

        foreach (Collider c in hits)
        {
            float d = Vector3.Distance(transform.position, c.transform.position);
            if (d < closest && c.TryGetComponent(out Enemy e))
            {
                closest = d;
                nearest = e;
            }
        }

        currentTarget = nearest;
    }
    protected virtual void RotateTowards(Vector3 targetPos)
    {
        if (!barrel) return;

        // -------------------------------
        // 1. Raw direction to target
        // -------------------------------
        Vector3 newDir = (targetPos - barrel.position);

        // Project onto rotation plane
        switch (rotationAxis)
        {
            case RotationAxis.Y: newDir.y = 0; break;
            case RotationAxis.X: newDir.x = 0; break;
            case RotationAxis.Z: newDir.z = 0; break;
        }

        if (newDir.sqrMagnitude < 0.0001f) return;
        newDir.Normalize();

        // -------------------------------
        // 2. Initialize on first frame
        // -------------------------------
        if (!aimInitialized)
        {
            smoothedAimDir = newDir;
            aimInitialized = true;
        }

        // -------------------------------
        // 3. Blend aim direction (prevents snapping)
        // -------------------------------
        smoothedAimDir = Vector3.Lerp(
            smoothedAimDir,
            newDir,
            Time.deltaTime * stats.traverseSpeed * 0.75f   // soft blending
        );

        // -------------------------------
        // 4. Compute desired rotation
        // -------------------------------
        Vector3 up = rotationAxis switch
        {
            RotationAxis.Y => Vector3.up,
            RotationAxis.X => Vector3.right,
            RotationAxis.Z => Vector3.forward,
            _ => Vector3.up
        };

        Quaternion desiredRot = Quaternion.LookRotation(smoothedAimDir, up);
        Quaternion offsetRot = Quaternion.Euler(barrelRotationOffsetEuler);
        desiredRot *= offsetRot;

        // -------------------------------
        // 5. Determine current angle difference
        // -------------------------------
        float angleDiff = Quaternion.Angle(barrel.rotation, desiredRot);

        // -------------------------------
        // 6. Accelerate / decelerate turning speed
        // -------------------------------
        if (angleDiff > 0.5f)
        {
            // speeding up
            currentTurnSpeed += aimAcceleration * Time.deltaTime;
        }
        else
        {
            // slowing down
            currentTurnSpeed -= aimDeceleration * Time.deltaTime;
        }

        currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, 0f, maxRotationSpeed);

        // -------------------------------
        // 7. Rotate at our velocity
        // -------------------------------
        float t = (currentTurnSpeed / maxRotationSpeed);

        barrel.rotation = Quaternion.Slerp(
            barrel.rotation,
            desiredRot,
            t * stats.traverseSpeed * Time.deltaTime
        );
    }


    // ============================================================
    // FIRING
    // ============================================================
    protected virtual void TryFire()
    {
        if (Time.time < lastFireTime + (1f / stats.cyclics))
            return;

        Fire();
        lastFireTime = Time.time;
    }

    protected virtual void Fire()
    {
        if (currentTarget == null) return;

        Bullet bullet = bulletPool.Get();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.pool = bulletPool;
        bullet.Activate();

        Vector3 targetPos = currentTarget.transform.position;

        if (launchMode == LaunchMode.Arching && bullet is ArchingBullet arching)
        {
            arching.LaunchAtTarget(firePoint.position, targetPos, bullet.Stats.bulletSpeed);
        }
        else
        {
            Vector3 dir = (targetPos - firePoint.position).normalized;
            bullet.Launch(dir, bullet.Stats.bulletSpeed);
        }
    }

    // ============================================================
    // POOL CALLBACKS
    // ============================================================
    protected virtual Bullet CreateBullet() => Instantiate(bulletPrefab);
    protected virtual void OnGetFromPool(Bullet b) => b.gameObject.SetActive(true);
    protected virtual void OnReleaseToPool(Bullet b) => b.gameObject.SetActive(false);
    protected virtual void OnDestroyPooledObject(Bullet b) => Destroy(b.gameObject);

#if UNITY_EDITOR
protected virtual void OnDrawGizmos()
{
    if (!firePoint || !stats) return;
    if (launchMode != LaunchMode.Arching) return;

    // Just a temporary dummy target — replace with currentTarget if you want dynamic preview
    Vector3 targetPos = (Application.isPlaying && currentTarget != null)
        ? currentTarget.transform.position
        : firePoint.position + firePoint.forward * stats.attackRange;

    // Try calculate velocity
    if (BallisticUtility.CalculateLaunchVelocity(
        firePoint.position, targetPos, bulletPrefab.Stats.bulletSpeed, 9.81f, false, out Vector3 velocity))
    {
        // Draw predicted arc
        var points = BallisticUtility.GenerateTrajectoryPoints(firePoint.position, velocity, 9.81f);
        Gizmos.color = Color.cyan;
        for (int i = 0; i < points.Count - 1; i++)
            Gizmos.DrawLine(points[i], points[i + 1]);

        // Draw impact marker
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(points[^1], 0.25f);
    }
    else
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(firePoint.position, firePoint.forward * stats.attackRange);
    }
}
#endif
}


public class PlayerSpoof
{
    public int M_level { get; private set; }
    public int Affliction_number { get; private set; }

    public PlayerSpoof(int level, int affliction_number)
    {
        M_level = level;
        this.Affliction_number = affliction_number;
    }
}

public static class BallisticUtility
{
    /// <summary>
    /// Calculates the launch velocity vector to hit a target at a given speed and gravity.
    /// </summary>
    public static bool CalculateLaunchVelocity(Vector3 start, Vector3 target, float speed, float gravity, bool highArc, out Vector3 velocity)
    {
        Vector3 toTarget = target - start;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);

        float dxz = toTargetXZ.magnitude;
        float dy = toTarget.y;
        float g = gravity;

        float v2 = speed * speed;
        float underRoot = v2 * v2 - g * (g * dxz * dxz + 2 * dy * v2);

        if (underRoot < 0f)
        {
            velocity = Vector3.zero;
            return false;
        }

        float root = Mathf.Sqrt(underRoot);
        float tanTheta = highArc ? (v2 + root) / (g * dxz) : (v2 - root) / (g * dxz);
        float angle = Mathf.Atan(tanTheta);

        Vector3 dirXZ = toTargetXZ.normalized;
        velocity = dirXZ * Mathf.Cos(angle) * speed;
        velocity.y = Mathf.Sin(angle) * speed;
        return true;
    }

    /// <summary>
    /// Generates a list of world points approximating the projectile's arc for Gizmos or LineRenderer.
    /// </summary>
    public static List<Vector3> GenerateTrajectoryPoints(Vector3 start, Vector3 velocity, float gravity, int steps = 30, float timestep = 0.1f)
    {
        List<Vector3> points = new List<Vector3>(steps);
        Vector3 current = start;
        Vector3 vel = velocity;

        for (int i = 0; i < steps; i++)
        {
            points.Add(current);
            vel += Vector3.down * gravity * timestep;
            current += vel * timestep;
        }
        return points;
    }
}