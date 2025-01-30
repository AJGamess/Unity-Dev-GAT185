using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] float damage = 1;
    [SerializeField] bool destroyOnHit = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamagable component))
        {
            component.ApplyDamage(damage);
        }

        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable component))
        {
            component.ApplyDamage(damage * Time.deltaTime);
        }
    }
}
