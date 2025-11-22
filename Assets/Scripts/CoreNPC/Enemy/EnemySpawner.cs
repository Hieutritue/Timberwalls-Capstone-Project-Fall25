using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DefaultNamespace.ScheduleSystem;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private List<DifficultyThemeSO> allWaves;
    [SerializeField] private int nightLengthHours = 6; // first 6 hours = night
    [SerializeField] private int maxWavesPerNight = 6;

    [Header("Scaling Settings")]
    [SerializeField] private int scalingStartDay = 6;
    [SerializeField] private float enemyScalePerDay = 0.15f; // +15% enemy per day after scalingStartDay

    [Header("Spawn System")]
    [SerializeField] private EnemiesGeneralPool enemyPool;
    [SerializeField] private GameTimeManager timeManager;

    private Queue<DifficultyThemeSO> tonightQueue;
    private DifficultyThemeSO currentWave;

    private int upper_spawn_limit;
    private int initialWaveLimit;

    private float spawnTimer = 0f;     // countdown timer inside update
    private float spawnIntervalHours;  // time between spawns in-game hours

    private bool isNight = false;
    private bool spawningWave = false;
    private bool rushMode = false;

    [Header("Rush Wave Settings")]
    [SerializeField] private float rushPercent = 0.15f;
    [SerializeField] private float rushSpawnIntervalHours = 0.1f;
    [SerializeField] private UnityEvent OnRushWaveStarted;

    private List<Transform> spawnPoints = new();
    private int leftCount = 0;
    private int rightCount = 0;

    private void Start()
    {
        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnerLeft").Select(t => t.transform));
        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnerRight").Select(t => t.transform));
    }

    private void Update()
    {
        int hour = timeManager.CurrentHour;
        float hourProg = timeManager.HourProgress;
        int day = timeManager.CurrentDay;

        // night = first X hours
        bool nowNight = hour < nightLengthHours;

        if (!isNight && nowNight && day != 1)
        {
            BeginNight(day);
        }

        if (isNight && !spawningWave) return;

        if (!isNight) return;

        // advance our spawn timer using in-game hours
        float hourDelta = (Time.deltaTime / timeManager.RealSecondsPerInGameHour);
        spawnTimer += hourDelta;

        float needed = rushMode ? rushSpawnIntervalHours : spawnIntervalHours;

        if (spawnTimer >= needed)
        {
            spawnTimer -= needed;
            SpawnWaveStep();
        }
    }

    private void BeginNight(int day)
    {
        Debug.Log("Night begins – selecting waves");

        isNight = true;
        spawningWave = false;
        rushMode = false;

        // 1) filter: unlocked
        List<DifficultyThemeSO> unlocked =
            allWaves.Where(w => day >= w.day_to_unlock).ToList();

        // 2) shuffle
        for (int i = 0; i < unlocked.Count; i++)
        {
            int r = Random.Range(i, unlocked.Count);
            (unlocked[i], unlocked[r]) = (unlocked[r], unlocked[i]);
        }

        // 3) choose up to max waves
        tonightQueue = new Queue<DifficultyThemeSO>(
            unlocked.Take(maxWavesPerNight)
        );

        StartNextWave(day);
    }

    private void StartNextWave(int day)
    {
        if (tonightQueue.Count == 0)
        {
            Debug.Log("Night finished");
            isNight = false;
            return;
        }

        currentWave = tonightQueue.Dequeue();

        // base amount
        upper_spawn_limit = currentWave.enemy_spawn_upper_limit;

        // apply infinite scaling
        if (day >= scalingStartDay)
        {
            int daysOver = day - scalingStartDay;
            float multiplier = 1f + enemyScalePerDay * daysOver;
            upper_spawn_limit = Mathf.CeilToInt(upper_spawn_limit * multiplier);
        }

        initialWaveLimit = upper_spawn_limit;
        rushMode = false;

        // compute spawn interval
        spawnIntervalHours = currentWave.interval / Mathf.Max(1, upper_spawn_limit);

        spawnTimer = 0f;
        spawningWave = true;

        Debug.Log($"Wave '{currentWave.name}' starting. Limit={upper_spawn_limit}, interval={spawnIntervalHours}");
    }

    private void SpawnWaveStep()
    {
        if (upper_spawn_limit <= 0)
        {
            spawningWave = false;
            StartNextWave(timeManager.CurrentDay);
            return;
        }

        // rush wave
        int remaining = upper_spawn_limit;
        int threshold = Mathf.CeilToInt(initialWaveLimit * rushPercent);

        if (!rushMode && remaining <= threshold)
        {
            rushMode = true;
            OnRushWaveStarted?.Invoke();
            Debug.Log("RUSH MODE!");
        }

        SpawnEnemiesForWave(currentWave);
    }

    private void SpawnEnemiesForWave(DifficultyThemeSO wave)
    {
        int totalWeight = wave.enemies_list.Sum(e => e.weight);
        int roll = Random.Range(0, totalWeight);

        EnemyTableSO chosen = WeightedChoice(wave.enemies_list, roll);
        if (chosen == null) return;

        for (int i = 0; i < chosen.enemy_spawn_count; i++)
        {
            Transform sp = GetBalancedSpawnPoint();
            Vector3 pos = GetSpawnOffset(sp.position);

            GameObject obj = enemyPool.SpawnEnemy(chosen.enemy_prefab, pos, Quaternion.identity);

            bool fromLeft = sp.position.x < 0;
            if (obj.TryGetComponent(out DefaultNamespace.Enemy.EnemyInstance inst))
                inst.SetTarget(fromLeft);

            upper_spawn_limit--;
            if (upper_spawn_limit <= 0) break;
        }
    }

    private EnemyTableSO WeightedChoice(List<EnemyTableSO> list, int roll)
    {
        int cum = 0;
        foreach (var e in list)
        {
            cum += e.weight;
            if (roll < cum) return e;
        }
        return null;
    }

    private Transform GetBalancedSpawnPoint()
    {
        var left = spawnPoints.Where(s => s.position.x < 0).ToList();
        var right = spawnPoints.Where(s => s.position.x >= 0).ToList();

        float lw = 50f, rw = 50f;
        if (leftCount > rightCount + 2) { lw = 25; rw = 75; }
        else if (rightCount > leftCount + 2) { lw = 75; rw = 25; }

        float r = Random.Range(0f, lw + rw);

        if (r < lw && left.Count > 0)
        {
            leftCount++;
            return left[Random.Range(0, left.Count)];
        }
        else if (right.Count > 0)
        {
            rightCount++;
            return right[Random.Range(0, right.Count)];
        }

        return left[0];
    }

    private Vector3 GetSpawnOffset(Vector3 basePos)
    {
        float radius = 1f;
        float angle = Random.Range(0f, Mathf.PI * 2);
        float r = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;

        return new Vector3(
            basePos.x + Mathf.Cos(angle) * r,
            basePos.y + Mathf.Sin(angle) * r,
            basePos.z
        );
    }
}
