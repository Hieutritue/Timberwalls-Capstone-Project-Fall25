using System;
using UnityEngine;

namespace DefaultNamespace.Lighting
{
    [Serializable]
    [CreateAssetMenu(fileName = "LightPreset", menuName = "LightingPreset")]
    public class LightingPreset : ScriptableObject
    {
        public Gradient AmbientColor;
        public Gradient DirectionalColor;
        public Gradient FogColor;
    }
}