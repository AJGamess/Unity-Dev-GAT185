using UnityEngine;

public class SpinningLog : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // Adjust speed in the Inspector

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}

