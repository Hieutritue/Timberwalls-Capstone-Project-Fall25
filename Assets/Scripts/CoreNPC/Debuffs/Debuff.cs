using UnityEngine;

public abstract class Debuff
{
    public float Duration { get; private set; }
    public float TimeRemaining { get; private set; }

    protected IDebuffable target;

    protected Debuff(float duration)
    {
        Duration = duration;
        TimeRemaining = duration;
    }

    public void Initialize(IDebuffable targetEntity)
    {
        target = targetEntity;
        OnApply();
    }

    public bool Update(float deltaTime)
    {
        TimeRemaining -= deltaTime;
        OnUpdate(deltaTime);

        if (TimeRemaining <= 0f)
        {
            OnExpire();
            return true; // signal expired
        }

        return false;
    }

    protected virtual void OnApply() { }
    protected virtual void OnUpdate(float dt) { }
    protected virtual void OnExpire() { }

     public abstract Debuff Clone();
}
