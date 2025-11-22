using UnityEngine;

public abstract class Debuff : ScriptableObject
{
    [Header("Debuff Settings")]
    public float duration = 2f;

    [System.NonSerialized] protected IDebuffable target;
    [System.NonSerialized] protected float timeRemaining;

    public void Initialize(IDebuffable targetEntity)
    {
        target = targetEntity;
        timeRemaining = duration;
        OnApply();
    }

    public void Refresh()
    {
        timeRemaining = duration;
    }

    public virtual bool Tick(float deltaTime, EnemyBase enemy = null)
    {
        timeRemaining -= deltaTime;
        OnUpdate(deltaTime);

        if (timeRemaining <= 0f)
        {
            OnExpire();
            return true;
        }
        return false;
    }

    // Default stat modifiers (override in child debuffs)
    public virtual float ModifyMovement(float baseValue) => baseValue;
    public virtual float ModifyAttackCooldown(float baseValue) => baseValue;
    public virtual float ModifyDamageTaken(float baseValue) => baseValue;
    public virtual float ModifyHealthValue(float baseValue) => baseValue;
    public virtual void OnApply() { }
    protected virtual void OnUpdate(float dt) { }
    public virtual void OnExpire() { }

    // Default clone: duplicate this asset as a runtime instance
    public virtual Debuff Clone()
    {
        return Instantiate(this);
    }
}