using System.Collections.Generic;
using UnityEngine;

public class DebuffManager
{
    private readonly List<Debuff> activeDebuffs = new List<Debuff>();

    /// Apply a new debuff instance to this enemy
    public void Apply(Debuff debuff, IDebuffable owner)
    {
        if (debuff == null) return;

        // Clone the debuff instance so each enemy gets its own timer
        Debuff clone = debuff.Clone();
        clone.Initialize(owner);

        activeDebuffs.Add(clone);
    }

    /// Tick all debuffs from EnemyBase.Update()
    public void Update(float deltaTime)
    {
        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            // Tick returns true if finished/expired
            if (activeDebuffs[i].Tick(deltaTime))
            {
                activeDebuffs[i].OnExpire();
                activeDebuffs.RemoveAt(i);
            }
        }
    }

    // ──────────────────────────────
    // OPTIONAL: Modify stats based on active debuffs
    // ──────────────────────────────
    public float ModifyMovement(float baseSpeed)
    {
        float result = baseSpeed;

        foreach (var debuff in activeDebuffs)
            result = debuff.ModifyMovement(result);

        return result;
    }

    public float ModifyAttackCooldown(float baseCooldown)
    {
        float result = baseCooldown;

        foreach (var debuff in activeDebuffs)
            result = debuff.ModifyAttackCooldown(result);

        return result;
    }

    public float ModifyDamageTaken(float baseMultiplier)
    {
        float result = baseMultiplier;

        foreach (var debuff in activeDebuffs)
            result = debuff.ModifyDamageTaken(result);

        return result;
    }
}

