using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] int healthAmount = 5;
    [SerializeField] GameObject pickupFx;

    private void OnCollisionEnter(Collision collision)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerTank component))
            {
                component.health += healthAmount;
                Destroy(gameObject);
                if (pickupFx != null) Instantiate(pickupFx, transform.position, Quaternion.identity);
            }
        }
    }
}
