using System;
using System.Collections.Generic;
using DefaultNamespace.Enemy;
using UnityEngine;

namespace BuildingSystem
{
    public class FlameCollider : MonoBehaviour
    {
        public HashSet<EnemyInstance> EnemyInstances { get; private set; } = new HashSet<EnemyInstance>();
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            if (other.TryGetComponent(out EnemyInstance enemyInstance))
            {
                EnemyInstances.Add(enemyInstance);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            if (other.TryGetComponent(out EnemyInstance enemyInstance))
            {
                EnemyInstances.Remove(enemyInstance);
            }
        }
    }
}