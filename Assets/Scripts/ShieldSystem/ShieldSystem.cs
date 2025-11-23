using System;
using ShieldSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.ShieldSystem
{
    public class ShieldSystem : MonoSingleton<ShieldSystem>
    {
        public ShieldWall ShieldWall;
        
        [SerializeField] private ShieldHpLevelSO[] _shieldHpSos;
        [SerializeField] private ShieldMaintainabilityLevelSo[] _shieldMaintainabilitySos;
        
        private ShieldHpLevelSO _currentHpLevelSo;
        private ShieldMaintainabilityLevelSo _currentMaintainabilitySo;
        private float _currentHealth;
        private float _maxHealth;

        public Action OnCurrentHealthChanged;
        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                OnCurrentHealthChanged?.Invoke();
            }
        }
        
        public Action OnMaxHealthChanged;
        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = Mathf.Max(0, value);
                OnMaxHealthChanged?.Invoke();
            }
        }
        
        [Button]
        public void SetShieldHpLevel(int level)
        {
            if (level < 0 || level >= _shieldHpSos.Length)
            {
                Debug.LogError("Invalid shield level: " + level);
                return;
            }
            
            var missingHealth = MaxHealth - CurrentHealth;

            _currentHpLevelSo = _shieldHpSos[level];
            MaxHealth = Mathf.RoundToInt(_currentHpLevelSo.Health);
            CurrentHealth = MaxHealth - missingHealth;
        }
        
        public void SetShieldMaintainabilityLevel(int level)
        {
            if (level < 0 || level >= _shieldMaintainabilitySos.Length)
            {
                Debug.LogError("Invalid shield maintainability level: " + level);
                return;
            }
            
            _currentMaintainabilitySo = _shieldMaintainabilitySos[level];
        }
        
        public void ReceiveDamage(float damage)
        {
            CurrentHealth -= damage;
        }
    }
}