using UnityEngine;

public class MusicNote : MonoBehaviour
{
    public float speed = 5f;
    public float waveAmplitude = 0.5f;
    public float waveFrequency = 4f;
    public float lifeTime = 3f;

    private float timeAlive;

    void OnEnable()
    {
        timeAlive = 0f;
        // Auto-disable after lifeTime
        Invoke(nameof(DisableSelf), lifeTime);
    }

    void Update()
    {
        timeAlive += Time.deltaTime;

        // Move forward
        transform.position += transform.right * speed * Time.deltaTime;

        // Wave vertically
        float wave = Mathf.Sin(timeAlive * waveFrequency) * waveAmplitude;
        transform.position += Vector3.up * wave * Time.deltaTime;
    }

    void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}