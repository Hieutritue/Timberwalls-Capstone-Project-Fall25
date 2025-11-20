using UnityEngine;

public class Furnace : MonoBehaviour
{
    [Header("Renderer")]
    public Renderer rend;

    public bool rotate = true;
    [Header("Heat Colors")]
    public Color coldColor = Color.white;    // màu bình thường
    public Color hotColor = new Color(1f, 0.2f, 0f); // đỏ cam

    [Header("Emission")]
    public float coldEmission = 0f;
    public float hotEmission = 5f;

    [Header("Metallic & Smoothness")]
    public float coldMetallic = 0f;
    public float hotMetallic = 1f;

    public float coldSmoothness = 0.3f;
    public float hotSmoothness = 1f;

    [Header("Melt Transform")]
    public float meltAmount = 0.9f; 
    public float dripIntensity = 0.05f; 

    [Header("Timing")]
    public float duration = 5f;

    private float timer;
    private MaterialPropertyBlock mpb;
    private Vector3 originalScale;
    public float rotateSpeed = 20f;

    void Start()
    {
        if (rend == null) rend = GetComponent<Renderer>();

        mpb = new MaterialPropertyBlock();
        originalScale = transform.localScale;
        rend.material.EnableKeyword("_EMISSION");
        rend.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", coldColor);
        mpb.SetColor("_EmissionColor", coldColor * coldEmission);
        mpb.SetFloat("_Metallic", coldMetallic);
        mpb.SetFloat("_Smoothness", coldSmoothness);
        rend.SetPropertyBlock(mpb);
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        Color newColor = Color.Lerp(coldColor, hotColor, t);
        float newEmission = Mathf.Lerp(coldEmission, hotEmission, t);
        float newMetallic = Mathf.Lerp(coldMetallic, hotMetallic, t);
        float newSmoothness = Mathf.Lerp(coldSmoothness, hotSmoothness, t);
        rend.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", newColor);
        mpb.SetColor("_EmissionColor", newColor * newEmission);
        mpb.SetFloat("_Metallic", newMetallic);
        mpb.SetFloat("_Smoothness", newSmoothness);
        rend.SetPropertyBlock(mpb);
        float meltY = Mathf.Lerp(1f, meltAmount, t);
        float wiggle = Mathf.Sin(Time.time * 20f) * dripIntensity * t;
        transform.localScale = new Vector3(
            originalScale.x + wiggle,
            originalScale.y * meltY,
            originalScale.z + wiggle
        );
        if(rotate)
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
    }
}