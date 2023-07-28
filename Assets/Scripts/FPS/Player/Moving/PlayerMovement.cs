using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private CharacterController _controller;
    
    [SerializeField] private float _speed;
    public float _gravity;
    [SerializeField] private float _jumpHeight;

    public Vector3 _velocity;
    private Vector2 _move;
    private bool isGrounded, isJumping;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance;
    [SerializeField] private LayerMask _groundMask;
    

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartJump();
        }

        if (context.canceled)
        {
            EndJump();
        }
    }

    private void StartJump()
    {
        isJumping = true;
    }
    
    private void EndJump()
    {
        isJumping = false;
    }
    
    private void Update()
    {
        movePlayer();
        if ( isGrounded && isJumping)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }
    }

    private void movePlayer()
    {
        isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        
        _move = _move.normalized;
        Vector3 moveDir = transform.right * _move.x + transform.forward * _move.y;

        _controller.Move(moveDir * _speed * Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
    }
}
