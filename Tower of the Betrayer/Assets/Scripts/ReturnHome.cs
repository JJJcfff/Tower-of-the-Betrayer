// Authors: Jeff Cui, Elaine Zhao
// Handles the button click event to return to the main menu scene, resetting inventory and stats.

using UnityEngine;
using UnityEngine.SceneManagement;
using Inventory;

public class ReturnToMainButton : MonoBehaviour
{
    public void LoadMainScene()
    {
        // Reset inventory before returning to main menu
        ResetInventory();
        
        // Return to main menu
        GameManager.Instance.LoadMainScene();
    }
    
    private void ResetInventory()
    {
        // Reset inventory contents
        if (InventoryManager.Instance != null)
        {
            // Use the public method to reset inventory
            InventoryManager.Instance.ResetInventory();
            
            // Reset player stats if needed
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.ResetToDefault();
            }
            
            Debug.Log("Inventory and stats reset before returning to main menu");
        }
    }
}
