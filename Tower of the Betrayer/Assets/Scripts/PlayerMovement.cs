// Authors: Jeff Cui, Elaine Zhao

using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Manages player movement and rotation based on user input.
public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    
    private Vector2 movementValue;
    private PlayerShooting playerShooting;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerShooting = GetComponent<PlayerShooting>();
    }
    
    public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>();
    }
    
    void FixedUpdate()
    {
        // Get normalized input direction in world space
        Vector3 moveDirection = new Vector3(movementValue.x, 0, movementValue.y).normalized;
        
        // Calculate movement delta
        Vector3 movement = moveDirection * speed * Time.fixedDeltaTime;
        
        // Move the transform directly in world space
        transform.position += movement;

        // Send movement direction to PlayerShooting
        if (playerShooting != null)
        {
            playerShooting.SetMovementDirection(moveDirection);
        }
    }
}