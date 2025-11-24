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
            _impactCollider.EnemyInstances.ForEach(e => e.TakeDamage(TurretSo.BaseDamage));
        }

        protected override void StopShooting()
        {
            base.StopShooting();
            _flameFX.Stop();
        }
    }
}