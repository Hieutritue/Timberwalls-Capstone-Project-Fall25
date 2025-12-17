using UnityEngine;

public class SleighMover : MonoBehaviour
{
    [Header("Horizontal bounds")]
    public float minX = -30f;
    public float maxX = 30f;

    [Header("Motion")]
    public float speed = 6f;
    public float verticalBobAmplitude = 0.5f;
    public float verticalBobFrequency = 1.2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Horizontal movement
        transform.position += Vector3.right * speed * Time.deltaTime;

        // Vertical bobbing
        float bob = Mathf.Sin(Time.time * verticalBobFrequency) * verticalBobAmplitude;
        transform.position = new Vector3(
            transform.position.x,
            startPos.y + bob,
            transform.position.z
        );

        // Wraparound
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(
                minX,
                transform.position.y,
                transform.position.z
            );
        }
    }
}
