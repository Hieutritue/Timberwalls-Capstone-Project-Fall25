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
    private float lastFireTime;
    private int defaultCapacity = 20;
    private int maxCapacity = 100;

    private void Awake()
    {
        detectionRadius = stats.attackRange;
        rotationSpeed = stats.traverseSpeed;
        fireRate = stats.cyclics;
        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, true, defaultCapacity, maxCapacity);
    }

    void Update()
    {
        FindTarget();

        if (currentTarget != null)
        {
            RotateTowardsTarget();
            TryFire();
        }
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
        barrel.rotation = Quaternion.Slerp(barrel.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void TryFire()
    {
        if (Time.time >= lastFireTime + (1f / fireRate))
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