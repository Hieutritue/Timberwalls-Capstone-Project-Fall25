using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class MaterialSwapper : MonoBehaviour
{
    // Store original materials per Renderer
    private readonly Dictionary<Renderer, Material[]> _originalMaterials = new Dictionary<Renderer, Material[]>();

    // Apply highlight
    public void ApplyHighlight(GameObject gameObject, Material material)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            // Save original
            _originalMaterials[rend] = rend.materials;

            // Replace with highlight material for all slots
            Material[] mats = new Material[rend.materials.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = material;

            rend.materials = mats;
        }
    }

    [Button]
    // Restore originals
    public void Restore()
    {
        foreach (var kvp in _originalMaterials)
        {
            if (kvp.Key != null) // in case object was destroyed
                kvp.Key.materials = kvp.Value;
        }

        _originalMaterials.Clear();
    }

    public void RemoveHighlight(GameObject gameObject)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            if (_originalMaterials.TryGetValue(rend, out var original))
            {
                if (rend != null)
                    rend.materials = original;

                _originalMaterials.Remove(rend);
            }
        }
    }
}