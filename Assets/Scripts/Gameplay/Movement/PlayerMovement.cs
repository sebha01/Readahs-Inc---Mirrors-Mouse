using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //movement variables
    float x;
    float z;

    //player component references
    [Header("Player Component References")]
    [SerializeField] private PlayerInput playerInput;
    public CharacterController controller;

    //movement variables
    [Header("Movement Variables")]
    [SerializeField] private float maxSpeed = 8.0f;
    [SerializeField] private float acceleration = 2.0f;
    [SerializeField] private float deceleration = 20.0f;
    private Vector3 currentVelocity = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        //Read movement value recieved by player input component
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        //get x and z from move input
        x = moveInput.x;
        z = moveInput.y;

        //move player in direction based on input recieved
        Vector3 desiredDirection = transform.right * x + transform.forward * z;
        desiredDirection.Normalize();

        Vector3 targetVelocity = desiredDirection * maxSpeed;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

        controller.Move(currentVelocity * Time.deltaTime);
    }
}
