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
            ShieldSystem.Instance.ShieldWall.OnCurrentHealthChanged += UpdateShieldHealthUI;
            ShieldSystem.Instance.ShieldWall.OnMaxHealthChanged += UpdateShieldHealthUI;
        }

        private void UpdateShieldHealthUI(float currentHealth, float maxHealth)
        {
            _shieldHealthText.text = $"{(int)currentHealth} / {maxHealth} HP";
            _shieldHealthSlider.maxValue = maxHealth;
            _shieldHealthSlider.value = currentHealth;
        }
    }
}