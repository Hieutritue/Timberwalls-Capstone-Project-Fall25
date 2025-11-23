using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Slow")]
public class SlowDebuff : Debuff
{
    [Header("Slow Settings")]
    public float movementMultiplier = 0.5f;  // 50% speed
    public float attack_cooldown_mult = 1.5f;

    public override float ModifyMovement(float baseValue)
    {
        return baseValue * movementMultiplier;
    }

    public override float ModifyAttackCooldown(float baseValue)
    {
        return baseValue * attack_cooldown_mult;
    }


    public override void OnApply()
    {
        // optional VFX / debug
        // Debug.Log("Slow applied to " + target);
    }
}

