using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] Transform platform;

    [SerializeField] Vector3 pointA;  // Start position
    [SerializeField] Vector3 pointB;  // End position
    [SerializeField] float speed = 2f;

    private void Start()
    {
        // Ensure platform points are set in world space correctly.
        // If platform has a parent, ensure the points are still correct in world space.
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        float t = Mathf.PingPong(Time.time * speed, 1);
        platform.position = Vector3.Lerp(pointA, pointB, t);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player entered moving platform trigger.");
            // Parent the player to the platform
            other.transform.parent = platform;
            // Optionally reset velocity if the player has a Rigidbody
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player exited moving platform trigger.");
            // Unparent the player from the platform when they leave
            other.transform.parent = null;
        }
    }
}
