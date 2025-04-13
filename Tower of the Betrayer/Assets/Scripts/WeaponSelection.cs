// Authors: Jeff Cui, Elaine Zhao
// Handles the weapon selection UI in the home scene, updating GameManager state and enabling the start button.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WeaponSelection : MonoBehaviour
{
    public Toggle swordToggle;
    public Toggle staffToggle;
    public Button startGameButton;

    private void Start()
    {
        // Use GameManager state instead of static variables
        swordToggle.isOn = GameManager.Instance.hasSword;
        staffToggle.isOn = GameManager.Instance.hasStaff;
        
        if (swordToggle != null)
        {
            swordToggle.onValueChanged.AddListener(ToggleSword);
        }
        
        if (staffToggle != null)
        {
            staffToggle.onValueChanged.AddListener(ToggleStaff);
        }
        
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(StartGame);
            UpdateStartButton();
        }
    }

    private void ToggleSword(bool isChecked)
    {
        if (isChecked)
        {
            GameManager.Instance.hasSword = true;
            GameManager.Instance.hasStaff = false;
            staffToggle.isOn = false;
        }
        else
        {
            GameManager.Instance.hasSword = false;
        }
        UpdateStartButton();
    }

    private void ToggleStaff(bool isChecked)
    {
        if (isChecked)
        {
            GameManager.Instance.hasStaff = true;
            GameManager.Instance.hasSword = false;
            swordToggle.isOn = false;
        }
        else
        {
            GameManager.Instance.hasStaff = false;
        }
        UpdateStartButton();
    }

    private void UpdateStartButton()
    {
        if (startGameButton != null)
        {
            startGameButton.interactable = GameManager.Instance.hasSword || GameManager.Instance.hasStaff;
        }
    }

    private void StartGame()
    {
        if (!GameManager.Instance.hasSword && !GameManager.Instance.hasStaff)
        {
            Debug.LogWarning("Cannot start game: No weapon selected!");
            return;
        }
        
        Debug.Log("Starting Game... Sword: " + GameManager.Instance.hasSword + ", Staff: " + GameManager.Instance.hasStaff);
        SceneManager.LoadScene("Game");
    }
}
