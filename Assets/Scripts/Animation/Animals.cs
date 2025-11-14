using System.Collections.Generic;
using UnityEngine;

public abstract class Animals : MonoBehaviour
{
    public List<GameObject> animals = new List<GameObject>();
    public float moveSpeed = 3f;
    public float range = 3f;
    public float stoppingDistance = 0.3f;
    public float stuckCheckTime = 1f; 

    private Vector3[] targetPositions;
    private Vector3[] startPositions;
    private Vector3[] lastPositions;
    private float[] stuckTimers;
    void Start()
    {
        int animalLayer = LayerMask.NameToLayer("Animal");
        Physics.IgnoreLayerCollision(animalLayer, animalLayer, true);
        int count = animals.Count;

        targetPositions = new Vector3[count];
        startPositions = new Vector3[count];
        lastPositions = new Vector3[count];
        stuckTimers = new float[count];

        for (int i = 0; i < count; i++)
        {
            startPositions[i] = animals[i].transform.position;
            lastPositions[i] = animals[i].transform.position;

            // Add redirect component for collision turning
            var redirect = animals[i].AddComponent<ObjectRedirect>();
            redirect.index = i;
            redirect.controller = this;

            SetNewTarget(i);
        }
    }

    void Update()
    {
        for (int i = 0; i < animals.Count; i++)
        {
            var animal = animals[i];
            if (animal == null) continue;

            Vector3 target = targetPositions[i];

            // Calculate movement
            Vector3 nextPos = Vector3.MoveTowards(
                animal.transform.position,
                target,
                moveSpeed * Time.deltaTime
            );

            // Smooth turning
            Vector3 dir = (target - animal.transform.position).normalized;
            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                animal.transform.rotation = Quaternion.Slerp(
                    animal.transform.rotation,
                    targetRot,
                    Time.deltaTime * 2f   // lower = slower turn
                );
            }

            // Move
            animal.transform.position = nextPos;

            // Reached target?
            if (Vector3.Distance(animal.transform.position, target) < stoppingDistance)
            {
                SetNewTarget(i);
                continue;
            }

            // STUCK DETECTION
            float moved = (animal.transform.position - lastPositions[i]).sqrMagnitude;
            if (moved < 0.0001f)
            {
                stuckTimers[i] += Time.deltaTime;
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
        float randomX = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);

        Vector3 start = startPositions[index];

        targetPositions[index] = new Vector3(
            start.x + randomX,
            start.y,
            start.z + randomZ
        );
    }
}
