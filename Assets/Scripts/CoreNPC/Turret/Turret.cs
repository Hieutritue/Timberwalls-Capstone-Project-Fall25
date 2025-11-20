using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Turret base class capable of firing both straight and arching projectiles.
/// </summary>
public class Turret : MonoBehaviour
{
    public enum LaunchMode { Straight, Arching, Flamethrower }
    public enum TargetType
    {
        Any,
        GroundOnly,
        AirOnly
    }

    public enum TargetPriority
    {
        ClosestToTurret,
        FurthestFromTurret,
        HighestElevation,
        ClosestToGround
    }


    [Header("Target Filtering")]
    [SerializeField] private TargetType targetType = TargetType.Any;

    [Header("Target Priority")]
    [SerializeField] private TargetPriority priority = TargetPriority.ClosestToTurret;

    [SerializeField] private float groundHeightThreshold = 0.5f;


    [Header("Arching Visual Tilt")]
    [SerializeField]
    AnimationCurve tiltCurve =
    new AnimationCurve(
        new Keyframe(0f, 0f),     // close = no tilt
        new Keyframe(10f, 20f),   // medium = 20 degrees
        new Keyframe(20f, 30f)    // far = max tilt
    );
    public enum RotationAxis
    {
        X, // rotate around X (2D �flip up/down� on YZ plane)
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
    [SerializeField] protected Transform[] firePoints;   // MULTIPLE     
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
    private Vector3 lastLaunchDirection = Vector3.forward;
    private int fireIndex = 0;


    // ============================================================
    // INITIALIZATION
    // ============================================================
    protected virtual void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(
            CreateBullet, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            true, 20, 100);

        aimAcceleration = stats.traverseSpeed / 2;
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

    protected Transform GetNextFirePoint()
    {
        if (firePoints == null || firePoints.Length == 0)
            return barrel;   // fallback

        Transform fp = firePoints[fireIndex];

        fireIndex++;
        if (fireIndex >= firePoints.Length)
            fireIndex = 0;

        return fp;
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

        // STEP 1 — Filter by ground/air/any
        List<Enemy> candidates = FilterByType(hits);

        if (candidates.Count == 0)
        {
            currentTarget = null;
            return;
        }

        // STEP 2 — Apply priority rule
        currentTarget = SelectByPriority(candidates);
    }


    protected virtual void RotateTowards(Vector3 targetPos)
    {
        if (!barrel) return;

        // ============================================================
        // ARCHING MODE: Smooth horizontal rotation + cosmetic tilt
        // ============================================================
        if (launchMode == LaunchMode.Arching)
        {
            // 1. Horizontal direction (yaw only)
            Vector3 toTarget = targetPos - barrel.position;

            switch (rotationAxis)
            {
                case RotationAxis.Y: toTarget.y = 0; break;   // horizontal for ground turret
                case RotationAxis.X: toTarget.x = 0; break;
                case RotationAxis.Z: toTarget.z = 0; break;
            }

            if (toTarget.sqrMagnitude < 0.001f) return;
            toTarget.Normalize();

            // 2. Smooth aim direction (prevent snapping)
            if (!aimInitialized)
            {
                smoothedAimDir = toTarget;
                aimInitialized = true;
            }

            smoothedAimDir = Vector3.Lerp(
                smoothedAimDir,
                toTarget,
                Time.deltaTime * stats.traverseSpeed * 0.75f
            );

            Quaternion yawOnly = Quaternion.LookRotation(smoothedAimDir, Vector3.up);

            // 3. Compute COSMETIC tilt from distance
            float dist = Vector3.Distance(barrel.position, targetPos);
            float tiltAmount = tiltCurve.Evaluate(dist);     // evaluate curve

            // 4. Combine tilt into rotation
            Quaternion tiltRot = Quaternion.Euler(-tiltAmount, 0f, 0f);

            Quaternion desired = yawOnly * tiltRot * Quaternion.Euler(barrelRotationOffsetEuler);

            // 5. Rotate smoothly with acceleration
            float angleDiff = Quaternion.Angle(barrel.rotation, desired);

            if (angleDiff > 0.5f)
                currentTurnSpeed += aimAcceleration * Time.deltaTime;
            else
                currentTurnSpeed -= aimDeceleration * Time.deltaTime;

            currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, 0f, maxRotationSpeed);

            float t = currentTurnSpeed / maxRotationSpeed;

            barrel.rotation = Quaternion.Slerp(
                barrel.rotation,
                desired,
                t * stats.traverseSpeed * Time.deltaTime
            );

            return;
        }


        // ============================================================
        // STRAIGHT MODE (original)
        // ============================================================

        Vector3 newDir = (targetPos - barrel.position);

        switch (rotationAxis)
        {
            case RotationAxis.Y: newDir.y = 0; break;
            case RotationAxis.X: newDir.x = 0; break;
            case RotationAxis.Z: newDir.z = 0; break;
        }

        if (newDir.sqrMagnitude < 0.0001f) return;
        newDir.Normalize();

        if (!aimInitialized)
        {
            smoothedAimDir = newDir;
            aimInitialized = true;
        }

        smoothedAimDir = Vector3.Lerp(
            smoothedAimDir,
            newDir,
            Time.deltaTime * stats.traverseSpeed * 0.75f
        );

        Vector3 up = rotationAxis switch
        {
            RotationAxis.Y => Vector3.up,
            RotationAxis.X => Vector3.right,
            RotationAxis.Z => Vector3.forward,
            _ => Vector3.up
        };

        Quaternion desiredRot = Quaternion.LookRotation(smoothedAimDir, up)
                                * Quaternion.Euler(barrelRotationOffsetEuler);

        float diff = Quaternion.Angle(barrel.rotation, desiredRot);

        if (diff > 0.5f)
            currentTurnSpeed += aimAcceleration * Time.deltaTime;
        else
            currentTurnSpeed -= aimDeceleration * Time.deltaTime;

        currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, 0, maxRotationSpeed);

        float tt = currentTurnSpeed / maxRotationSpeed;

        barrel.rotation = Quaternion.Slerp(
            barrel.rotation, desiredRot,
            tt * stats.traverseSpeed * Time.deltaTime
        );
    }





    // ============================================================
    // FIRING
    // ============================================================
    protected virtual void TryFire()
    {
        if (launchMode == LaunchMode.Flamethrower)
        {
            Fire();    // continuous, no cycle time
            return;
        }

        if (Time.time < lastFireTime + (1f / stats.cyclics))
            return;

        Fire();
        lastFireTime = Time.time;
    }

    private void RotateTowardsLaunchDirection()
    {
        if (!barrel) return;

        // Remove vertical for 2.5D rotation
        Vector3 dir = lastLaunchDirection;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion desiredRot = Quaternion.LookRotation(dir, Vector3.up);

        barrel.rotation = Quaternion.Slerp(
            barrel.rotation,
            desiredRot,
            stats.traverseSpeed * Time.deltaTime
        );
    }

    private void RotateHorizontallyTowardTarget(Vector3 targetPos)
    {
        if (!barrel) return;

        // raw direction
        Vector3 dir = targetPos - barrel.position;

        // flatten to XZ plane
        dir.y = 0f;

        // if too small, use existing forward (prevents snapping)
        if (dir.sqrMagnitude < 0.01f)
            dir = barrel.forward;

        dir.Normalize();

        // artificial stabilization so it never points straight up/down visually
        float stability = 0.35f;
        dir = Vector3.Lerp(dir, barrel.forward, stability).normalized;

        // compute rotation
        Quaternion desiredRot = Quaternion.LookRotation(dir, Vector3.up);

        // smooth rotation (no snapping)
        barrel.rotation = Quaternion.Slerp(
            barrel.rotation,
            desiredRot,
            stats.traverseSpeed * Time.deltaTime
        );
    }

    private Enemy GetClosest(List<Enemy> list)
    {
        float best = float.MaxValue;
        Enemy chosen = null;

        foreach (var e in list)
        {
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < best)
            {
                best = d;
                chosen = e;
            }
        }
        return chosen;
    }


    private Enemy GetFurthest(List<Enemy> list)
    {
        float best = float.MinValue;
        Enemy chosen = null;

        foreach (var e in list)
        {
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d > best)
            {
                best = d;
                chosen = e;
            }
        }
        return chosen;
    }


    private Enemy GetHighestY(List<Enemy> list)
    {
        float best = float.MinValue;
        Enemy chosen = null;

        foreach (var e in list)
        {
            float y = e.transform.position.y;
            if (y > best)
            {
                best = y;
                chosen = e;
            }
        }
        return chosen;
    }

    private Enemy GetLowestY(List<Enemy> list)
    {
        float best = float.MaxValue;
        Enemy chosen = null;

        foreach (var e in list)
        {
            float y = e.transform.position.y;
            if (y < best)
            {
                best = y;
                chosen = e;
            }
        }
        return chosen;
    }


    private List<Enemy> FilterByType(Collider[] hits)
{
    List<Enemy> ground = new List<Enemy>();
    List<Enemy> air = new List<Enemy>();

    foreach (var h in hits)
    {
        if (!h.TryGetComponent(out Enemy e))
            continue;

        float y = e.transform.position.y;

        if (y <= groundHeightThreshold)
            ground.Add(e);
        else
            air.Add(e);
    }

    switch (targetType)
    {
        case TargetType.GroundOnly:
            // Prefer ground → fallback to air
            return ground.Count > 0 ? ground : air;

        case TargetType.AirOnly:
            // Prefer air → fallback to ground
            return air.Count > 0 ? air : ground;

        case TargetType.Any:
        default:
            // Merge both (ground first)
            ground.AddRange(air);
            return ground;
    }
}


    private Enemy SelectByPriority(List<Enemy> list)
    {
        switch (priority)
        {
            case TargetPriority.ClosestToTurret:
                return GetClosest(list);

            case TargetPriority.FurthestFromTurret:
                return GetFurthest(list);

            case TargetPriority.HighestElevation:
                return GetHighestY(list);

            case TargetPriority.ClosestToGround:
                return GetLowestY(list);

            default:
                return list[0];
        }
    }


    protected virtual void Fire()
    {
        if (currentTarget == null) return;

        // ---- MULTI BARREL SUPPORT ----
        Transform fp = GetNextFirePoint();

        Vector3 targetPos = currentTarget.transform.position;

        // ============================================================
        // FLAMETHROWER (Hitbox bullet, no aiming logic)
        // ============================================================
        if (launchMode == LaunchMode.Flamethrower)
        {
            Bullet flame = bulletPool.Get();

            flame.transform.position = fp.position;
            flame.transform.rotation = fp.rotation;
            flame.pool = bulletPool;
            flame.Activate();

            return;  // IMPORTANT: stop here
        }

        // ============================================================
        // COMMON BULLET CREATION (Straight or Arching)
        // ============================================================
        Bullet bullet = bulletPool.Get();
        bullet.transform.position = fp.position;
        bullet.transform.rotation = fp.rotation;
        bullet.pool = bulletPool;
        bullet.Activate();

        // ============================================================
        // ARCHING MODE
        // ============================================================
        if (launchMode == LaunchMode.Arching && bullet is ArchingBullet arching)
        {
            // project target onto 2D plane
            Vector3 targetPos2D = currentTarget.transform.position;
            targetPos2D.z = fp.position.z;

            Vector3 rawDir = (targetPos2D - fp.position).normalized;
            Vector3 launchDir = (rawDir + Vector3.up * 0.5f).normalized;

            lastLaunchDirection = launchDir;

            arching.LaunchAtTarget(fp.position, targetPos2D, bullet.Stats.bulletSpeed);
            return;
        }

        // ============================================================
        // STRAIGHT MODE
        // ============================================================
        Vector3 dir = (targetPos - fp.position).normalized;
        bullet.Launch(dir, bullet.Stats.bulletSpeed);
    }




    // ============================================================
    // POOL CALLBACKS
    // ============================================================
    protected virtual Bullet CreateBullet() => Instantiate(bulletPrefab);
    protected virtual void OnGetFromPool(Bullet b) => b.gameObject.SetActive(true);
    protected virtual void OnReleaseToPool(Bullet b) => b.gameObject.SetActive(false);
    protected virtual void OnDestroyPooledObject(Bullet b)
    {
        if (b == null) return;
        Destroy(b.gameObject);
    }

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