// Authors: Jeff Cui, Elaine Zhao

using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the screen transitions and win/lose conditions in the game.
public class WavesGameMode : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerLife;

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
        if (EnemyManager.instance.enemies.Count <= 0 && WaveManager.instance.waves.Count <= 0)
        {
            Debug.Log("Player win. Loading WinScreen ...");
            GameManager.Instance.CompleteFloor(true);
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            Cursor.visible = true;
        }
    }
}
