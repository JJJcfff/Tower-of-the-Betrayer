using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WeaponSelection : MonoBehaviour
{
    public Toggle swordToggle;
    public Toggle staffToggle;
    public Button startGameButton;

    public static bool hasSword = true;
    public static bool hasStaff = true;

    private void Start()
    {
        // Ensure default values
        hasSword = true;
        hasStaff = true;

        // Add Listeners for the Toggles
        swordToggle.onValueChanged.AddListener(delegate { ToggleSword(swordToggle.isOn); });
        staffToggle.onValueChanged.AddListener(delegate { ToggleStaff(staffToggle.isOn); });

        // Add Listener for Start Game Button
        startGameButton.onClick.AddListener(StartGame);
    }

    private void ToggleSword(bool isChecked)
    {
        hasSword = isChecked;
        Debug.Log("Sword: " + hasSword + ", Staff: " + hasStaff);
    }

    private void ToggleStaff(bool isChecked)
    {
        hasStaff = isChecked;
        Debug.Log("Sword: " + hasSword + ", Staff: " + hasStaff);
    }

    private void StartGame()
    {
        Debug.Log("Starting Game... Sword: " + hasSword + ", Staff: " + hasStaff);
        SceneManager.LoadScene("Game");
    }
}
