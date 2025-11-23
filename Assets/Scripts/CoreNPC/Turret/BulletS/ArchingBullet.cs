using UnityEngine;

public class ArchingBullet : Bullet
{
    [Header("Fake Arc Settings")]
    [SerializeField] private float arcHeight = 3f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float travelTime;
    private float timer;

    public void LaunchAtTarget(Vector3 start, Vector3 target, float speed)
    {
        // Store original positions
        startPos = start;

        // Lock target to same Z-plane (2.5D world)
        target.z = start.z;
        targetPos = target;

        float distance = Vector3.Distance(startPos, targetPos);

        travelTime = distance / speed;
        timer = 0f;

        rb.useGravity = false;
        rb.isKinematic = true;

        OnActivate();
    }

    private void Update()
    {
        if (travelTime <= 0f) 
            return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / travelTime);

        // Horizontal motion (XZ)
        Vector3 horizontal = Vector3.Lerp(startPos, targetPos, t);

        // Parabolic vertical arc
        float height = arcHeight * (1 - Mathf.Pow(2 * t - 1, 2));

        Vector3 pos = horizontal;
        pos.y += height;

        transform.position = pos;

        // Make the projectile face its motion direction
        if (t > 0f)
        {
            Vector3 dir = pos - transform.position;
            if (dir.sqrMagnitude > 0.0001f)
                transform.forward = dir.normalized;
        }

        // End of arc
        if (t >= 1f)
            Impact();
    }

    private void Impact()
    {
        // Damage if very close to collider
        Collider[] results = Physics.OverlapSphere(transform.position, 0.6f);
        foreach (var r in results)
        {
            if (r.TryGetComponent<IDamageable>(out var hit))
            {
                hit.Damage(stats.damage, stats.name);
                OnHit(hit);
                break;
            }
        }

        Deactivate();
    }

    protected override void OnDeactivate()
    {
        base.OnDeactivate();
        rb.useGravity = false;
        rb.isKinematic = false;
    }
}
