using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [Header("Signal Shake Settings")]
    [Tooltip("Duration added per signal")]
    public float signalDuration = 0.25f;

    [Tooltip("Strength added per signal")]
    public float signalStrength = 0.1f;

    [Tooltip("Maximum allowed shake strength")]
    public float maxStrength = 0.35f;

    private Vector3 baseLocalPos;
    private float shakeTimer;
    private float currentStrength;

    void Awake()
    {
        baseLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        if (shakeTimer > 0f)
        {
            Vector3 offset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            ) * currentStrength;

            transform.localPosition = baseLocalPos + offset;

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = baseLocalPos;
            currentStrength = 0f;
        }
    }

    /// <summary>
    /// Parameterless method for Timeline Signal Receiver
    /// </summary>
    public void ShakeFromSignal()
    {
        baseLocalPos = transform.localPosition;

        shakeTimer = Mathf.Max(shakeTimer, signalDuration);
        currentStrength = Mathf.Min(
            currentStrength + signalStrength,
            maxStrength
        );
    }
}