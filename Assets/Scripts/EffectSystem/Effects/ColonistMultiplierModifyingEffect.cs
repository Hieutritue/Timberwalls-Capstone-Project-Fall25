using System;
using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.EffectSystem.SO;
using Sirenix.Utilities;

namespace DefaultNamespace.EffectSystem.Effects
{
    [Serializable]
    public class ColonistMultiplierModifyingEffect : IEffect
    {
        public Colonist Colonist;
        public ColonistMultiplierModifyingSO EffectSo; 

        public ColonistMultiplierModifyingEffect(List<StatWithMultiplier> statIncreaseMultipliers, List<StatWithMultiplier> statDecreaseMultipliers)
        {
            EffectSo.StatIncreaseMultipliers = statIncreaseMultipliers;
            EffectSo.StatDecreaseMultipliers = statDecreaseMultipliers;
        }
        public void OnAttach()
        {
            EffectSo.StatIncreaseMultipliers.ForEach(x =>
            {
                if (Colonist.StatDict.TryGetValue(x.StatType, out var statRuntime))
                {
                    statRuntime.IncreaseMultiplier *= x.Multiplier;
                }
            });
            EffectSo.StatDecreaseMultipliers.ForEach(x =>
            {
                if (Colonist.StatDict.TryGetValue(x.StatType, out var statRuntime))
                {
                    statRuntime.DecreaseMultiplier *= x.Multiplier;
                }
            });
        }

        public void OnDetach()
        {
            EffectSo.StatIncreaseMultipliers.ForEach(x =>
            {
                if (Colonist.StatDict.TryGetValue(x.StatType, out var statRuntime))
                {
                    statRuntime.IncreaseMultiplier /= x.Multiplier;
                }
            });
            EffectSo.StatDecreaseMultipliers.ForEach(x =>
            {
                if (Colonist.StatDict.TryGetValue(x.StatType, out var statRuntime))
                {
                    statRuntime.DecreaseMultiplier /= x.Multiplier;
                }
            });
        }
    }
}