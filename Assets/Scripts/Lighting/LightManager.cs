using System;
using DefaultNamespace.ScheduleSystem;
using UnityEditor.Presets;
using UnityEngine;

namespace DefaultNamespace.Lighting
{
    public class LightManager : MonoBehaviour
    {
        [SerializeField] private Light _directionalLight;
        [SerializeField] private LightingPreset _lightingPreset;

        private float _timeOfDayPercent;

        private void Update()
        {
            if (Application.isPlaying)
            {
                _timeOfDayPercent = GameTimeManager.Instance.GetCurrentTimeOfDayPercent();
                UpdateLighting(_timeOfDayPercent);
            }
        }

        private void UpdateLighting(float timerPercent)
        {
            RenderSettings.ambientLight = _lightingPreset.AmbientColor.Evaluate(timerPercent);
            RenderSettings.fogColor = _lightingPreset.FogColor.Evaluate(timerPercent);

            if (_directionalLight)
            {
                _directionalLight.color = _lightingPreset.DirectionalColor.Evaluate(timerPercent);
                _directionalLight.transform.localRotation =
                    Quaternion.Euler(new Vector3(0, 170, (timerPercent * 360f) - 90f));
            }
        }
        
        private void OnValidate()
        {
            if (!_directionalLight) return;
            if (RenderSettings.sun)
            {
                _directionalLight = RenderSettings.sun;
            }
            else
            {
                Light[] lights = GameObject.FindObjectsByType<Light>(FindObjectsSortMode.None);
                foreach (var light in lights)
                {
                    if (light.type == LightType.Directional)
                    {
                        _directionalLight = light;
                        return;
                    }
                }
            }
        }
    }
}