using UnityEngine;

public class BatteryProjectile : MonoBehaviour
{
    public LayerMask hitMask;
    [SerializeField] private ShootBattery shootingInformation;

    private void OnCollisionEnter(Collision collision)
    {
        if ((hitMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            gameObject.SetActive(false);
            transform.position = shootingInformation.spawnPoint.position;
        }
    }
}
