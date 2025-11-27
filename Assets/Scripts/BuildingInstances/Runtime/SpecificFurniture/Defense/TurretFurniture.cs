using DefaultNamespace.ColonistSystem;
using DefaultNamespace.Enemy;
using DefaultNamespace.General;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace BuildingSystem
{
    public class TurretFurniture : Furniture, ITaskCreator
    {
        public Colonist ColonistAssignedToTurret { get; set; }

        [Header("Refs")] [SerializeField] private Transform _firePoint; // where bullets spawn
        [SerializeField] private Transform _partToRotate; // rotating head of the turret
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _actionPoint;

        [SerializeField] private Animator _animator;
        protected TurretFurnitureSo TurretSo => (TurretFurnitureSo)PlaceableSo;

        private float _fireCooldown;
        private Transform _currentTarget;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private int SkillLevel => ColonistAssignedToTurret?.ColonistSo.Skills[SkillType.Marksmanship] ?? 0;

        public void UpdateInWorkingState()
        {
            AcquireTarget();
            RotateZToFaceTarget();
            TryShoot();
        }


        // ------------------------------------------
        //  FIND TARGET
        // ------------------------------------------
        void AcquireTarget()
        {
            if (_currentTarget != null)
            {
                // if target moves out of range -> lose it
                if (Vector3.Distance(transform.position, _currentTarget.position) > TurretSo.AttackRange)
                    _currentTarget = null;

                return;
            }

            // Find nearest enemy
            float bestDist = Mathf.Infinity;
            Transform best = null;

            foreach (var enemy in EnemyManager.Instance.EnemyInstances) // keep a static list in Enemy.cs
            {
                float d = Vector3.Distance(transform.position, enemy.transform.position);
                if (d < bestDist && d <= TurretSo.AttackRange)
                {
                    bestDist = d;
                    best = enemy.transform;
                }
            }

            _currentTarget = best;
        }


        // ------------------------------------------
        //  ROTATE TOWARD TARGET
        // ------------------------------------------
        void RotateZToFaceTarget()
        {
            if (_currentTarget == null) return;

            // Calculate the rotation needed to look at the target
            Quaternion targetRotation = Quaternion.LookRotation(_currentTarget.position - _partToRotate.position);

            // Smoothly rotate towards the target rotation
            _partToRotate.rotation = Quaternion.RotateTowards(_partToRotate.rotation, targetRotation,
                FormulaCollection.GetTurretRotationSpeed(TurretSo.BaseTraverseSpeed, SkillLevel) * Time.deltaTime);
        }

        // ------------------------------------------
        //  FIRE ONLY WHEN POINTING AT TARGET
        // ------------------------------------------
        void TryShoot()
        {
            if (!_currentTarget || !_currentTarget.gameObject.activeInHierarchy)
            {
                StopShooting();
                return;
            }

            _fireCooldown -= Time.deltaTime;
            if (_fireCooldown > 0) return;

            bool isAimingAtTarget = false;

            // check if turret direction is toward target on Z axis
            Physics.Raycast(_firePoint.position, _firePoint.forward, out RaycastHit hitInfo, TurretSo.AttackRange,
                LayerMask.GetMask("Enemies"), QueryTriggerInteraction.Collide);
            if (hitInfo.collider != null && hitInfo.collider.transform == _currentTarget)
            {
                isAimingAtTarget = true;
            }

            if (!isAimingAtTarget) return;

            // Fire
            _fireCooldown = 1f / FormulaCollection.GetFireRate(TurretSo.BaseFireRate,
                SkillLevel);
            Shoot();
        }

        protected virtual void StopShooting()
        {
        }


        public virtual void Shoot()
        {
            _animator.SetTrigger(Attack);
            Bullet bullet = ObjectPoolManager.Instance.Get(_bulletPrefab.gameObject).GetComponent<Bullet>();
            bullet.transform.position = _firePoint.position;
            bullet.transform.rotation = _firePoint.rotation;
            bullet.Damage = TurretSo.BaseDamage;
        }

        public void CreateTask()
        {
            AddTask(new ManningTurretTask(this, _actionPoint, TaskType.ManningTurrets));
        }
    }
}