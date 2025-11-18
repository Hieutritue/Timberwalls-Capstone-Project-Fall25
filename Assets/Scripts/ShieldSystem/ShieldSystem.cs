using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.ShieldSystem
{
    public class ShieldSystem : MonoSingleton<ShieldSystem>
    {
        [SerializeField] private ShieldLevelSO[] _shieldLevelSos;
        
        private ShieldLevelSO _currentLevelSo;
        private int _currentHealth;
        private int _maxHealth;

        public Action OnCurrentHealthChanged;
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                OnCurrentHealthChanged?.Invoke();
            }
        }
        
        public Action OnMaxHealthChanged;
        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = Mathf.Max(0, value);
                OnMaxHealthChanged?.Invoke();
            }
        }
        
        [Button]
        public void SetShieldLevel(int level)
        {
            if (level < 0 || level >= _shieldLevelSos.Length)
            {
                Debug.LogError("Invalid shield level: " + level);
                return;
            }
            
            var missingHealth = MaxHealth - CurrentHealth;

            _currentLevelSo = _shieldLevelSos[level];
            MaxHealth = Mathf.RoundToInt(_currentLevelSo.Health);
            CurrentHealth = MaxHealth - missingHealth;
        }
    }
}