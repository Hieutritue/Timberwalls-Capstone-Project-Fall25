using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class DebuffManager
{
    private readonly List<Debuff> activeDebuffs = new List<Debuff>();
    private EnemyBase enemy;

    public DebuffManager(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Apply(Debuff debuff, IDebuffable owner)
    {
        if (debuff == null) return;

        // Check if a debuff of same TYPE already exists
        for (int i = 0; i < activeDebuffs.Count; i++)
        {
            if (activeDebuffs[i].GetType() == debuff.GetType())
            {
                // REFRESH duration
                activeDebuffs[i].Refresh();
                return;
            }
        }

        // Otherwise create a new runtime clone
        Debuff clone = debuff.Clone();
        clone.Initialize(owner);

        activeDebuffs.Add(clone);
    }

    public void Update(float deltaTime)
    {
        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            if (activeDebuffs[i].Tick(deltaTime, enemy))
            {
                activeDebuffs[i].OnExpire();
                activeDebuffs.RemoveAt(i);
            }
        }
    }

    // ──────────────────────────────
    // Stat Modifiers (movement / damage / etc.)
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

    public float ModifyHealthValue(float baseMultiplier)
    {
        float result = baseMultiplier;
        foreach (var debuff in activeDebuffs)
            result = debuff.ModifyHealthValue(result);
        return result;
    }
}


