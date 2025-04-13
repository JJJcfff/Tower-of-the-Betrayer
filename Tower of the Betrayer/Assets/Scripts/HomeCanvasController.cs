// Authors: Jeff Cui, Elaine Zhao
// Controls the initial state and visibility of UI elements on the home canvas.

using UnityEngine;

public class HomeCanvasController : MonoBehaviour
{
    private GameObject weaponUI;
    private GameObject potionsUI;

    void Awake()
    {
        Transform panel = transform.Find("Panel");
        if (panel == null)
        {
            Debug.LogWarning("Panel not found under HomeCanvas!");
            return;
        }

        // Deactivate all top-level children of Panel except 'Right'
        foreach (Transform child in panel)
        {
            if (child.name != "Right")
            {
                child.gameObject.SetActive(false);
            }
        }

        // Save references for later use (even though they're inactive now)
        weaponUI = panel.Find("Left/Weapon")?.gameObject;
        potionsUI = panel.Find("Left/Potions")?.gameObject;

        Debug.Log("Weapon UI found: " + (weaponUI != null));
        Debug.Log("Potions UI found: " + (potionsUI != null));
    }

    public void ShowWeaponUI()
    {
        Debug.Log("Showing Weapon UI");
        if (weaponUI != null) weaponUI.SetActive(true);
    }

    public void ShowPotionsUI()
    {
        Debug.Log("Showing Potions UI");
        if (potionsUI != null) potionsUI.SetActive(true);
    }
}
