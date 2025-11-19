using UnityEngine;

public class MusicNoteShooter : MonoBehaviour
{
    public MusicNotePool pool;
    public float shootInterval = 0.5f;

    void Start()
    {
        InvokeRepeating(nameof(ShootNote), 0f, shootInterval);
    }

    void ShootNote()
    {
        GameObject note = pool.GetNote();

        // Place note at shooter's position (world space)
        note.transform.position = transform.position;
        note.transform.rotation = transform.rotation;
    }
}