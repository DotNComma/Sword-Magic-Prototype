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

    [Header("Coyote Timing")]
    private float coyoteTime = 0.2f;
    private float coyoteTimer;

    [Header("Dash Settings")]
    private float dashForce = 25f;
    private float dashDuration = 0.5f;
    private bool isDashing;

    [Header("Air Dash Settings")]
    private float airDashForce = 30f;
    private float airDashDuration = 0.3f;
    private bool hasAirDashed = true;
    private bool isAirDashing;

    [Header("Camera")]
    public Transform cameraTransform;
    public CameraManager cameraManager;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        gravity = GetComponent<Gravity>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Roll/Dash");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    private void Update()
    {
        if (isDashing) return;

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float currentSpeed = sprintAction.IsPressed() ? sprintForce : walkForce;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 move = (cameraForward * moveValue.y + cameraRight * moveValue.x) * currentSpeed;
        Vector3 gravityMovement = gravity.GetGravityMovement();

        controller.Move((move + gravityMovement) * Time.deltaTime);

        if (cameraManager.isFollowMode)
        {
            transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        }
        else if (move.magnitude > 0)
        {
            transform.forward = Vector3.Lerp(transform.forward, move, Time.deltaTime * 10f);
        }

        if (jumpAction.WasPressedThisFrame() && coyoteTimer > 0)
        {
            gravity.setVerticalVelocity(jumpForce);
            coyoteTimer = 0;
        }

        if (gravity.isGrounded)
        {
            hasAirDashed = false;
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (dashAction.WasPressedThisFrame())
        {
            if(gravity.isGrounded)
            {
                Vector3 dashDirection = move.magnitude > 0 ? move.normalized : transform.forward;
                StartCoroutine(PerformDash(dashDirection));
            }
            else if (!hasAirDashed)
            {
                StartCoroutine(PerformAirDash());
            }
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

    private IEnumerator PerformAirDash()
    {
        isAirDashing = true;
        hasAirDashed = true;

        Vector3 dashDirection = cameraTransform.forward;
        gravity.setVerticalVelocity(0);

        float startTime = Time.time;
        while (Time.time < startTime + airDashDuration)
        {
            controller.Move(dashDirection * airDashForce * Time.deltaTime);
            gravity.setVerticalVelocity(0);
            yield return null;
        }

        isAirDashing = false;
    }
}
