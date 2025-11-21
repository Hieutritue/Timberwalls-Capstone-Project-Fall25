using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.ShieldSystem
{
    public class ShieldHealthUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _shieldHealthText;
        [SerializeField] private Slider _shieldHealthSlider;
        
        private void Start()
        {
            ShieldSystem.Instance.OnCurrentHealthChanged += UpdateShieldHealthUI;
            ShieldSystem.Instance.OnMaxHealthChanged += UpdateShieldHealthUI;
            UpdateShieldHealthUI();
        }

        private void UpdateShieldHealthUI()
        {
            float currentHealth = ShieldSystem.Instance.CurrentHealth;
            float maxHealth = ShieldSystem.Instance.MaxHealth;

            _shieldHealthText.text = $"{currentHealth} / {maxHealth} HP";
            _shieldHealthSlider.maxValue = maxHealth;
            _shieldHealthSlider.value = currentHealth;
        }
    }
}