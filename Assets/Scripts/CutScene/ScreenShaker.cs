using UnityEngine;
using System.Collections;

public class ScreenShaker : MonoBehaviour
{
    [Header("Signal Shake Settings")]
    [Tooltip("Shake duration when triggered by Timeline Signal")]
    public float signalDuration = 0.3f;

    [Tooltip("Shake strength when triggered by Timeline Signal")]
    public float signalStrength = 0.65f;

    private Vector3 originalLocalPos;
    private Coroutine shakeCoroutine;
    
    public void ShakeFromSignal()
    {
        Shake(signalDuration, signalStrength);
    }
    void Shake(float duration, float strength)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        originalLocalPos = transform.localPosition;
        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, strength));
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            ) * strength;

            transform.localPosition = originalLocalPos + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPos;
    }
}