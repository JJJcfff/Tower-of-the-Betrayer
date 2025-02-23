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
    }

    void Update()
    {
        if (playerLife != null)
        {
            if (playerLife.currentHealth <= 0)
            {
                Debug.Log("Player died. Loading LoseScreen ...");
                SceneManager.LoadScene("LoseScreen");
            }
        }
        else
        {
            Debug.Log("Player object is null. Loading LoseScreen.");
            SceneManager.LoadScene("LoseScreen");
        }
    }

    void CheckWinCondition()
    {
        if (EnemyManager.instance.enemies.Count <= 0 && WaveManager.instance.waves.Count <= 0)
        {
            Debug.Log("Player win. Loading WinScreen ...");
            SceneManager.LoadScene("WinScreen");
        }
    }
}
