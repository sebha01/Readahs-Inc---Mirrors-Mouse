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
    [Header("Ground Movement")]
    [SerializeField] private float maxSpeed = 8.0f;
    [SerializeField] private float acceleration = 2.0f;
    [SerializeField] private float deceleration = 20.0f;
    
    [Header("Gravity")]
    [SerializeField] private float gravity = -20.0f;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 2.0f;

    [Header("Air Movement")]
    [SerializeField][Range(0.0f, 1.0f)] private float airControl = 0.35f;

    [Header("Other Stuff")]
    private Vector3 groundVelocity = Vector3.zero;
    private float verticalVelocity = 0.0f;

    // Update is called once per frame
    void Update()
    {
        //==================
        // Get Player Input
        //==================

        //Read movement value recieved by player input component
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Check if jump was pressed.
        bool jumpPressed = playerInput.actions["Jump"].WasPressedThisFrame();

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

        // Calculate acceleration based on if the player is on the floor or not.
        float currentAcceleration = controller.isGrounded ? acceleration : acceleration * airControl;
        
        // Apply acceleration while moving.
        if (bIsMoving)
        {
            groundVelocity = Vector3.MoveTowards(groundVelocity, targetVelocity, currentAcceleration * Time.deltaTime);
        }

        // Only decelerate while on the ground.
        else if (controller.isGrounded)
        {
            groundVelocity = Vector3.MoveTowards(groundVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        //===============
        // Gravity Stuff 
        //===============

        //  Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        // Stop vertical velocity from occuring if the player is grounded.
        if (controller.isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -2.0f;
        }

        //=========
        // Jumping 
        //=========

        if (jumpPressed && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        Vector3 movement = groundVelocity + Vector3.up * verticalVelocity;

        // Move the player.
        controller.Move(movement * Time.deltaTime);
    }
}
