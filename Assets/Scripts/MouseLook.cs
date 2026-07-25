using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    //Mouse movement coordinates
    float mouseX;
    float mouseY;
    float xRotation;

    //Player component references
    [Header("Player Component References")]
    [SerializeField] private PlayerInput playerInput;
    public Transform playerBody;

    //Mouse settings
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Read the look input from the player input component
        Vector2 mouseDelta = playerInput.actions["Look"].ReadValue<Vector2>();

        //Set x and y appropriately
        mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        //Adjust rotate it and clamp it within a range so player doesnt look behind them without turning their body
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotate player body based on where the mouse looks on the x axis
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
