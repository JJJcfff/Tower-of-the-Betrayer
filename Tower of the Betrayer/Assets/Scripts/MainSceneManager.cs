// Authors: Jeff Cui, Elaine Zhao
// Manages the main menu scene, initializes managers, and handles the start game button.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Inventory;

public class MainSceneManager : MonoBehaviour
{
    public Button startGameButton;
    
    void Start()
    {
        InitializeManagers();
        SetupStartButton();
    }

    private void InitializeManagers()
    {
        // Ensure we have the required managers
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager not found in Main scene!");
            return;
        }
        if (PlayerStats.Instance == null)
        {
            Debug.LogError("PlayerStats not found in Main scene!");
            return;
        }
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found in Main scene!");
            return;
        }

        // Start with no resources (players must earn everything)
        InventoryManager.Instance.ResetInventory();

        Debug.Log("All managers initialized successfully!");
    }

    private void SetupStartButton()
    {
        if (startGameButton != null)
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(StartGame);
            Debug.Log("Start button initialized successfully!");
        }
        else
        {
            Debug.LogError("StartGameButton is not assigned in the Inspector!");
        }
    }

    public void StartGame()
    {
        Debug.Log("Starting new game...");
        GameManager.Instance.StartNewGame();
        SceneManager.LoadScene("Home");
    }
}