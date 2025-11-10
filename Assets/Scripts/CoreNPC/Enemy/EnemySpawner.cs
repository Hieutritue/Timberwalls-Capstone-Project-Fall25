using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private float base_spawn_interval = 2.0f;
    [SerializeField] private DifficultyThemeSO difficulty;
    [SerializeField] private EnemiesGeneralPool enemyPool;
    private List<Transform> spawn_points;
    private int total_weight = 0;
    private float act_spawn_interval = 0f;
    private List<EnemyTableSO> enemies_list;

    private int left_spawn_count = 0;
    private int right_spawn_count = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemies_list = difficulty.enemies_list;

        spawn_points = GameObject.FindGameObjectsWithTag("Spawner")
                            .Select(go => go.transform)
                            .ToList();

        act_spawn_interval = base_spawn_interval / difficulty.interval_modifier;

        foreach (var enemy in enemies_list) total_weight += enemy.weight;

        InvokeRepeating("spawn_enemy", act_spawn_interval, act_spawn_interval);

    }

    private void spawn_enemy()
    {
        int rand_point = Random.Range(0, total_weight);
        EnemyTableSO enemy_to_spawn = weighted_choice(rand_point);

        if (enemy_to_spawn != null) for (int i = 1; i <= enemy_to_spawn.enemy_spawn_count; i++)
            {
                Transform spawn_point = get_balanced_spawn_point();
                Vector3 spawn_pos = get_spawn_pos_with_offset(spawn_point.position);

                enemyPool.SpawnEnemy(enemy_to_spawn.enemy_prefab, spawn_pos, Quaternion.identity);
            }

    }

    private EnemyTableSO weighted_choice(int rolled_value, int cul_weight = 0, int pos = 0)
    {
        if (enemies_list.Count <= pos) return null;

        if (rolled_value < cul_weight + enemies_list[pos].weight) return enemies_list[pos];
        else return weighted_choice(rolled_value, cul_weight + enemies_list[pos].weight, pos + 1);
    }

    private Transform get_balanced_spawn_point()
    {
        List<Transform> left_spawns = new List<Transform>();
        List<Transform> right_spawns = new List<Transform>();

        // Separate spawns by side
        for (int i = 0; i < spawn_points.Count; i++)
        {
            if (spawn_points[i].position.x < 0)
                left_spawns.Add(spawn_points[i]);
            else
                right_spawns.Add(spawn_points[i]);
        }

        // Calculate favor weights
        float left_weight = 50f;
        float right_weight = 50f;

        if (left_spawn_count > right_spawn_count + 2)
        {
            // Favor right side (75% vs 25%)
            left_weight = 25f;
            right_weight = 75f;
        }
        else if (right_spawn_count > left_spawn_count + 2)
        {
            // Favor left side (75% vs 25%)
            left_weight = 75f;
            right_weight = 25f;
        }

        // Weighted random selection
        float total_weight = left_weight + right_weight;
        float random_value = Random.Range(0f, total_weight);

        Transform chosen_spawn;

        if (random_value < left_weight && left_spawns.Count > 0)
        {
            // Choose left spawn
            left_spawn_count++;
            chosen_spawn = left_spawns[Random.Range(0, left_spawns.Count)];
        }
        else if (right_spawns.Count > 0)
        {
            // Choose right spawn
            right_spawn_count++;
            chosen_spawn = right_spawns[Random.Range(0, right_spawns.Count)];
        }
        else
        {
            // Fallback if one side has no spawns
            left_spawn_count++;
            chosen_spawn = left_spawns[Random.Range(0, left_spawns.Count)];
        }

        return chosen_spawn;
    }

    private Vector3 get_spawn_pos_with_offset(Vector3 base_position)
    {
        float offset_radius = 1.5f;
        float random_angle = Random.Range(0f, Mathf.PI * 2);

        // Random radius within circle (sqrt for even distribution)
        float random_radius = Mathf.Sqrt(Random.Range(0f, 1f)) * offset_radius;

        // Circular offset on XZ plane
        float offset_x = Mathf.Cos(random_angle) * random_radius;
        float offset_y = Mathf.Sin(random_angle) * random_radius;

        return new Vector3(
            base_position.x + offset_x,
            base_position.y + offset_y,
            base_position.z
        );
    }
}
