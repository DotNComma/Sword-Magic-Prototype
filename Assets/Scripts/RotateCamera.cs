using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    private CinemachineInputAxisController inputController;
    private InputAction rightClick;

    private void Start()
    {
        inputController = GetComponent<CinemachineInputAxisController>();
        rightClick = InputSystem.actions.FindAction("CameraLook");
    }

    private void Update()
    {
        if (rightClick.IsPressed())
        {
            inputController.Controllers[0].Input.Gain = 1f;
            inputController.Controllers[1].Input.Gain = -1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            inputController.Controllers[0].Input.Gain = 0f;
            inputController.Controllers[1].Input.Gain = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
