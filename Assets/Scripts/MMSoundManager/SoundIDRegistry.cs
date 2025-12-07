using System.Collections.Generic;
using UnityEngine;

public class SoundIDRegistry : MonoBehaviour
{
    private static SoundIDRegistry _instance;
    public static SoundIDRegistry Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find existing
                _instance = FindFirstObjectByType<SoundIDRegistry>();

                // No registry in scene → create one
                if (_instance == null)
                {
                    GameObject obj = new GameObject("SoundIDRegistry");
                    _instance = obj.AddComponent<SoundIDRegistry>();
                    DontDestroyOnLoad(obj);
                }
            }

            return _instance;
        }
    }

    // Active IDs currently in use
    private HashSet<int> usedIDs = new HashSet<int>();

    // Pool of released IDs that can be reused
    private Queue<int> availableIDs = new Queue<int>();

    private int lastID = 1000;

    /// <summary>
    /// Returns a unique ID. Reuses released IDs when possible.
    /// </summary>
    public int GetUniqueID()
    {
        int id;

        // First, try to reuse a released ID
        if (availableIDs.Count > 0)
        {
            id = availableIDs.Dequeue();
        }
        else
        {
            // No available IDs, create a new one
            lastID++;

            // Skip any IDs that are somehow still in use
            while (usedIDs.Contains(lastID))
            {
                lastID++;
            }

            id = lastID;
        }

        usedIDs.Add(id);
        return id;
    }

    /// <summary>
    /// Releases an ID back to the pool for reuse.
    /// Call this when a sound is stopped and freed.
    /// </summary>
    public void ReleaseID(int id)
    {
        if (usedIDs.Remove(id))
        {
            availableIDs.Enqueue(id);
        }
    }

    /// <summary>
    /// Clear all IDs (useful for scene transitions).
    /// </summary>
    public void Clear()
    {
        usedIDs.Clear();
        availableIDs.Clear();
        lastID = 1000;
    }

#if UNITY_EDITOR
    // Debug info for Inspector
    [Header("Debug Info (Read-Only)")]
    [SerializeField] private int activeIDCount;
    [SerializeField] private int availableIDCount;
    [SerializeField] private int nextID;

    private void Update()
    {
        activeIDCount = usedIDs.Count;
        availableIDCount = availableIDs.Count;
        nextID = lastID + 1;
    }
#endif
}