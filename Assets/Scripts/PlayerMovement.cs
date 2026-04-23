using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input System")]
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction sprintAction;

    [Header("Player Controls")]
    private Rigidbody rb;
    private Collider playerCollider;
    private float playerHeight;

    [Header("Movement Values")]
    private float jumpForce = 10f;
    private float rollForce = 300f;
    private float gravityForce = 20f;
    private float moveForce;

    [Header("Ground Check")]
    private bool isGrounded;
    private LayerMask groundMask;

    [Header("Coyote Timing")]
    private float coyoteTime = 0.2f;
    private float coyoteTimer;

    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        playerHeight = playerCollider.bounds.size.y;
        
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Roll/Dash");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundMask);

        if (sprintAction.IsPressed())
        {
            moveForce = 2000f;
        }
        else 
        {
            moveForce = 1000f;
        }

        rb.linearVelocity = new Vector3((moveValue.x * moveForce) * Time.deltaTime, rb.linearVelocity.y, (moveValue.y * moveForce) * Time.deltaTime);

        if (jumpAction.WasPressedThisFrame() && 
            coyoteTimer > 0)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
            coyoteTimer = 0;
        }

        if (dashAction.WasPressedThisFrame() && isGrounded)
        {
            rb.AddForce(moveValue * rollForce, ForceMode.Impulse);
        }

        if (!isGrounded)
        {
            rb.AddForce(-transform.up * gravityForce, ForceMode.Force);
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = coyoteTime;
        }
    }
}
