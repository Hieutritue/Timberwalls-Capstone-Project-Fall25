using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyThemeSO", menuName = "Scriptable Objects/DifficultyThemeSO")]
public class DifficultyThemeSO : ScriptableObject
{
    public List<EnemyTableSO> enemies_list;

    [Tooltip("How often enemies spawn (in seconds)")]
    [Range(0.1f, 10f)]
    public float interval;
    public int enemy_spawn_upper_limit;

    public int day_to_unlock = 1;
    
}
