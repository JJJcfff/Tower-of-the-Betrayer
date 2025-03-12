using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WeaponSelection : MonoBehaviour
{
    public Toggle swordToggle;
    public Toggle staffToggle;
    public Button startGameButton;

    public static bool hasSword = false;
    public static bool hasStaff = false;

    private void Start()
    {
        // Reset weapon selection
        hasSword = false;
        hasStaff = false;
        swordToggle.isOn = false;
        staffToggle.isOn = false;

        // Add Listeners for the Toggles
        swordToggle.onValueChanged.AddListener(delegate { ToggleSword(swordToggle.isOn); });
        staffToggle.onValueChanged.AddListener(delegate { ToggleStaff(staffToggle.isOn); });

        // Add Listener for Start Game Button
        startGameButton.onClick.AddListener(StartGame);
        
        // Initially disable the start button until a weapon is selected
        startGameButton.interactable = false;
    }

    private void ToggleSword(bool isChecked)
    {
        if (isChecked)
        {
            hasSword = true;
            hasStaff = false;
            staffToggle.isOn = false;
        }
        else
        {
            hasSword = false;
        }
        UpdateStartButton();
        Debug.Log("Sword: " + hasSword + ", Staff: " + hasStaff);
    }

    private void ToggleStaff(bool isChecked)
    {
        if (isChecked)
        {
            hasStaff = true;
            hasSword = false;
            swordToggle.isOn = false;
        }
        else
        {
            hasStaff = false;
        }
        UpdateStartButton();
        Debug.Log("Sword: " + hasSword + ", Staff: " + hasStaff);
    }

    private void UpdateStartButton()
    {
        // Enable start button only if exactly one weapon is selected
        startGameButton.interactable = hasSword || hasStaff;
    }

    private void StartGame()
    {
        if (!hasSword && !hasStaff)
        {
            Debug.LogWarning("Cannot start game: No weapon selected!");
            return;
        }
        
        Debug.Log("Starting Game... Sword: " + hasSword + ", Staff: " + hasStaff);
        SceneManager.LoadScene("Game");
    }
}
