using System.Collections.Generic;
using DefaultNamespace.Enemy.SO;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        [SerializeField] private EnemySo[] _enemySos;
        public List<EnemyInstance> EnemyInstances = new List<EnemyInstance>();
        
        public void AddEnemyInstance(EnemyInstance enemyInstance)
        {
            EnemyInstances.Add(enemyInstance);
        }
        
        public void RemoveEnemyInstance(EnemyInstance enemyInstance)
        {
            EnemyInstances.Remove(enemyInstance);
        }
    }
}