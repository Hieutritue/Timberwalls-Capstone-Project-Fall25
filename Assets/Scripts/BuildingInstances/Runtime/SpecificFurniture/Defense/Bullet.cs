using System;
using System.Collections;
using DefaultNamespace.Enemy;
using UnityEngine;

namespace BuildingSystem
{
    public class Bullet : MonoBehaviour
    {
        public float Damage { get; set; }

        private Coroutine _delayRelease;

        private void OnEnable()
        {
            if (_delayRelease != null)
                StopCoroutine(_delayRelease);
        }

        // public float Speed { get; set; }
        //
        // void Update()
        // {
        //     transform.position += transform.forward * (Speed * Time.deltaTime);
        // }
        //
        // private void OnTriggerEnter(Collider other)
        // {
        //     if (!other.CompareTag("Enemy")) return;
        //     var enemy = other.GetComponent<EnemyInstance>();
        //     if (enemy != null)
        //     {
        //         enemy.TakeDamage(Damage);
        //         gameObject.SetActive(false);
        //     }
        //     ObjectPoolManager.Instance.Release(gameObject);
        // }

        private void OnParticleCollision(GameObject other)
        {
            if (!other.CompareTag("Enemy")) return;
            var enemy = other.GetComponent<EnemyInstance>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);

                _delayRelease = StartCoroutine(DelayRelease());
            }
        }

        IEnumerator DelayRelease()
        {
            yield return new WaitForSeconds(5);
            ObjectPoolManager.Instance.Release(gameObject);
        }
    }
}