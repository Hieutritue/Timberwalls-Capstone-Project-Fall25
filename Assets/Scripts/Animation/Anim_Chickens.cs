using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chickens : Animals
{
    private readonly string EAT = "Eat_b";
    private bool[] isEating;

    protected override void Start()
    {
        base.Start();
        int count = animals.Count;
        isEating = new bool[count];

        // Start random eating routine
        StartCoroutine(RandomEatRoutine());
    }

    private IEnumerator RandomEatRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f)); // wait before next chicken eats

            int index = Random.Range(0, animals.Count);
            if (!isEating[index])
            {
                StartEating(index);
            }
        }
    }

    private void StartEating(int index)
    {
        GameObject chicken = animals[index];
        Rigidbody rb = chicken.GetComponent<Rigidbody>();

        // Stop movement
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Play animation
        chicken.GetComponent<Animator>().SetBool(EAT, true);
        isEating[index] = true;

        // Resume after delay
        StartCoroutine(StopEating(index, 2f));
    }

    private IEnumerator StopEating(int index, float duration)
    {
        yield return new WaitForSeconds(duration);
        GameObject chicken = animals[index];
        chicken.GetComponent<Animator>().SetBool(EAT, false);
        isEating[index] = false;

        // Pick a new target so it can roam again
        SetNewTarget(index);
    }

    protected override void FixedUpdate()
    {
        // Move all chickens that are NOT eating
        for (int i = 0; i < animals.Count; i++)
        {
            if (isEating[i]) continue;

            GameObject animal = animals[i];
            Rigidbody rb = animal.GetComponent<Rigidbody>();
            Vector3 target = targetPositions[i];

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
}
