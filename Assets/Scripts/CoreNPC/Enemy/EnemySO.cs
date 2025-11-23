using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public int health;
    public int attackDamage;
    public float attackCooldown;
    public float attackRange;
    public float movementSpeed;
}
