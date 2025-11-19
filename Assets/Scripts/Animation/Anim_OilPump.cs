using UnityEngine;

public class OilPump : MonoBehaviour
{
    [SerializeField] private GameObject oilBulge;

    public void DisableOilBulge()
    {
        oilBulge.SetActive(false);
    }
}
