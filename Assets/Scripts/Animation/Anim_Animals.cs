using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Animals : MonoBehaviour
{
    [Header("Animal Settings")]
    public List<GameObject> animals = new List<GameObject>();
    public float moveSpeed = 3f;
    public float range = 3f;
    public float stoppingDistance = 0.3f;
    public float stuckCheckTime = 1f;

    protected Vector3[] targetPositions;
    protected Vector3[] startPositions;
    protected Vector3[] lastPositions;
    protected float[] stuckTimers;

    protected Rigidbody[] animalRigidbodies;

    public bool Initialized { get; private set; } = false;

    protected virtual void Start()
    {
        int animalLayer = LayerMask.NameToLayer("Animal");
        Physics.IgnoreLayerCollision(animalLayer, animalLayer, true);

        int count = animals.Count;

        targetPositions = new Vector3[count];
        startPositions = new Vector3[count];
        lastPositions = new Vector3[count];
        stuckTimers = new float[count];
        animalRigidbodies = new Rigidbody[count];

        for (int i = 0; i < count; i++)
        {
            GameObject animal = animals[i];
            startPositions[i] = animal.transform.position;
            lastPositions[i] = animal.transform.position;

            // Ensure each animal has a Rigidbody
            Rigidbody rb = animal.GetComponent<Rigidbody>();
            if (rb == null) rb = animal.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            animalRigidbodies[i] = rb;

            // Add redirect component for collisions
            var redirect = animal.GetComponent<ObjectRedirect>();
            if (redirect == null) redirect = animal.AddComponent<ObjectRedirect>();

            redirect.index = i;
            redirect.controller = this;

            // SAFE — arrays already exist
            SetNewTarget(i);
        }

        Initialized = true;  // mark as ready!
    }

    protected virtual void FixedUpdate()
    {
        if (!Initialized) return; // Safety

        for (int i = 0; i < animals.Count; i++)
        {
            GameObject animal = animals[i];
            if (animal == null) continue;

            Rigidbody rb = animalRigidbodies[i];
            Vector3 target = targetPositions[i];

            // Stop movement entirely if velocity is zero (optional)
            if (rb.linearVelocity.sqrMagnitude < 0.01f)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Calculate direction
            Vector3 dir = (target - animal.transform.position);
            float distance = dir.magnitude;

            if (distance < stoppingDistance)
            {
                SetNewTarget(i);
                continue;
            }

            dir.Normalize();

            // Smooth rotation
            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * 2f));
            }

            // Move using Rigidbody
            rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

            // Stuck detection
            float moved = (animal.transform.position - lastPositions[i]).sqrMagnitude;
            if (moved < 0.0001f)
            {
                stuckTimers[i] += Time.fixedDeltaTime;
                if (stuckTimers[i] > stuckCheckTime)
                {
                    SetNewTarget(i);
                    stuckTimers[i] = 0;
                }
            }
            else
            {
                stuckTimers[i] = 0;
            }

            lastPositions[i] = animal.transform.position;
        }
    }

    public void SetNewTarget(int index)
    {
        if (!Initialized) return; // Prevent early calls

        float randomX = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);
        Vector3 start = startPositions[index];

        targetPositions[index] = new Vector3(start.x + randomX, start.y, start.z + randomZ);
    }
    
    public void StopAnimal(int index)
    {
        Rigidbody rb = animalRigidbodies[index];
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
