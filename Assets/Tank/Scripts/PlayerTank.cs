using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    [SerializeField] float maxTorque = 90;
    [SerializeField] float maxForce = 1;
    [SerializeField] Transform barrel;
    [SerializeField] GameObject rocket;

    float torque;
    float force;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        torque = Input.GetAxis("Horizontal") * maxTorque;
        force = Input.GetAxis("Vertical") * maxForce;

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(rocket, barrel.position, barrel.rotation);
        }
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.forward * force);
        rb.AddRelativeTorque(Vector3.up * torque);
    }
}
