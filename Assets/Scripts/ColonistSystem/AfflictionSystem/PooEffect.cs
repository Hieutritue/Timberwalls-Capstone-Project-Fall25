using System.Collections.Generic;
using BuildingSystem.CleanObjects;
using UnityEngine;
using Util;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    [CreateAssetMenu(fileName = "Poo Affliction",
        menuName = "Colonist System/Affliction System/Affliction Effects/Poo Effect")]
    public class PooEffect : AAfflictionEffect
    {
        public Dictionary<StatType, float> StatDecrease;
        public Poop AssociatedPoop;

        public override void ApplyEffect(Colonist colonist)
        {
            base.ApplyEffect(colonist);
            // Decrease specified stats
            foreach (var stat in StatDecrease)
            {
                colonist.SetStat(stat.Key, colonist.StatDict[stat.Key] - stat.Value);
            }

            colonist.SetStat(StatType.Continence, 100);

            // Spawn poop in the world at colonist's position
            if (AssociatedPoop != null)
            {
                Instantiate(AssociatedPoop, colonist.transform.position.With(y: colonist.transform.position.y + 1),
                    Quaternion.identity);
            }
        }
    }
}