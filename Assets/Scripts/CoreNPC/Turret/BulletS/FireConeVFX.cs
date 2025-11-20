using UnityEngine;

public class FireConeVFX : MonoBehaviour
{
    [Header("Visual Settings")]
    public float coneLength = 2f;      // how far the flame extends
    public float coneWidth = 1f;       // base width of the flame
    public float fadeOutTime = 0.15f;  // disappear smoothly (no popping)

    [Header("Debug Placeholder Mesh")]
    public MeshRenderer placeholderMesh;

    private float originalAlpha = 1f;
    private float fadeTimer = 0f;
    private bool isFading = false;

    void Awake()
    {
        if (placeholderMesh != null)
        {
            // store material alpha if a placeholder mesh exists
            originalAlpha = placeholderMesh.material.color.a;
        }
    }

    void OnEnable()
    {
        fadeTimer = 0f;
        isFading = false;
        ApplySize();
        ResetAlpha();
    }

    void Update()
    {
        if (!isFading) return;

        fadeTimer += Time.deltaTime;
        float t = fadeTimer / fadeOutTime;

        if (placeholderMesh != null)
        {
            Color c = placeholderMesh.material.color;
            c.a = Mathf.Lerp(originalAlpha, 0f, t);
            placeholderMesh.material.color = c;
        }

        if (t >= 1f)
            gameObject.SetActive(false); // fully faded
    }

    // Called when the flamethrower wants to resize visually
    public void ApplySize()
    {
        // A stretched-out cone representation:
        transform.localScale = new Vector3(coneWidth, 1f, coneLength);
    }

    public void ResetAlpha()
    {
        if (placeholderMesh != null)
        {
            Color c = placeholderMesh.material.color;
            c.a = originalAlpha;
            placeholderMesh.material.color = c;
        }
    }

    public void BeginFade()
    {
        isFading = true;
        fadeTimer = 0f;
    }
}