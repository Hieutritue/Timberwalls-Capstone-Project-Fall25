using System.Collections.Generic;
using UnityEngine;

public class MusicNotePool : MonoBehaviour
{
    public GameObject musicNotePrefab;
    public int poolSize = 4;

    private List<GameObject> pool;

    void Awake()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            // Instantiate as child of this GameObject
            GameObject note = Instantiate(musicNotePrefab, transform);
            note.transform.localPosition = Vector3.zero;
            note.transform.localRotation = Quaternion.identity;
            note.SetActive(false);
            pool.Add(note);
        }
    }

    // Get an inactive note from the pool
    public GameObject GetNote()
    {
        foreach (GameObject note in pool)
        {
            if (!note.activeInHierarchy)
            {
                note.SetActive(true);
                return note;
            }
        }

        // Optionally expand pool if all are in use
        GameObject newNote = Instantiate(musicNotePrefab, transform);
        newNote.transform.localPosition = Vector3.zero;
        newNote.transform.localRotation = Quaternion.identity;
        pool.Add(newNote);
        return newNote;
    }
}