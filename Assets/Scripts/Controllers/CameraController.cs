using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool enableCameraLook = true;
    public bool enableCameraMove = true;

    [Header("Controls")]
    public float sensitivity = 10.0f;

    [Header("Movement")]
    public float maxSpeed = 100.0f;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private Vector2 mouse;

    [Header("User Interface")]
    public GameObject UIPanel;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LookAround()
    {
        mouse.x = Input.GetAxis("Mouse X") * sensitivity;
        mouse.y = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouse.y;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        yRotation += mouse.x;
        yRotation = Mathf.Clamp(yRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0.0f);
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        float z = Input.GetAxisRaw("Up");

        Vector3 forward = Vector3.MoveTowards(Vector3.zero, transform.forward * maxSpeed, y * maxSpeed * Time.deltaTime);
        Vector3 sideways = Vector3.MoveTowards(Vector3.zero, transform.right * maxSpeed, x * maxSpeed * Time.deltaTime);
        Vector3 up = Vector3.MoveTowards(Vector3.zero, transform.up * maxSpeed, z * maxSpeed * Time.deltaTime);
        transform.position += forward + sideways + up;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIPanel.activeInHierarchy)
        {
            if (enableCameraLook)
                LookAround();

            if (enableCameraMove)
                Move();
        }
    }
}
