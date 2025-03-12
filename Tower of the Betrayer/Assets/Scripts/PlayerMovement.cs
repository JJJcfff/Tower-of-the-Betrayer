// Authors: Jeff Cui, Elaine Zhao

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Manages player movement and rotation based on user input.
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    
    private Vector2 movementValue;
    private float lookValue;
    private Rigidbody rb;
    private PlayerShooting playerShooting; // Reference to PlayerShooting component

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        rb = GetComponent<Rigidbody>();
        playerShooting = GetComponent<PlayerShooting>(); // Get PlayerShooting component
    }
    
    public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>() * speed;
    }
    

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementValue.x, 0, movementValue.y);
        rb.AddForce(
            movement.x * Time.deltaTime,
            0,
            movement.z * Time.deltaTime,
            ForceMode.Force);

        // Send movement direction to PlayerShooting
        if (playerShooting != null)
        {
            playerShooting.SetMovementDirection(movement);
        }
    }
}