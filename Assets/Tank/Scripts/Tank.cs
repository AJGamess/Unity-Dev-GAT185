using UnityEngine;

public class tank : MonoBehaviour
{
    [SerializeField] float turnRate = 90;
    [SerializeField] float maxSpeed = 1;
    [SerializeField] GameObject rocket;

    void Start()
    {
        
    }

    void Update()
    {
        float rotation = Input.GetAxis("Horizontal");
        float speed = Input.GetAxis("Vertical");

        transform.rotation = transform.rotation * Quaternion.AngleAxis(rotation * turnRate * Time.deltaTime, Vector3.up);
        //transform.position += Vector3.forward * speed * maxSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * speed * maxSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(rocket, transform.position + Vector3.up, transform.rotation);
        }
    }
}
