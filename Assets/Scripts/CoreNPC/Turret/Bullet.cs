using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float timeoutDelay = 5.0f;
    public BulletSO stats;
    
    private Rigidbody rb;

    private IObjectPool<Bullet> Pool;
    public IObjectPool<Bullet> pool {set => Pool = value; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void deactivate()
    {
        StartCoroutine(deactivateRoutine(timeoutDelay));
    }

    IEnumerator deactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Pool.Release(this);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(stats.damage, stats.name);
        }
        Pool.Release(this);
    }
}


