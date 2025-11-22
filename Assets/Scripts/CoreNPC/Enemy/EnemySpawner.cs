using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DefaultNamespace.ScheduleSystem;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [Tooltip("All possible waves (DifficultyThemeSO).")]
    [SerializeField] private List<DifficultyThemeSO> allWaves;

    [Tooltip("How many in-game hours count as night (from hour 0).")]
    [SerializeField] private int nightLengthHours = 6;

    [Tooltip("Max number of waves that can appear in a single night.")]
    [SerializeField] private int maxWavesPerNight = 6;

    [Header("Scaling Settings")]
    [Tooltip("From which in-game day difficulty starts scaling.")]
    [SerializeField] private int scalingStartDay = 6;

    [Tooltip("Extra enemies per day after scalingStartDay (0.15 = +15% per day).")]
    [SerializeField] private float enemyScalePerDay = 0.15f;

    [Header("Spawn System")]
    [SerializeField] private EnemiesGeneralPool enemyPool;
    [SerializeField] private GameTimeManager timeManager;

    [Header("Rush Wave Settings")]
    [Tooltip("When remaining enemies in current wave <= this % of the original limit we enter rush mode.")]
    [SerializeField] private float rushPercent = 0.15f;

    [Tooltip("Spawn interval (in hours) while in rush mode.")]
    [SerializeField] private float rushSpawnIntervalHours = 0.1f;

    [SerializeField] private UnityEvent OnRushWaveStarted;

    // --- Runtime state ---
    private Queue<DifficultyThemeSO> tonightQueue;
    private DifficultyThemeSO currentWave;

    private int upper_spawn_limit;   // remaining enemies in current wave
    private int initialWaveLimit;    // original limit for current wave

    private float spawnTimer = 0f;   // in hours
    private float spawnIntervalHours;
    private int last_spawner_day = -1;

    private bool isNight = false;
    private bool spawningWave = false;
    private bool rushMode = false;

    private readonly List<Transform> spawnPoints = new();
    private int leftCount = 0;
    private int rightCount = 0;

    private void Start()
    {
        // Collect spawn points (both sides)
        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnerLeft").Select(t => t.transform));
        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnerRight").Select(t => t.transform));

        if (timeManager == null)
        {
            timeManager = GameTimeManager.Instance;
        }
    }

    private void Update()
    {
        if (timeManager == null) return;

        int hour = timeManager.CurrentHour;
        int day = timeManager.CurrentDay;

        // Night is from hour 0 .. nightLengthHours-1
        bool nowNight = hour < nightLengthHours;

        // 1) Night just ended
        if (isNight && !nowNight)
        {
            EndNight(day);
            return; // don't keep spawning
        }


        // 2) Night just started (skip day 1 if you want, remove 'day != 1' if not)
        if (!isNight && nowNight && day != 1)
        {
            BeginNight(day);
        }

        // 3) If it is not night or we are not in a wave, do nothing
        if (!isNight || !spawningWave) return;

        // 4) Progress spawn timer in HOURS
        float hourDelta = Time.deltaTime / timeManager.RealSecondsPerInGameHour;
        spawnTimer += hourDelta;

        float neededInterval = rushMode ? rushSpawnIntervalHours : spawnIntervalHours;

        if (spawnTimer >= neededInterval)
        {
            spawnTimer -= neededInterval;
            SpawnWaveStep();
        }
    }

    // -------------------- Night lifecycle --------------------

    private void BeginNight(int day)
    {
        if (last_spawner_day == day) return;
        Debug.Log("Night begins – selecting waves");

        isNight = true;
        
        spawningWave = false;
        rushMode = false;

        // Get all unlocked waves for this day
        List<DifficultyThemeSO> unlocked =
            allWaves.Where(w => day >= w.day_to_unlock).ToList();

        // Shuffle unlocked waves
        for (int i = 0; i < unlocked.Count; i++)
        {
            int r = Random.Range(i, unlocked.Count);
            (unlocked[i], unlocked[r]) = (unlocked[r], unlocked[i]);
        }

        // Pick up to maxWavesPerNight
        tonightQueue = new Queue<DifficultyThemeSO>(
            unlocked.Take(maxWavesPerNight)
        );

        StartNextWave(day);
    }

    private void EndNight(int day)
    {
        Debug.Log("Night ended – stopping all waves");

        isNight = false;
        spawningWave = false;
        rushMode = false;
last_spawner_day = day;
        tonightQueue?.Clear();
        currentWave = null;
    }

    // -------------------- Wave lifecycle --------------------

    private void StartNextWave(int day)
    {
        if (!isNight) return;

        if (tonightQueue == null || tonightQueue.Count == 0)
        {
            Debug.Log("Night finished (spawned all waves)");
            isNight = false;
            spawningWave = false;
            currentWave = null;
            return;
        }

        currentWave = tonightQueue.Dequeue();

        // Base limit from SO
        upper_spawn_limit = currentWave.enemy_spawn_upper_limit;

        // Difficulty scaling by day
        if (day >= scalingStartDay)
        {
            int daysOver = day - scalingStartDay;
            float multiplier = 1f + enemyScalePerDay * daysOver;
            upper_spawn_limit = Mathf.CeilToInt(upper_spawn_limit * multiplier);
        }

        initialWaveLimit = upper_spawn_limit;
        rushMode = false;

        // Convert DifficultyThemeSO.interval (= total wave duration in HOURS)
        // to "time between spawn batches"
        // so that if interval == 2 and limit == 100, we spawn 100 enemies evenly over 2 hours.
        spawnIntervalHours = currentWave.interval / Mathf.Max(1, upper_spawn_limit);

        spawnTimer = 0f;
        spawningWave = true;

        Debug.Log($"Wave '{currentWave.name}' starting. Limit={upper_spawn_limit}, interval={spawnIntervalHours}");
    }

    private void SpawnWaveStep()
    {
        if (!isNight) return;

        // Wave finished
        if (upper_spawn_limit <= 0)
        {
            spawningWave = false;
            StartNextWave(timeManager.CurrentDay);
            return;
        }

        // Check for rush mode trigger
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
        if (wave == null || wave.enemies_list == null || wave.enemies_list.Count == 0)
            return;

        int totalWeight = wave.enemies_list.Sum(e => e.weight);
        int roll = Random.Range(0, totalWeight);

        EnemyTableSO chosen = WeightedChoice(wave.enemies_list, roll);
        if (chosen == null) return;

        for (int i = 0; i < chosen.enemy_spawn_count; i++)
        {
            if (upper_spawn_limit <= 0) break;

            Transform sp = GetBalancedSpawnPoint();
            Vector3 pos = GetSpawnOffset(sp.position);

            GameObject obj = enemyPool.SpawnEnemy(chosen.enemy_prefab, pos, Quaternion.identity);

            bool fromLeft = sp.position.x < 0;
            if (obj.TryGetComponent(out DefaultNamespace.Enemy.EnemyInstance inst))
                inst.SetTarget(fromLeft);

            upper_spawn_limit--;
        }
    }

    // -------------------- Helpers --------------------

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
        var left = spawnPoints.Where(s => s.position.x < 0f).ToList();
        var right = spawnPoints.Where(s => s.position.x >= 0f).ToList();

        float lw = 50f, rw = 50f;
        if (leftCount > rightCount + 2) { lw = 25f; rw = 75f; }
        else if (rightCount > leftCount + 2) { lw = 75f; rw = 25f; }

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

        // fallback
        return left.Count > 0 ? left[0] : right[0];
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
