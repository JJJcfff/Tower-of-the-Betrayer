// Authors: Jeff Cui, Elaine Zhao

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Manages the screen transitions and win/lose conditions in the game.
public class WavesGameMode : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerLife;
    private bool isCompletingLevel = false;

    void Start()
    {
        EnemyManager.instance.onChanged.AddListener(CheckWinCondition);
        WaveManager.instance.onChanged.AddListener(CheckWinCondition);
        
        if (playerLife != null)
        {
            playerLife.onDeath.AddListener(HandlePlayerDeath);
        }
    }

    void Update()
    {
        if (playerLife != null)
        {
            if (playerLife.currentHealth <= 0)
            {
                HandlePlayerDeath();
            }
        }
        else
        {
            Debug.LogWarning("Player object is null. This is expected after death.");
        }
    }

    void HandlePlayerDeath()
    {
        Debug.Log("Player died. Loading LoseScreen ...");
        GameManager.Instance.CompleteFloor(false);
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true;
    }

    void CheckWinCondition()
    {
        if (EnemyManager.instance.enemies.Count <= 0 && WaveManager.instance.waves.Count <= 0 && !isCompletingLevel)
        {
            isCompletingLevel = true;
            Debug.Log("Player win. Waiting for UI update before returning to home...");
            StartCoroutine(CompleteLevel());
        }
    }
    
    IEnumerator CompleteLevel()
    {
        // Wait for 1 second to allow UI to update
        yield return new WaitForSeconds(1f);
        
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Check if we were in a boss floor
        bool wasBossFloor = GameManager.Instance.IsNextFloorBoss();
        bool isEndlessMode = GameManager.Instance.IsEndlessModeEnabled();
        
        Debug.Log($"[Level Completion] Floor: {GameManager.Instance.currentFloor}, Boss Floor: {wasBossFloor}, Endless Mode: {isEndlessMode}");
        
        // Check if this is a boss floor - use the same logic as GameSceneInitializer
        if (wasBossFloor && !isEndlessMode)
        {
            // Only show win screen if not in endless mode
            Debug.Log("Boss defeated! Loading WinScreen...");
            SceneManager.LoadScene("WinScreen");
        }
        else
        {
            // Complete the floor and return to home screen for normal floors
            Debug.Log("Returning to home screen after delay");
            GameManager.Instance.CompleteFloor(true);
        }
    }
}
