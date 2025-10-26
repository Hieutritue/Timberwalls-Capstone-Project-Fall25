using UnityEngine;
using UnityEngine.Pool;

public class Turret : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private TurretSO stats;
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform barrel;

    [Header("Firing")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private IObjectPool<Bullet> bulletPool;
    [SerializeField] private float fireRate = 2f;

    private Enemy currentTarget;
    private PlayerSpoof p;
    private float lastFireTime;
    private int defaultCapacity = 20;
    private int maxCapacity = 100;

    // Cached modifiers
    private float rotationSpeedModifier = 1f;
    private float fireRateModifier = 1f;

    private void Awake()
    {
        detectionRadius = stats.attackRange;
        rotationSpeed = stats.traverseSpeed;
        fireRate = stats.cyclics;
        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, true, defaultCapacity, maxCapacity);

        p = new PlayerSpoof(Random.Range(1, 11), Random.Range(0, 2));
        UpdateModifiers();
    }

    void Update()
    {
        // Toggle PlayerSpoof with Space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (p == null)
            {
                p = new PlayerSpoof(Random.Range(1, 11), Random.Range(0, 2));
                Debug.Log("PlayerSpoof created");
            }
            else
            {
                p = null;
                Debug.Log("PlayerSpoof set to null");
            }
            UpdateModifiers();
        }
        FindTarget();

        if (p == null) return;
        else
        {
            

            if (currentTarget != null)
            {
                RotateTowardsTarget();
                TryFire();
            }
        }
    }

    void UpdateModifiers()
    {
        // Reset modifiers
        rotationSpeedModifier = 1f;
        fireRateModifier = 1f;

        if (p == null) return;

        // Marksmanship: Each level adds +5% to rotation and fire rate
        float marksmanshipBonus = p.M_level * 0.05f;
        rotationSpeedModifier += marksmanshipBonus;
        fireRateModifier += marksmanshipBonus;

        // Afflictions: Each affliction reduces stats by 25%
        float afflictionPenalty = p.Affliction_number * 0.25f;
        rotationSpeedModifier -= afflictionPenalty;
        fireRateModifier -= afflictionPenalty;

        // Ensure modifiers don't go below 10% (0.1)
        rotationSpeedModifier = Mathf.Max(0.1f, rotationSpeedModifier);
        fireRateModifier = Mathf.Max(0.1f, fireRateModifier);

        Debug.Log($"Modifiers updated - Marksmanship Lvl: {p.M_level} (+{marksmanshipBonus * 100}%), " +
                  $"Afflictions: {p.Affliction_number} (-{afflictionPenalty * 100}%), " +
                  $"Final Rotation: {rotationSpeedModifier * 100}%, Final Fire Rate: {fireRateModifier * 100}%");
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        if (hits.Length > 0)
        {
            // Find closest enemy
            currentTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    float distance = Mathf.Abs(transform.position.x - hit.transform.position.x);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        currentTarget = enemy;
                    }
                }
            }
        }
        else
        {
            currentTarget = null;
        }
    }

    void RotateTowardsTarget()
    {
        if (currentTarget == null) return;

        Vector3 directionToTarget = (currentTarget.transform.position - barrel.position).normalized;
        directionToTarget.z = 0;
        directionToTarget = directionToTarget.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Apply rotation speed with modifier
        float effectiveRotationSpeed = rotationSpeed * rotationSpeedModifier;
        barrel.rotation = Quaternion.Slerp(barrel.rotation, targetRotation, effectiveRotationSpeed * Time.deltaTime);
    }

    void TryFire()
    {
        // Apply fire rate modifier
        float effectiveFireRate = fireRate * fireRateModifier;

        if (Time.time >= lastFireTime + (1f / effectiveFireRate))
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    void Fire()
    {
        if (currentTarget == null) return;

        Bullet bullet = bulletPool.Get();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.pool = bulletPool;

            // Direction from firePoint directly to target (X-Z plane only)
            Vector3 firingDirection = (currentTarget.transform.position - firePoint.position);
            firingDirection.z = 0;
            firingDirection = firingDirection.normalized;

            // Rotate bullet to match firing direction
            bullet.transform.rotation = Quaternion.LookRotation(firingDirection);
            bullet.deactivate();

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.forward * bullet.stats.bulletSpeed;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Detection radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Target line
        if (currentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentTarget.transform.position);
        }
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.pool = bulletPool;
        return bullet;
    }

    private void OnGetFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(Bullet bullet)
    {
        Destroy(bullet.gameObject);
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