// Authors: Jeff Cui, Elaine Zhao

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Manages player movement and rotation based on user input.
public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    public float baseSpeed;
    public bool speedBoosted = false;
    
    private Vector2 movementValue;
    private PlayerShooting playerShooting;
    
    // Track active speed boosts
    private List<SpeedBoost> activeSpeedBoosts = new List<SpeedBoost>();
    
    // Class to track individual speed boosts
    private class SpeedBoost
    {
        public float amount;
        public Coroutine coroutine;
        
        public SpeedBoost(float amount, Coroutine coroutine)
        {
            this.amount = amount;
            this.coroutine = coroutine;
        }
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerShooting = GetComponent<PlayerShooting>();
        
        // Initialize speed from PlayerStats if available
        if (PlayerStats.Instance != null)
        {
            speed = PlayerStats.Instance.Speed;
            baseSpeed = speed;
        }
        else
        {
            baseSpeed = speed;
        }
        
        // Apply floor difficulty modifiers to speed
        if (FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ModifyPlayerSpeed(this);
        }
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
    
    // Apply a temporary speed boost that stacks with other boosts
    public void ApplySpeedBoost(float boostAmount, float duration)
    {
        // Start coroutine to track this specific boost
        Coroutine boostCoroutine = StartCoroutine(HandleSpeedBoost(boostAmount, duration));
        
        // Add to active boosts list
        activeSpeedBoosts.Add(new SpeedBoost(boostAmount, boostCoroutine));
        
        // Apply the boost immediately
        speed += boostAmount;
        speedBoosted = true;
    }
    
    private IEnumerator HandleSpeedBoost(float boostAmount, float duration)
    {
        yield return new WaitForSeconds(duration);
        
        // Remove this specific boost
        RemoveSpeedBoost(boostAmount);
    }
    
    private void RemoveSpeedBoost(float boostAmount)
    {
        // Find the boost in the list
        SpeedBoost boostToRemove = null;
        foreach (var boost in activeSpeedBoosts)
        {
            if (Math.Abs(boost.amount - boostAmount) < 0.001f)
            {
                boostToRemove = boost;
                break;
            }
        }
        
        if (boostToRemove != null)
        {
            // Remove the boost amount from speed
            speed -= boostToRemove.amount;
            activeSpeedBoosts.Remove(boostToRemove);
            
            // Update speedBoosted flag
            speedBoosted = activeSpeedBoosts.Count > 0;
        }
    }
    
    // For debugging purposes
    public int GetActiveBoostCount()
    {
        return activeSpeedBoosts.Count;
    }
}