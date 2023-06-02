using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{

    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _range = 100f;
    
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit hit;
            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(startPosition, transform.forward, out hit, _range))
            {
                Debug.Log(hit.transform.name);
            }
        }
        
    }
}
