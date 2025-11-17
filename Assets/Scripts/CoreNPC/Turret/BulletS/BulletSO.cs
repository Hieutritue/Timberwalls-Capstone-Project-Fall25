using UnityEngine;

[CreateAssetMenu(fileName = "BulletSO", menuName = "Scriptable Objects/BulletSO")]
public class BulletSO : ScriptableObject
{
    public int damage;
    public int bulletSpeed;
    public Debuff[] debuffs;
}
