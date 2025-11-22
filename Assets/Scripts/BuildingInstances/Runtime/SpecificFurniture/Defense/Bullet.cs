using DefaultNamespace.Enemy;
using UnityEngine;

namespace BuildingSystem
{
    public class Bullet : MonoBehaviour
    {
        public float Damage { get; set; }
        public float Speed { get; set; }
        
        void Update()
        {
            transform.position += transform.forward * (Speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            var enemy = other.GetComponent<EnemyInstance>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
            ObjectPoolManager.Instance.Release(gameObject);
        }
    }
}