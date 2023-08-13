using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class MouseLook : MonoBehaviour
{

    public static float mouseSensitivityX;
    public static float mouseSensitivityY;
    [SerializeField] private Transform playerBody;
    
    private Vector2 look;
    private float xRotation = 0f;


    private void Awake()
    {
        playerBody = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;

        if (mouseSensitivityX == 0) {
            mouseSensitivityX = 3;
        }
        if (mouseSensitivityY == 0) {
            mouseSensitivityY = 3;
        }
    }

    private void Update()
    {
        playerRotate();
    }

    private void playerRotate()
    {
        float lookX = look.x * mouseSensitivityX * Time.deltaTime;
        float lookY = look.y * mouseSensitivityY * Time.deltaTime;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * lookX);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    
}
