using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private CharacterController _controller;
    
    [SerializeField] private float _speed;
    private Vector2 _move;

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        movePlayer();
    }

    public void movePlayer()
    {
        _move = _move.normalized;
        Vector3 moveDir = transform.right * _move.x + transform.forward * _move.y;

        _controller.Move(moveDir * _speed * Time.deltaTime);
    }
}
