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
    private Vector3 groundVelocity = Vector3.zero;
    private float verticalVelocity = 0.0f;
    [SerializeField] private float gravity = -20.0f;

    // Update is called once per frame
    void Update()
    {
        //==================
        // Get Player Input
        //==================

        //Read movement value recieved by player input component
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();


        //============================
        // Calculate Desired Movement
        //============================

        // Convert the input into a direction relative to where they're facing.
        Vector3 desiredDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Stops diagonal movement from being faster.
        desiredDirection.Normalize();

        // Calculates the velocity the player should reach.
        Vector3 targetVelocity = desiredDirection * maxSpeed;


        //================
        // Apply Momentum 
        //================

        // Check if the player is giving input.
        bool bIsMoving = desiredDirection.sqrMagnitude > 0.01f;

        // Use acceleration if moving, or deceleration if not.
        float rate = bIsMoving ? acceleration : deceleration;

        // Gradually set the player's velocity towards the target velocity.
        groundVelocity = Vector3.MoveTowards(groundVelocity, targetVelocity, rate * Time.deltaTime);

        //  Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        // Stop vertical velocity from occuring if the player is grounded.
        if (controller.isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -2.0f;
        }

        Vector3 movement = groundVelocity + Vector3.up * verticalVelocity;

        // Move the player.
        controller.Move(movement * Time.deltaTime);
    }
}
