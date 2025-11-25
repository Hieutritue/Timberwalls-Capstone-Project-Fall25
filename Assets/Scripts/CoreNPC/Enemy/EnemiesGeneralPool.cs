using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class EnemiesGeneralPool : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPoolConfig
    {
        public GameObject enemyPrefab;
        public int defaultCapacity = 10;
        public int maxCapacity = 50;
    }

    [Header("Enemy Pool Configuration")]
    [SerializeField] private List<EnemyPoolConfig> enemyConfigs = new List<EnemyPoolConfig>();

    private Dictionary<GameObject, IObjectPool<GameObject>> enemyPools = new Dictionary<GameObject, IObjectPool<GameObject>>();

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var config in enemyConfigs)
        {
            if (config.enemyPrefab == null)
            {
                Debug.LogWarning("Enemy prefab is null in config, skipping...");
                continue;
            }

            var pool = new ObjectPool<GameObject>(
                createFunc: () => CreateEnemy(config.enemyPrefab),
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnReleaseToPool,
                actionOnDestroy: OnDestroyPooledObject,
                collectionCheck: true,
                defaultCapacity: config.defaultCapacity,
                maxSize: config.maxCapacity
            );

            enemyPools.Add(config.enemyPrefab, pool);

            Debug.Log($"Pool created for {config.enemyPrefab.name} - Default: {config.defaultCapacity}, Max: {config.maxCapacity}");
        }
    }

    // Spawn enemy from pool
    public GameObject SpawnEnemy(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
    {
        if (!enemyPools.ContainsKey(enemyPrefab))
        {
            Debug.LogError($"No pool found for enemy prefab: {enemyPrefab.name}");
            return null;
        }

        GameObject enemy = enemyPools[enemyPrefab].Get();
        enemy.transform.position = position;
        enemy.transform.rotation = rotation;

        return enemy;
    }

    // Return enemy to pool
    public void ReturnEnemy(GameObject enemyPrefab, GameObject enemy)
    {
        if (!enemyPools.ContainsKey(enemyPrefab))
        {
            Debug.LogError($"No pool found for enemy prefab: {enemyPrefab.name}");
            Destroy(enemy);
            return;
        }

        enemyPools[enemyPrefab].Release(enemy);
    }

    // Pool callbacks
    private GameObject CreateEnemy(GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab);

        // Add PooledEnemy component to track which pool it belongs to
        PooledEnemy pooledEnemy = enemy.GetComponent<PooledEnemy>();
        if (pooledEnemy == null)
        {
            pooledEnemy = enemy.AddComponent<PooledEnemy>();
        }
        pooledEnemy.Initialize(this, prefab);

        return enemy;
    }

    private void OnGetFromPool(GameObject enemy)
{
    enemy.SetActive(true);

    // Support OLD Enemy system
    if (enemy.TryGetComponent(out Enemy oldEnemy))
        oldEnemy.ResetEnemy();

    // Support NEW EnemyInstance system
    if (enemy.TryGetComponent(out DefaultNamespace.Enemy.EnemyInstance newEnemy))
        newEnemy.ResetForPooling();
}

    private void OnReleaseToPool(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    private void OnDestroyPooledObject(GameObject enemy)
    {
        Destroy(enemy);
    }
}

// Helper component to track pooled enemies
public class PooledEnemy : MonoBehaviour
{
    private EnemiesGeneralPool pool;
    private GameObject prefabReference;

    public void Initialize(EnemiesGeneralPool enemyPool, GameObject prefab)
    {
        pool = enemyPool;
        prefabReference = prefab;
    }

    // Call this when enemy dies/should be returned to pool
    public void ReturnToPool()
    {
        if (pool != null && prefabReference != null)
        {
            pool.ReturnEnemy(prefabReference, gameObject);
        }
        else
        {
            Debug.LogWarning("Pool or prefab reference is null, destroying object instead");
            Destroy(gameObject);
        }
    }
}
