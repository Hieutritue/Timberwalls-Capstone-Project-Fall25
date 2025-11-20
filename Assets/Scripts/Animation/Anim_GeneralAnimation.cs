using System;
using UnityEngine;

public class Anim_GeneralAnimation : MonoBehaviour
{
    private Vector3 originalScale;
    [Header("Melt Transform")]
    public float meltAmount = 0.9f;
    public float dripIntensity = 0.05f;
    [Header("Timing")]
    public float duration = 5f;
    private float timer;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    // public void Shake()
    // {
    //     float t = Mathf.Clamp01(timer / duration);
    //     float meltY = Mathf.Lerp(1f, meltAmount, t);
    //     float wiggle = Mathf.Sin(Time.time * 20f) * dripIntensity * t;
    //     transform.localScale = new Vector3(
    //         originalScale.x + wiggle,
    //         originalScale.y * meltY,
    //         originalScale.z + wiggle
    //     );
    // }
    //
    // void Update()
    // {
    //     timer += Time.deltaTime;
    // }
}
