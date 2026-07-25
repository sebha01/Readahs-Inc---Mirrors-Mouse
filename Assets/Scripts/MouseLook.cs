using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    float mouseX;
    float mouseY;
    float xRotation;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 100.0f;

    [Header("Player References")]
    [SerializeField]private PlayerInput playerInput;
    public Transform playerBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = playerInput.actions["Look"].ReadValue<Vector2>();

        mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
