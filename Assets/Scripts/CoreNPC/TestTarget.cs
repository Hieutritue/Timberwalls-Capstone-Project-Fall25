using UnityEngine;

public class TestTarget : MonoBehaviour, IDamageable
{
    public virtual void Damage(int amount, string EnemyName)
    {
        Debug.Log($"Damage amount is {amount} by {EnemyName}");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
