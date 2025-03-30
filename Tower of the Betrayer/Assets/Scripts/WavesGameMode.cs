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
        
        // Complete the floor and return to home screen
        Debug.Log("Returning to home screen after delay");
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true;
        GameManager.Instance.CompleteFloor(true);
    }
}
