using System.Collections.Generic;

namespace DefaultNamespace.EffectSystem
{
    public interface IAffectable
    {
        List<IEffect> ActiveEffects { get; set; }
    }
}