using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;  // Speed while sprinting
    public float crouchSpeed = 2.5f;  // Speed while crouching
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    private bool isCrouching = false;
    private bool isSprinting = false;
    private float originalHeight;
    public float crouchHeight = 1f;  // Height when crouching
    public float standingHeight = 2f;  // Normal standing height

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;  // Store the original height for standing
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    // This method processes movement, accounting for crouching and sprinting
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        // Determine current speed based on crouch/sprint status
        float currentSpeed = walkSpeed;
        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }
        else if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        // Move the character based on the speed
        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);

        // Apply gravity
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;  // Reset gravity when grounded
        }

        // Move the character based on vertical velocity
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // Handles jumping logic
    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    // Toggle crouch state
    public void Crouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            controller.height = crouchHeight;  // Adjust height for crouching
        }
        else
        {
            isCrouching = false;
            controller.height = standingHeight;  // Reset to standing height
        }
    }

    // Toggle sprinting state
    public void Sprint()
    {
        isSprinting = !isSprinting;  // Toggle sprinting state
    }
}
