using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input System")]
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction sprintAction;

    [Header("Components")]
    private CharacterController controller;
    private Gravity gravity;

    [Header("Movement Values")]
    private float walkForce = 20f;
    private float sprintForce = 40f;
    private float jumpForce = 10f;
    private float rollForce = 20f;

    [Header("Coyote Timing")]
    private float coyoteTime = 0.2f;
    private float coyoteTimer;

    [Header("Dash Settings")]
    private float dashForce = 25f;
    private float dashDuration = 0.5f;
    private bool isDashing;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        gravity = GetComponent<Gravity>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Roll/Dash");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    void Update()
    {
        if (isDashing) return;

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        float currentSpeed = sprintAction.IsPressed() ? sprintForce : walkForce;
        Vector3 move = new Vector3(moveValue.x, 0, moveValue.y) * currentSpeed;
        Vector3 gravityMovement = gravity.GetGravityMovement();

        controller.Move((move + gravityMovement) * Time.deltaTime);


        if (jumpAction.WasPressedThisFrame() && coyoteTimer > 0)
        {
            gravity.setVerticalVelocity(jumpForce);
            coyoteTimer = 0;
        }

        if (gravity.isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (dashAction.WasPressedThisFrame() && gravity.isGrounded)
        {
            Vector3 dashDirection = move.magnitude > 0 ? move.normalized : transform.forward;
            StartCoroutine(PerformDash(dashDirection));
        }
    }

    private IEnumerator PerformDash(Vector3 direction)
    {
        isDashing = true;
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            controller.Move(direction * dashForce * Time.deltaTime);
            yield return null;
        }
        isDashing = false;
    }
}
