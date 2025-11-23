using UnityEngine;

[CreateAssetMenu(fileName = "TurretSO", menuName = "Scriptable Objects/TurretSO")]
public class TurretSO : ScriptableObject
{
    public int tier;
    public float attackRange;
    public float cyclics;
    public float traverseSpeed;
}
