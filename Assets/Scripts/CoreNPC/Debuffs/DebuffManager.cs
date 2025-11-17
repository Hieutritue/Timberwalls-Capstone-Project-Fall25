using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour, IDebuffable
{
    private List<Debuff> activeDebuffs = new List<Debuff>();

    public void ApplyDebuff(Debuff debuff)
    {
        debuff.Initialize(this);
        activeDebuffs.Add(debuff);
    }

    private void Update()
    {
        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            if (activeDebuffs[i].Update(Time.deltaTime))
            {
                activeDebuffs.RemoveAt(i);
            }
        }
    }
}

