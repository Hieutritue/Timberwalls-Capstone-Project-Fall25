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
        private TurretFurnitureSo _turretSo => (TurretFurnitureSo)PlaceableSo;

        private float _fireCooldown;
        private Transform _currentTarget;
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
                if (Vector3.Distance(transform.position, _currentTarget.position) > _turretSo.AttackRange)
                    _currentTarget = null;

                return;
            }

            // Find nearest enemy
            float bestDist = Mathf.Infinity;
            Transform best = null;

            foreach (var enemy in EnemyManager.Instance.EnemyInstances) // keep a static list in Enemy.cs
            {
                float d = Vector3.Distance(transform.position, enemy.transform.position);
                if (d < bestDist && d <= _turretSo.AttackRange)
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

            Vector3 dir = _currentTarget.position - _partToRotate.position;

            // Convert world direction into *flat* 2D direction
            Vector2 dir2D = new Vector2(dir.x, dir.y); // or (dir.x, dir.z) depending on your game

            // If your game is top-down (X + Y plane), use:
            // Vector2 dir2D = new Vector2(dir.x, dir.y);

            if (dir2D.sqrMagnitude < 0.0001f)
                return;

            // Angle in degrees
            float targetAngle = Mathf.Atan2(dir2D.y, dir2D.x) * Mathf.Rad2Deg;

            // Rotate ONLY around Z axis (top-down)
            float currentAngle = _partToRotate.eulerAngles.z;

            float newAngle = Mathf.MoveTowardsAngle(
                currentAngle,
                targetAngle,
                FormulaCollection.GetTurretRotationSpeed(_turretSo.BaseTraverseSpeed, SkillLevel) * Time.deltaTime
            );

            _partToRotate.rotation = Quaternion.Euler(_partToRotate.rotation.x, 0, newAngle);
        }

        // ------------------------------------------
        //  FIRE ONLY WHEN POINTING AT TARGET
        // ------------------------------------------
        void TryShoot()
        {
            if (_currentTarget == null) return;

            _fireCooldown -= Time.deltaTime;
            if (_fireCooldown > 0) return;

            bool isAimingAtTarget = false;
            
            // check if turret direction is toward target on Z axis
            Physics.Raycast(_firePoint.position,_firePoint.forward, out RaycastHit hitInfo, _turretSo.AttackRange, LayerMask.NameToLayer("Enemies"), QueryTriggerInteraction.Collide);
            if (hitInfo.collider != null && hitInfo.collider.transform == _currentTarget)
            {
                isAimingAtTarget = true;
            }
            
            if (!isAimingAtTarget) return;

            // Fire
            _fireCooldown = 1f / FormulaCollection.GetFireRate(_turretSo.BaseFireRate,
                SkillLevel);
            Shoot();
        }


        public virtual void Shoot()
        {
            Bullet bullet = ObjectPoolManager.Instance.Get(_bulletPrefab.gameObject).GetComponent<Bullet>();
            bullet.transform.position = _firePoint.position;
            bullet.transform.rotation = _firePoint.rotation;
            bullet.Damage = _turretSo.BaseDamage;
            bullet.Speed = _turretSo.BulletSpeed;
        }

        public void CreateTask()
        {
            AddTask(new ManningTurretTask(this,_actionPoint,TaskType.ManningTurrets));
        }
    }
}