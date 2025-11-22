using UnityEngine;

public class Flame : MonoBehaviour
{
    [SerializeField] private GameObject flame;

    public void EnableFlame()
    {
        flame.SetActive(true);
    }

    public void DisableFlame()
    {
        flame.SetActive(false);
    }

}
