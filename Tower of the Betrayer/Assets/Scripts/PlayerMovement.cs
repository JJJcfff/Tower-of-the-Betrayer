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
        Vector3 movement = new Vector3(movementValue.x, 0, movementValue.y);
        rb.AddForce(
            movement.x * Time.deltaTime,
            0,
            movement.z * Time.deltaTime,
            ForceMode.Force);
    }
}