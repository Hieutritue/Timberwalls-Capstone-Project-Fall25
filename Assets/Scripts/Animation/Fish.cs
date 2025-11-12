using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
     public List<GameObject> fishes = new List<GameObject>(); // Assign your fish objects in the Inspector
     public float moveSpeed = 3f;
     public float range = 1f;
     public float stoppingDistance = 0.3f;
     private Vector3[] targetPositions;
    
     void Start()
     {
         // Initialize random target for each fish
         targetPositions = new Vector3[fishes.Count];
         for (int i = 0; i < fishes.Count; i++)
         {
             SetNewTarget(i);
         }
     }
    
     void Update()
     {
         // Move each fish toward its target
         for (int i = 0; i < fishes.Count; i++)
         {
             GameObject fish = fishes[i];
             Vector3 target = targetPositions[i];
    
             if (fish == null) continue;
    
             // Move fish
             fish.transform.position = Vector3.MoveTowards( fish.transform.position,
                                                                   target,
                                                     moveSpeed * Time.deltaTime);
    
             // Optional: make fish face the direction it’s swimming
             if ((target - fish.transform.position).sqrMagnitude > 0.01f)
                 fish.transform.LookAt(target);
    
             // If it reached target, pick a new one
             if (Vector3.Distance(fish.transform.position, target) < stoppingDistance)
             {
                 SetNewTarget(i);
             }
         }
     }
    
     void SetNewTarget(int index)
     {
         float randomX = Random.Range(-range, range);
         float randomZ = Random.Range(-range, range);
         float y = fishes[index].transform.position.y; // Keep the same water level
         targetPositions[index] = new Vector3(randomX, y, randomZ);
     }
}
