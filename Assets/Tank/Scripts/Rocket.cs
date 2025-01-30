using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float force = 10;
    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * force, ForceMode.VelocityChange);
    }
}
