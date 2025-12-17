using System.Linq;
using DefaultNamespace.Enemy;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace BuildingSystem
{
    public class FlameThrowerFurniture : TurretFurniture
    {
        [SerializeField] private FlameCollider _impactCollider;
        [SerializeField] private ParticleSystem _flameFX;

        public override void Shoot()
        {
            if (!_flameFX.isPlaying)
                _flameFX.Play();
            _impactCollider.EnemyInstances.ToList().ForEach(e => e.TakeDamage(TurretSo.BaseDamage));
        }

        public override void StopShooting()
        {
            base.StopShooting();
            if(_flameFX != null) _flameFX?.Stop();
        }
    }
}