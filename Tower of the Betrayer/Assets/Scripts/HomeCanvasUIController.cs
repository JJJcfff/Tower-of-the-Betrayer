// Authors: Jeff Cui, Elaine Zhao
// Manages the UI panels (Weapon, Potions) on the home scene canvas.

using UnityEngine;
using UnityEngine.UI;

public class HomeCanvasUIController : MonoBehaviour
{
    private GameObject leftPanel;
    private GameObject weaponUI;
    private GameObject potionsUI;
    private GameObject closeButton;

    private bool weaponUIOpen = false;
    private bool potionsUIOpen = false;

    // Reference to the movement script to send the player back to neutral
    public SmoothMoverWithUITrigger movementController;

    void Awake()
    {
        Transform panel = transform.Find("Panel");
        if (panel == null)
        {
            Debug.LogWarning("Panel not found under HomeCanvas!");
            return;
        }

        // Get references
        leftPanel = panel.Find("Left")?.gameObject;
        weaponUI = panel.Find("Left/Weapon")?.gameObject;
        potionsUI = panel.Find("Left/Potions")?.gameObject;
        closeButton = transform.Find("Close")?.gameObject;

        // Hide all except Right
        foreach (Transform child in panel)
        {
            if (child.name != "Right")
                child.gameObject.SetActive(false);
        }

        if (closeButton != null)
        {
            closeButton.SetActive(false); // hide at start
            Button btn = closeButton.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(CloseAllUI);
        }

        Debug.Log("Weapon UI found: " + (weaponUI != null));
        Debug.Log("Potions UI found: " + (potionsUI != null));
        Debug.Log("Close Button found: " + (closeButton != null));
    }

    public void ShowWeaponUI()
    {
        Debug.Log("Showing Weapon UI");

        if (leftPanel != null) leftPanel.SetActive(true);
        if (weaponUI != null) weaponUI.SetActive(true);
        if (potionsUI != null) potionsUI.SetActive(false);
        if (closeButton != null) closeButton.SetActive(true);

        weaponUIOpen = true;
        potionsUIOpen = false;
    }

    public void ShowPotionsUI()
    {
        Debug.Log("Showing Potions UI");

        if (leftPanel != null) leftPanel.SetActive(true);
        if (potionsUI != null) potionsUI.SetActive(true);
        if (weaponUI != null) weaponUI.SetActive(false);
        if (closeButton != null) closeButton.SetActive(true);

        potionsUIOpen = true;
        weaponUIOpen = false;
    }

    public void CloseAllUI()
    {
        Debug.Log("Closing Weapon & Potion UI");

        if (weaponUI != null) weaponUI.SetActive(false);
        if (potionsUI != null) potionsUI.SetActive(false);
        if (closeButton != null) closeButton.SetActive(false);
        if (leftPanel != null) leftPanel.SetActive(false);

        weaponUIOpen = false;
        potionsUIOpen = false;

        // Send player back to neutral
        if (movementController != null)
        {
            movementController.GoToNeutral();
        }
    }

    public bool IsWeaponUIOpen() => weaponUIOpen;
    public bool IsPotionsUIOpen() => potionsUIOpen;
}
