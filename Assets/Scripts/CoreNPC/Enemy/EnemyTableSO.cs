using UnityEngine;


[CreateAssetMenu(fileName = "EnemyTableSO", menuName = "Scriptable Objects/EnemyTableSO")]
public class EnemyTableSO : ScriptableObject
{
    public GameObject enemy_prefab;
    public int enemy_spawn_count = 1;
    public int weight;
}

