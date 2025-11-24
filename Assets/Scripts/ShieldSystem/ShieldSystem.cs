using System;
using System.Collections.Generic;
using ShieldSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.ShieldSystem
{
    public class ShieldSystem : MonoSingleton<ShieldSystem>
    {
        public ShieldGenerator ShieldGenerator;
        public ShieldWall ShieldWall;

        private void Start()
        {
            ShieldWall.SetShieldHpLevel(0);
            ShieldGenerator.SetShieldMaintainabilityLevel(0);
        }
    }
}