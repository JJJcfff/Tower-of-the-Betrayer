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
    }

    public void ShowPotionsUI()
    {
        Debug.Log("Showing Potions UI");

        if (leftPanel != null) leftPanel.SetActive(true);
        if (potionsUI != null) potionsUI.SetActive(true);
        if (weaponUI != null) weaponUI.SetActive(false);

        if (closeButton != null) closeButton.SetActive(true);
    }

    public void CloseAllUI()
    {
        Debug.Log("Closing Weapon & Potion UI");

        if (weaponUI != null) weaponUI.SetActive(false);
        if (potionsUI != null) potionsUI.SetActive(false);
        if (closeButton != null) closeButton.SetActive(false);

        // Optionally, hide Left panel too if both are closed
        if (leftPanel != null) leftPanel.SetActive(false);
    }
}
