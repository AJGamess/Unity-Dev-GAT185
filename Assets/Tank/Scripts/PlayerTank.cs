using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTank : MonoBehaviour
{
    [SerializeField] float maxTorque = 90;
    [SerializeField] float maxForce = 1;
    [SerializeField] Transform barrel;
    [SerializeField] GameObject rocket;
    public int ammo = 10;
    public int health = 10;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] Slider healthSlider;

    float torque;
    float force;

    Rigidbody rb;
    Destructable destructable;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        destructable = GetComponent<Destructable>();
    }

    void Update()
    {
        torque = Input.GetAxis("Horizontal") * maxTorque;
        force = Input.GetAxis("Vertical") * maxForce;

        if (Input.GetButtonDown("Fire1") && ammo > 0)
        {
            ammo--;
            Instantiate(rocket, barrel.position, barrel.rotation);
        }

        ammoText.text = "Ammo: " + ammo.ToString();
        healthSlider.value = destructable.Health;
        if (destructable.Health <= 0)
        {
            GameManager.Instacnce.SetGameOver();
        }
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.forward * force);
        rb.AddRelativeTorque(Vector3.up * torque);
    }
}
