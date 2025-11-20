using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/DOT")]
public class DOTDebuff : Debuff
{
    [Header("DOT Settings")]
    public int damagePerTick = 2;
    public float tickInterval = 0.2f;

    private float nextTickTime;
    private bool firstTickApplied = false;

    public override void OnApply()
    {
        // First tick should apply immediately
        firstTickApplied = false;

        // Schedule the NEXT tick for after interval
        nextTickTime = Time.time + tickInterval;
    }

    public override bool Tick(float deltaTime, EnemyBase enemy = null)
    {
        if (enemy != null)
        {
            // FIRST TICK: apply immediately
            if (!firstTickApplied)
            {
                enemy.Damage(damagePerTick, name);
                firstTickApplied = true;
            }
            // NORMAL TICKS
            else if (Time.time >= nextTickTime)
            {
                enemy.Damage(damagePerTick, name);
                nextTickTime = Time.time + tickInterval;
            }
        }

        return base.Tick(deltaTime);
    }
}
