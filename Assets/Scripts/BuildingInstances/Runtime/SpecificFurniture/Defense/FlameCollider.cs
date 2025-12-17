using System;
using System.Collections.Generic;
using DefaultNamespace.Enemy;
using UnityEngine;

namespace BuildingSystem
{
    public class FlameCollider : MonoBehaviour
    {
        [field:SerializeField]
        public List<EnemyInstance> EnemyInstances { get; private set; } = new List<EnemyInstance>();
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            if (other.TryGetComponent(out EnemyInstance enemyInstance))
            {
                enemyInstance.OnDeath += OnEnemyDeath;
                EnemyInstances.Add(enemyInstance);
                Debug.LogWarning($"add");
            }
        }

        private void OnEnemyDeath(EnemyInstance enemyInstance)
        {
            Debug.LogWarning("remove");
            EnemyInstances.Remove(enemyInstance);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            if (other.TryGetComponent(out EnemyInstance enemyInstance))
            {
                enemyInstance.OnDeath -= OnEnemyDeath;
                Debug.LogWarning("remove");
                EnemyInstances.Remove(enemyInstance);
            }
        }
    }
}