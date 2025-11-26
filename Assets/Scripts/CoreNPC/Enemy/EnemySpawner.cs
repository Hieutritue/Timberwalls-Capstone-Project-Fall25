using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DefaultNamespace.ScheduleSystem;
using DefaultNamespace.Enemy;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private List<DifficultyThemeSO> allWaves;
    [SerializeField] private int wavesPerNight = 3;
    [SerializeField] private float waveDurationHours = 2f;

    [Header("Day Scaling")]
    [SerializeField] private int scalingStartDay = 8;
    [SerializeField] private float expGrowth = 1.1f;

    [Header("References")]
    [SerializeField] private GameTimeManager timeManager;
    [SerializeField] private EnemiesGeneralPool enemyPool;

    // Runtime state
    private Queue<DifficultyThemeSO> tonightQueue;
    private DifficultyThemeSO currentWave;
    private float spawnTimerHours;
    private int nextWaveEntryIndex;   // index into enemies_list
    private int spawnCounterInsideEntry;   // progress inside enemy_spawn_count

    private bool isNight;
    private int lastNightDay = -1;

    private readonly List<Transform> spawnPoints = new();
    private int leftCount, rightCount;


    private void Start()
    {
        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnerLeft").Select(t => t.transform));
        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnerRight").Select(t => t.transform));

        if (timeManager == null)
            timeManager = GameTimeManager.Instance;
    }

    private void Update()
    {
        if (timeManager == null) return;

        int day = timeManager.CurrentDay;
        int hour = timeManager.CurrentHour;

        bool nowNight = (hour < 6); // Night = first 6 hours

        // Night END
        if (isNight && !nowNight)
        {
            EndNight(day);
            return;
        }

        // Night START !isNight && nowNight && day != lastNightDay && day != 1
        if (!isNight && nowNight && day != lastNightDay)
        {
            BeginNight(day);
        }

        if (!isNight || currentWave == null) return;

        // Move timer in HOURS
        spawnTimerHours += Time.deltaTime / timeManager.RealSecondsPerInGameHour;

        int totalEnemiesInWave = currentWave.enemies_list.Sum(e => Mathf.Max(1, e.enemy_spawn_count));
        if (totalEnemiesInWave == 0) return;

        float interval = waveDurationHours / totalEnemiesInWave;

        if (spawnTimerHours >= interval)
        {
            spawnTimerHours -= interval;
            SpawnNextEnemy(day);
        }
    }

    // ----------------- NIGHT SYSTEM -----------------

    private void BeginNight(int day)
    {
        Debug.Log("Night begins – picking waves");

        isNight = true;
        lastNightDay = day;

        // Select unlocked waves
        var unlocked = allWaves.Where(w => day >= w.day_to_unlock).ToList();

        // Shuffle
        var selected = new List<DifficultyThemeSO>();

        for (int i = 0; i < wavesPerNight && unlocked.Count > 0; i++)
        {
            int r = Random.Range(0, unlocked.Count);
            selected.Add(unlocked[r]);
            unlocked.RemoveAt(r);
        }

        tonightQueue = new Queue<DifficultyThemeSO>(selected);

        StartNextWave(day);
    }

    private void EndNight(int day)
    {
        Debug.Log("Night ended – cleanup");
        isNight = false;
        tonightQueue?.Clear();
        currentWave = null;
    }

    private void StartNextWave(int day)
    {
        if (tonightQueue == null || tonightQueue.Count == 0)
        {
            Debug.Log("Night finished – all waves done");
            currentWave = null;
            return;
        }

        currentWave = tonightQueue.Dequeue();
        nextWaveEntryIndex = 0;
        spawnCounterInsideEntry = 0;
        spawnTimerHours = 0f;

        Debug.Log($"Wave {currentWave.name} begins, order count={currentWave.enemies_list.Count}.");
    }

    // ----------------- ENEMY SPAWNING -----------------

    private void SpawnNextEnemy(int day)
    {
        if (currentWave == null) return;

        // If out of entries → next wave
        if (nextWaveEntryIndex >= currentWave.enemies_list.Count)
        {
            StartNextWave(day);
            return;
        }

        var entry = currentWave.enemies_list[nextWaveEntryIndex];
        int count = Mathf.Max(1, entry.enemy_spawn_count);

        // finished all spawn_count for this entry?
        if (spawnCounterInsideEntry >= count)
        {
            nextWaveEntryIndex++;
            spawnCounterInsideEntry = 0;
            SpawnNextEnemy(day);
            return;
        }

        // Spawn ONE enemy per interval
        spawnCounterInsideEntry++;

        Transform sp = GetBalancedSpawnPoint();
        Vector3 pos = GetOffset(sp.position);

        GameObject obj = enemyPool.SpawnEnemy(entry.enemy_prefab, pos, Quaternion.identity);

        if (obj.TryGetComponent(out EnemyInstance inst))
        {

            ApplyScaling(inst, day);

            bool fromLeft = sp.CompareTag("SpawnerLeft");
            inst.SetTarget(fromLeft);

            inst.PrintFinalStats(fromLeft ? "LEFT" : "RIGHT");
        }
    }

    // ----------------- SPAWN HELPERS -----------------

    private Transform GetBalancedSpawnPoint()
    {
        var left = spawnPoints.Where(s => s.CompareTag("SpawnerLeft")).ToList();
        var right = spawnPoints.Where(s => s.CompareTag("SpawnerRight")).ToList();

        float lw = 50f, rw = 50f;
        if (leftCount > rightCount + 2) { lw = 25f; rw = 75f; }
        else if (rightCount > leftCount + 2) { lw = 75f; rw = 25f; }

        float roll = Random.Range(0f, lw + rw);
        Transform chosen;

        if (roll < lw && left.Count > 0)
        {
            chosen = left[Random.Range(0, left.Count)];
            leftCount++;
        }
        else if (right.Count > 0)
        {
            chosen = right[Random.Range(0, right.Count)];
            rightCount++;
        }
        else
        {
            chosen = left.Count > 0 ? left[0] : right[0];
        }

        return chosen;
    }

    private Vector3 GetOffset(Vector3 p)
    {
        float radius = 1f;
        float angle = Random.value * Mathf.PI * 2;
        float r = Mathf.Sqrt(Random.value) * radius;

        return new Vector3(
            p.x + Mathf.Cos(angle) * r,
            p.y + Mathf.Sin(angle) * r,
            p.z
        );
    }

    // ----------------- SCALING -----------------

    private void ApplyScaling(EnemyInstance inst, int day)
    {
        var baseStats = inst.Stats;
        var so = inst.EnemySo;
        var tier = so.tierMultiplier;

        if (tier == null)
        {
            inst.ApplyFinalStats(
                baseStats.Health,
                baseStats.AttackDamage,
                baseStats.MoveSpeed,
                baseStats.AttackRange,
                baseStats.AttackCooldown
            );
            return;
        }

        float d = Mathf.Max(0, day - scalingStartDay);
        float dayMult = Mathf.Pow(expGrowth, d);

        float H = baseStats.Health * tier.HealthMult * dayMult;
        float D = baseStats.AttackDamage * tier.DamageMult * dayMult;

        float S = baseStats.MoveSpeed * tier.SpeedMult * dayMult;
        if (so.MaxMoveSpeed > 0f) S = Mathf.Min(S, so.MaxMoveSpeed);

        float R = baseStats.AttackRange * tier.RangeMult * dayMult;
        if (so.MaxAttackRange > 0f) R = Mathf.Min(R, so.MaxAttackRange);

        float cdMult = tier.CooldownMult * dayMult;
        if (cdMult <= 0f) cdMult = 0.0001f;
        float CD = baseStats.AttackCooldown / cdMult;

        inst.ApplyFinalStats(H, D, S, R, CD);
    }
}
