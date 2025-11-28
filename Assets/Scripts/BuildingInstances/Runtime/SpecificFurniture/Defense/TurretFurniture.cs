using DefaultNamespace.ColonistSystem;
using DefaultNamespace.Enemy;
using DefaultNamespace.General;
using DefaultNamespace.TaskSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace BuildingSystem
{
    public class TurretFurniture : Furniture, ITaskCreator
    {
        public Colonist ColonistAssignedToTurret { get; set; }
        [Header("Refs")] [SerializeField] private Transform[] _firePoints; // where bullets spawn
        [SerializeField] private Transform _partToRotate; // rotating head of the turret
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _actionPoint;

        [SerializeField] private Animator _animator;
        protected TurretFurnitureSo TurretSo => (TurretFurnitureSo)PlaceableSo;

        private float _fireCooldown;
        private Transform _currentTarget;
        private static readonly int Attack = Animator.StringToHash("IsActive");
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
                if (!_currentTarget.gameObject.activeInHierarchy ||
                    Vector3.Distance(transform.position, _currentTarget.position) > TurretSo.AttackRange)
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

            // Get direction in WORLD space
            Vector3 worldDir = _currentTarget.position - _partToRotate.position;

            // Convert to LOCAL space of the parent (important!)
            Vector3 localDir = _partToRotate.parent.InverseTransformDirection(worldDir);

            // Compute the angle on Z axis using local XY plane
            float targetAngle = Mathf.Atan2(localDir.y, localDir.x) * Mathf.Rad2Deg;

            // Current angle
            float currentAngle = _partToRotate.localEulerAngles.z;

            // Rotate smoothly
            float speed = FormulaCollection.GetTurretRotationSpeed(TurretSo.BaseTraverseSpeed, SkillLevel);
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, speed * Time.deltaTime);

            // Apply rotation ONLY on Z (in local space)
            _partToRotate.localRotation = Quaternion.Euler(0f, 0f, newAngle);
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
            Physics.Raycast(_firePoints[0].position, _firePoints[0].forward, out RaycastHit hitInfo,
                TurretSo.AttackRange,
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
            
            var randomFirePoint = _firePoints[Random.Range(0, _firePoints.Length)];
            if (_animator)
                _animator.SetTrigger(Attack);
            Bullet bullet = ObjectPoolManager.Instance.Get(_bulletPrefab.gameObject).GetComponent<Bullet>();
            bullet.transform.position = randomFirePoint.position;
            bullet.transform.rotation = randomFirePoint.rotation;
            bullet.Damage = TurretSo.BaseDamage;
        }

        public void CreateTask()
        {
            AddTask(new ManningTurretTask(this, _actionPoint, TaskType.ManningTurrets));
        }
    }
}