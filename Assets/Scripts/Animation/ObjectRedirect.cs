using UnityEngine;

public class ObjectRedirect : MonoBehaviour
{
    public Animals controller;
    public int index;
    private float lastRedirect = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        // Avoid spam redirect when sliding along the wall
        if (Time.time - lastRedirect < 0.4f) return;

        controller.SetNewTarget(index);
        lastRedirect = Time.time;
    }
}


