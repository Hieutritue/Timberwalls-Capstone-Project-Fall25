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

    [SerializeField] private List<EnemyPoolConfig> enemyConfigs;
    private Dictionary<GameObject, IObjectPool<GameObject>> enemyPools = new();

    private void Awake()
    {
        foreach (var config in enemyConfigs)
        {
            if (config.enemyPrefab == null)
                continue;

            var pool = new ObjectPool<GameObject>(
                () => CreateEnemy(config.enemyPrefab),
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPooledObject,
                true,
                config.defaultCapacity,
                config.maxCapacity
            );

            enemyPools.Add(config.enemyPrefab, pool);
        }
    }

    public GameObject SpawnEnemy(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        GameObject obj = enemyPools[prefab].Get();
        obj.transform.SetPositionAndRotation(pos, rot);
        return obj;
    }

    private GameObject CreateEnemy(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        var pooled = obj.GetComponent<PooledEnemy>();
        if (pooled == null) pooled = obj.AddComponent<PooledEnemy>();
        pooled.Init(this, prefab);
        return obj;
    }

    private void OnGetFromPool(GameObject enemy)
    {
        enemy.SetActive(true);
        // EnemyInstance now auto-resets inside OnEnable()
    }

    private void OnReleaseToPool(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    private void OnDestroyPooledObject(GameObject enemy)
    {
        Destroy(enemy);
    }

    public void ReturnEnemy(GameObject prefab, GameObject enemy)
    {
        enemyPools[prefab].Release(enemy);
    }
}

public class PooledEnemy : MonoBehaviour
{
    private EnemiesGeneralPool pool;
    private GameObject prefab;

    public void Init(EnemiesGeneralPool p, GameObject f)
    {
        pool = p;
        prefab = f;
    }

    public void ReturnToPool()
    {
        pool.ReturnEnemy(prefab, gameObject);
    }
}
