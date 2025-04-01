using UnityEngine;

// This script allows shooting projectiles at a target and is attached to the "Canon" GameObject.
public class Canon : MonoBehaviour
{
    public Transform target;
    public GameObject canonBall;
    public float firePower = 70f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Spawn the projectile
            GameObject projectile = Instantiate(canonBall);
            projectile.transform.position = transform.position;

            // Send the projectile toward the target (the character)
            Vector3 direction = (target.position - transform.position).normalized;
            projectile.GetComponent<Rigidbody>().AddForce(direction * firePower, ForceMode.Impulse);
        }
    }
}
