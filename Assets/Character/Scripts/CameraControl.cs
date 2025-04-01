using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float senstiivity = 1;
    [SerializeField, Range(2, 10)] float distance;

    InputAction lookAction;
    Vector2 lookInput;
    Vector3 rotation = Vector3.zero; // x = pitch, y = yaw, z = roll

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        lookAction.performed += Look;
        lookAction.canceled += Look;

        Quaternion qrotation = Quaternion.Euler(rotation);
        rotation.x = qrotation.eulerAngles.x;
        rotation.y = qrotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        rotation.x += lookInput.y * senstiivity;
        rotation.y += lookInput.x * senstiivity;

        rotation.x = Mathf.Clamp(rotation.x, 20, 80);

        Quaternion qrotation = Quaternion.Euler(rotation);
        transform.position = target.position + qrotation * (Vector3.back * distance);
        transform.rotation = qrotation;
    }

    void Look(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }
}
