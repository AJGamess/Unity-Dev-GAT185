using UnityEngine;

// This script is attached to the projectiles fired by the cannon.
public class CanonBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit the player; if so, activate their ragdoll.
        if (collision.transform.CompareTag("Player"))
        {
            GameObject.Find("Shannon").GetComponent<RagdollEnabler>().EnableRagdoll();
            Debug.Log("Hit");
        }
    }
}
