using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    private InputAction switchCameraAction;

    public bool isFollowMode = true;
    public CinemachineCamera followCam;
    public CinemachineCamera freeLookCam;

    private void Start()
    {
        switchCameraAction = InputSystem.actions.FindAction("SwitchCamera");

        if (isFollowMode)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        if (switchCameraAction.WasPressedThisFrame())
        {
            isFollowMode = !isFollowMode;
            
            if(isFollowMode)
            {
                followCam.Priority = 10;
                freeLookCam.Priority = 0;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                followCam.Priority = 0;
                freeLookCam.Priority = 10;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
