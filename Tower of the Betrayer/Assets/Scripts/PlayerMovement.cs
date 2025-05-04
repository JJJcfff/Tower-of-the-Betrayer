// Authors: Jeff Cui, Elaine Zhao
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    public float baseSpeed;
    public bool speedBoosted = false;

    private Vector2 movementValue;
    private PlayerShooting playerShooting;

    [SerializeField] private Animator animator;

    private List<SpeedBoost> activeSpeedBoosts = new List<SpeedBoost>();

    private class SpeedBoost
    {
        public float amount;
        public Coroutine coroutine;
        public float endTime;

        public SpeedBoost(float amount, Coroutine coroutine, float duration)
        {
            this.amount = amount;
            this.coroutine = coroutine;
            this.endTime = Time.time + duration;
        }

        public float GetRemainingTime()
        {
            return Mathf.Max(0, endTime - Time.time);
        }
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerShooting = GetComponent<PlayerShooting>();

        if (animator == null)
        {
            Debug.LogError("❌ Animator is not assigned in the Inspector.");
        }
        else
        {
            Debug.Log("✅ Animator assigned: " + animator.gameObject.name);
        }

        if (PlayerStats.Instance != null)
        {
            speed = PlayerStats.Instance.Speed;
            baseSpeed = speed;
        }
        else
        {
            baseSpeed = speed;
        }

        if (FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ModifyPlayerSpeed(this);
        }
    }

    public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>();
        Debug.Log("⬅️ Move input: " + movementValue);
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(movementValue.x, 0, movementValue.y).normalized;
        Vector3 movement = moveDirection * speed * Time.fixedDeltaTime;

        Vector3 newPosition = transform.position + movement;
        newPosition.x = Mathf.Clamp(newPosition.x, -19.3f, 19.3f);
        newPosition.z = Mathf.Clamp(newPosition.z, -19.3f, 19.3f);

        transform.position = newPosition;

        // ✅ Set animation state
        if (animator != null)
        {
            bool isActuallyMoving = movementValue.magnitude > 0.01f;

            // Debug state transition
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"🎞 Animator state: {state.shortNameHash}, isMoving = {isActuallyMoving}");

            animator.SetBool("isMoving", isActuallyMoving);
        }

        if (playerShooting != null)
        {
            playerShooting.SetMovementDirection(moveDirection);
        }
    }

    public void ApplySpeedBoost(float boostAmount, float duration)
    {
        Coroutine boostCoroutine = StartCoroutine(HandleSpeedBoost(boostAmount, duration));
        activeSpeedBoosts.Add(new SpeedBoost(boostAmount, boostCoroutine, duration));
        speed += boostAmount;
        speedBoosted = true;
    }

    private IEnumerator HandleSpeedBoost(float boostAmount, float duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveSpeedBoost(boostAmount);
    }

    private void RemoveSpeedBoost(float boostAmount)
    {
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
            speed -= boostToRemove.amount;
            activeSpeedBoosts.Remove(boostToRemove);
            speedBoosted = activeSpeedBoosts.Count > 0;
        }
    }

    public int GetActiveBoostCount() => activeSpeedBoosts.Count;

    public float GetRemainingBoostTime()
    {
        float longestTime = 0f;
        foreach (var boost in activeSpeedBoosts)
        {
            longestTime = Mathf.Max(longestTime, boost.GetRemainingTime());
        }
        return longestTime;
    }
}
