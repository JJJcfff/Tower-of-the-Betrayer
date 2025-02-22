using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    
    private Vector2 movementValue;
    private float lookValue;
    private Rigidbody rb;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        rb = GetComponent<Rigidbody>();
    }
    
    public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>() * speed;
    }
    

    void FixedUpdate()
    {
        // Convert the 2D input into a 3D vector in world space
        Vector3 movement = new Vector3(movementValue.x, 0, movementValue.y);
        
        // Apply the force in world space instead of relative space
        rb.AddForce(
            movement.x * Time.deltaTime,
            0,
            movement.z * Time.deltaTime,
            ForceMode.Force);
    }
}