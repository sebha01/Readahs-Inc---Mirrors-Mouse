using System.Runtime.CompilerServices;
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

    [Header("Wall Detection")]
    [SerializeField] private float wallCheckDistance = 0.75f;
    [SerializeField] private LayerMask wallLayer;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool bWallOnLeft = false;
    private bool bWallOnRight = false;

    [Header("Wall Running")]
    private bool bIsWallRunning = false;
    [SerializeField] private float wallRunGravityMultiplier = 0.35f;

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

        if (bIsWallRunning)
        {
            Vector3 wallDirection;

            if (bWallOnLeft)
            {
                wallDirection = Vector3.Cross(Vector3.up, leftWallHit.normal);
            }
            else
            {
                wallDirection = Vector3.Cross(rightWallHit.normal, Vector3.up);
            }

            if (Vector3.Dot(groundVelocity, wallDirection) < 0)
            {
                wallDirection = -wallDirection;
            }

            desiredDirection = wallDirection;
            desiredDirection.Normalize();
        }

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

        // Use normal gravity by default.
        float currentGravity = gravity;

        // Reduce gravity while wall running.
        if (bIsWallRunning)
        {
            currentGravity *= wallRunGravityMultiplier;
        }

        // Apply gravity.
        verticalVelocity += currentGravity * Time.deltaTime;

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

        //================
        // Wall Detection
        //================
        
        // Fire a raycast to the right and left of the player and store if it collides with a wall.
        bWallOnRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallCheckDistance, wallLayer);
        bWallOnLeft= Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallCheckDistance, wallLayer);
        
        //==============
        // Wall Running
        //==============
        
        if (!controller.isGrounded && (bWallOnLeft || bWallOnRight))
        {
            bIsWallRunning = true;
        }
        else
        {
            bIsWallRunning = false;
        }

        if (bIsWallRunning)
        {
            Debug.Log("Wall Running");
        }

        // Move the player.
        controller.Move(movement * Time.deltaTime);
    }
    
    // Not used yet. Only here in case bug occurs later with just the layer check.
    private bool isWallRunnable(RaycastHit wallHit)
    {
        return Mathf.Abs(wallHit.normal.y) < 0.2f;
    }
}
