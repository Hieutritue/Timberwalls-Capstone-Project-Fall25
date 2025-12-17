using DefaultNamespace.Enemy;
using DefaultNamespace.ScheduleSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

public class HieuEnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")] [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private Transform _realSpawnPoint;
    [SerializeField] EnemyInstance[] _enemyInstances;

    [Header("Night Settings")] public float baseSpawnInterval = 12f;
    public float spawnIntervalPerDay = 0.5f;
    public int maxEnemiesPerNight = 10;

    [Header("Spawn protection")] 
    public int spawnProtectionDuration = 3;

    private bool _spawnProtectionActive = true;
    private float _spawnTimer;
    private int _spawnedThisNight;

    void Update()
    {
        int day = GameTimeManager.Instance.CurrentDay;

        // Spawn protection
        if (_spawnProtectionActive)
        {
            if (day > spawnProtectionDuration)
            {
                _spawnProtectionActive = false;
                Debug.Log("Caution: Spawn protection expired");
            }
            else
            {
                return;
            }
        }

        bool isNight = GameTimeManager.Instance.IsNight;

        if (!isNight)
        {
            _spawnedThisNight = 0;
            return;
        }

        // Night active
        float spawnInterval = Mathf.Max(2f, baseSpawnInterval - day * spawnIntervalPerDay);
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= spawnInterval && _spawnedThisNight < maxEnemiesPerNight + (day * 3))
        {
            _spawnTimer = 0;
            SpawnEnemy(day);
        }
    }

    [SerializeField] private float _heightSpawnRandomOffset;

    void SpawnEnemy(int day)
    {
        // Calculate weights
        float[] weights = new float[_enemyInstances.Length];
        for (int i = 0; i < _enemyInstances.Length; i++)
            weights[i] = _enemyInstances[i].GetWeight(day);

        // Choose variant
        EnemyInstance variant = WeightedRandom.Choose(_enemyInstances, weights);

        // Choose spawn point
        var randomSpawnIndex = Random.Range(0, _spawnPoints.Length);
        _realSpawnPoint.position = _spawnPoints[randomSpawnIndex].position;

        // Instantiate
        var y = Random.Range(.8f,
            DefaultNamespace.ShieldSystem.ShieldSystem.Instance.ShieldWall.WallHeight);
        _realSpawnPoint.position = _realSpawnPoint.position.With(y: y);
        var enemyInstance = Instantiate(variant, _realSpawnPoint);
        enemyInstance.transform.SetParent(null);
        enemyInstance.SetTarget(randomSpawnIndex == 0);
        
        _spawnedThisNight++;
    }
}