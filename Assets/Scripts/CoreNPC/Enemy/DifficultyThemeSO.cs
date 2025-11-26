using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DifficultyThemeSO", menuName = "Scriptable Objects/DifficultyThemeSO")]
public class DifficultyThemeSO : ScriptableObject
{
    [Tooltip("Exact list of enemies that spawn in this wave, in order.")]
    public List<EnemyTableSO> enemies_list;

    public int day_to_unlock = 1;
}
