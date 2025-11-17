using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Slow")]
public class SlowDebuff : Debuff
{
    [Header("Slow Settings")]
    public float movementMultiplier = 0.5f;  // 50% speed

    public override float ModifyMovement(float baseValue)
    {
        return baseValue * movementMultiplier;
    }

    protected override void OnApply()
    {
        // optional VFX / debug
        // Debug.Log("Slow applied to " + target);
    }
}

