using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
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
    public float speed = 12.0f;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Read movement value recieved by player input component
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        //get x and z from move input
        x = moveInput.x;
        z = moveInput.y;

        //move player in direction based on input recieved
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }
}
