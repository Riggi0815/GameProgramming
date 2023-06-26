using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{

    private Vector2 look;

    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float multiplier;

    // Update is called once per frame
    void Update()
    {
        //mouse Input
        float lookX = look.x * multiplier;
        float lookY = look.y * multiplier;

        //calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-lookY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(lookX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;
        
        //rotate
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
}
