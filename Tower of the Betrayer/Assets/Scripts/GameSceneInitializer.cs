// Authors: Jeff Cui, Elaine Zhao
// Initializes the game scene, including floor difficulty

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Ensure GameManager exists
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found! Scene initialization will fail.");
            return;
        }
        
        // Apply floor difficulty settings that were generated in the home scene
        GameManager.Instance.ApplyFloorDifficulty();
        
        // Log the current floor
        Debug.Log($"Initialized Game Scene for Floor {GameManager.Instance.currentFloor}");
    }
} 