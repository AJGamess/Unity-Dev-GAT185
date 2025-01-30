using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float senstiivity = 1;
    [SerializeField, Range(2, 10)] float distance;

    InputAction lookAction;
    Vector3 rotation = Vector3.zero; // x = pitch, y = yaw, z = roll

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        lookAction.performed += Look;
        //lookAction.canceled += Look;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion qrotation = Quaternion.Euler(rotation);
        transform.position = target.position + qrotation * (Vector3.back * distance);
        transform.rotation = qrotation;
    }

    void Look(InputAction.CallbackContext ctx)
    {
        var look = ctx.ReadValue<Vector2>();

        rotation.x += look.y * senstiivity;
        rotation.y += look.x * senstiivity;

        rotation.x = Mathf.Clamp(rotation.x, 20, 80);
    }
}
