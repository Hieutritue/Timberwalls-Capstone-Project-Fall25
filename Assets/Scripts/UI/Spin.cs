using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float speed = 50f;

    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}
