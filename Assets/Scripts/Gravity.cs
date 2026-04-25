using UnityEngine;

public class Gravity : MonoBehaviour
{
    [Header("Gravity Settings")]
    private float defaultGravity = 20f;
    private float fallMultiplier = 3f;

    private CharacterController controller;
    private float verticalVelocity;

    public bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        isGrounded = controller.isGrounded;
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public Vector3 GetGravityMovement()
    {
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            float currentGravity = (verticalVelocity < 0) ? defaultGravity * fallMultiplier : defaultGravity;
            verticalVelocity -= currentGravity * Time.deltaTime;
        }

        return new Vector3(0, verticalVelocity, 0);
    }

    public void setVerticalVelocity(float force)
    {
        verticalVelocity = force;
    }
}
