// Authors: Jeff Cui, Elaine Zhao
// Manages all UI elements in the Home scene, including resource display, weapon stats/upgrades, potion crafting, and game start options.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Inventory;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HomeSceneUI : MonoBehaviour
{
    [Header("Resource Display")]
    public TextMeshProUGUI gemDustText;
    public TextMeshProUGUI mushroomText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI speedText;

    [Header("Game Progress")]
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI modifiersHintText;

    [Header("Weapon Stats - Sword")]
    public TextMeshProUGUI swordDamageText;
    public TextMeshProUGUI swordSpeedText;
    public TextMeshProUGUI swordRangeText;
    public Button[] swordUpgradeButtons;
    public Button swordRefundButton;
    public TextMeshProUGUI swordRefundText;
    public TextMeshProUGUI[] swordUpgradeCostTexts;

    [Header("Weapon Stats - Staff")]
    public TextMeshProUGUI staffDamageText;
    public TextMeshProUGUI staffSpeedText;
    public TextMeshProUGUI staffRangeText;
    public Button[] staffUpgradeButtons;
    public Button staffRefundButton;
    public TextMeshProUGUI staffRefundText;
    public TextMeshProUGUI[] staffUpgradeCostTexts;

    [Header("Potion Crafting")]
    public Button[] permanentPotionButtons;
    public Button[] temporaryPotionButtons;
    public TextMeshProUGUI[] temporaryPotionCountText;

    [Header("Game Start")]
    public Button startBattleButton;
    public Toggle endlessModeToggle;
    public Toggle equipSwordToggle;
    public Toggle equipStaffToggle;

    private InventoryManager inventory;
    private PlayerStats playerStats;

    private void Start()
    {
        // Get manager instances - they should exist from the Main scene
        inventory = InventoryManager.Instance;
        playerStats = PlayerStats.Instance;

        // If managers don't exist, we might have started from the wrong scene
        if (inventory == null || playerStats == null)
        {
            Debug.LogError("Required managers not found! Please ensure you started from the Main scene.");
            return;
        }

        // Verify UI components
        if (!VerifyUIComponents()) 
        {
            Debug.LogError("UI components verification failed. Please check the Inspector for missing assignments.");
            return;
        }
        
        // Generate modifiers for the next floor if they haven't been generated yet
        if (FloorDifficultyManager.Instance != null && !FloorDifficultyManager.Instance.AreModifiersGenerated())
        {
            FloorDifficultyManager.Instance.GenerateFloorModifiers();
        }

        // Initialize UI
        UpdateResourceDisplay();
        UpdateWeaponStats();
        UpdatePotionDisplay();
        UpdateEndlessModeUI();
        UpdateFloorDisplay();

        // Add listeners
        SetupButtonListeners();

        // Set initial toggle states based on previous selection
        string lastWeapon = PlayerPrefs.GetString("SelectedWeapon", "Sword");
        equipSwordToggle.isOn = lastWeapon == "Sword";
        equipStaffToggle.isOn = lastWeapon == "Staff";

        // Setup weapon toggle group behavior
        equipSwordToggle.onValueChanged.AddListener(OnSwordToggleChanged);
        equipStaffToggle.onValueChanged.AddListener(OnStaffToggleChanged);
        endlessModeToggle.onValueChanged.AddListener(OnEndlessModeChanged);

        // Disable start button if no weapon selected
        UpdateStartButtonState();
    }

    private bool VerifyUIComponents()
    {
        bool isValid = true;

        // Check resource display
        if (gemDustText == null) { Debug.LogError("Gem Dust Text not assigned!"); isValid = false; }
        if (mushroomText == null) { Debug.LogError("Mushroom Text not assigned!"); isValid = false; }
        if (maxHealthText == null) { Debug.LogError("Max Health Text not assigned!"); isValid = false; }
        if (speedText == null) { Debug.LogError("Speed Text not assigned!"); isValid = false; }
        if (floorText == null) { Debug.LogError("Floor Text not assigned!"); isValid = false; }
        // modifiersHintText is optional, so no need to verify it

        // Check weapon stats
        if (swordDamageText == null) { Debug.LogError("Sword Damage Text not assigned!"); isValid = false; }
        if (swordSpeedText == null) { Debug.LogError("Sword Speed Text not assigned!"); isValid = false; }
        if (swordRangeText == null) { Debug.LogError("Sword Range Text not assigned!"); isValid = false; }
        if (staffDamageText == null) { Debug.LogError("Staff Damage Text not assigned!"); isValid = false; }
        if (staffSpeedText == null) { Debug.LogError("Staff Speed Text not assigned!"); isValid = false; }
        if (staffRangeText == null) { Debug.LogError("Staff Range Text not assigned!"); isValid = false; }
        
        // Check cost texts
        if (swordUpgradeCostTexts == null || swordUpgradeCostTexts.Length < 3) { 
            Debug.LogError("Sword Upgrade Cost Texts not properly assigned! Need 3 (damage, speed, range)"); 
            isValid = false; 
        }
        if (staffUpgradeCostTexts == null || staffUpgradeCostTexts.Length < 3) { 
            Debug.LogError("Staff Upgrade Cost Texts not properly assigned! Need 3 (damage, speed, range)"); 
            isValid = false; 
        }

        // Check buttons and toggles
        if (swordUpgradeButtons == null || swordUpgradeButtons.Length == 0) { Debug.LogError("Sword Upgrade Buttons not assigned!"); isValid = false; }
        if (staffUpgradeButtons == null || staffUpgradeButtons.Length == 0) { Debug.LogError("Staff Upgrade Buttons not assigned!"); isValid = false; }
        if (permanentPotionButtons == null || permanentPotionButtons.Length == 0) { Debug.LogError("Permanent Potion Buttons not assigned!"); isValid = false; }
        if (temporaryPotionButtons == null || temporaryPotionButtons.Length == 0) { Debug.LogError("Temporary Potion Buttons not assigned!"); isValid = false; }
        if (temporaryPotionCountText == null || temporaryPotionCountText.Length == 0) { Debug.LogError("Temporary Potion Count Texts not assigned!"); isValid = false; }
        if (startBattleButton == null) { Debug.LogError("Start Battle Button not assigned!"); isValid = false; }
        if (endlessModeToggle == null) { Debug.LogError("Endless Mode Toggle not assigned!"); isValid = false; }
        if (equipSwordToggle == null) { Debug.LogError("Equip Sword Toggle not assigned!"); isValid = false; }
        if (equipStaffToggle == null) { Debug.LogError("Equip Staff Toggle not assigned!"); isValid = false; }

        return isValid;
    }

    private void SetupButtonListeners()
    {
        foreach (var button in swordUpgradeButtons)
        {
            if (button != null)
                button.onClick.AddListener(() => OnWeaponUpgrade(WeaponType.Sword, button.name));
        }

        foreach (var button in staffUpgradeButtons)
        {
            if (button != null)
                button.onClick.AddListener(() => OnWeaponUpgrade(WeaponType.Staff, button.name));
        }

        foreach (var button in permanentPotionButtons)
        {
            if (button != null)
                button.onClick.AddListener(() => OnPermanentPotionCraft(button.name));
        }

        foreach (var button in temporaryPotionButtons)
        {
            if (button != null)
                button.onClick.AddListener(() => OnTemporaryPotionCraft(button.name));
        }
        
        if (swordRefundButton != null)
            swordRefundButton.onClick.AddListener(() => OnWeaponRefund(WeaponType.Sword));
            
        if (staffRefundButton != null)
            staffRefundButton.onClick.AddListener(() => OnWeaponRefund(WeaponType.Staff));

        if (startBattleButton != null)
            startBattleButton.onClick.AddListener(OnStartBattle);
    }

    private void Update()
    {
        UpdateResourceDisplay();
        UpdateFloorDisplay();
        UpdateRefundButtons();
        
        // Debug controls for adding resources
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddDebugResource(ResourceType.GemDust, 50);
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddDebugResource(ResourceType.Mushroom, 50);
        }
        
        // Debug control to jump to floor 9 (right before boss floor)
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentFloor = 9;
                Debug.Log("[DEBUG] Jumped to floor 9!");
                UpdateFloorDisplay();
                UpdateEndlessModeUI();
            }
        }
    }

    private void UpdateResourceDisplay()
    {
        if (inventory == null || playerStats == null) return;

        if (gemDustText != null)
            gemDustText.text = $"Gem Dust: {inventory.GetResourceAmount(ResourceType.GemDust)}";
        if (mushroomText != null)
            mushroomText.text = $"Mushrooms: {inventory.GetResourceAmount(ResourceType.Mushroom)}";
        if (maxHealthText != null)
            maxHealthText.text = $"Max Health: {playerStats.MaxHealth}";
        if (speedText != null)
            speedText.text = $"Speed: {playerStats.Speed}";
    }

    private void UpdateWeaponStats()
    {
        // Sword stats
        swordDamageText.text = $"Damage: {playerStats.GetWeaponDamage(WeaponType.Sword):F1}";
        swordSpeedText.text = $"Atk Speed: {playerStats.GetWeaponSpeed(WeaponType.Sword):F1}";
        swordRangeText.text = $"Atk Range: {playerStats.GetWeaponRange(WeaponType.Sword):F1}";

        // Staff stats
        staffDamageText.text = $"Damage: {playerStats.GetWeaponDamage(WeaponType.Staff):F1}";
        staffSpeedText.text = $"Atk Speed: {playerStats.GetWeaponSpeed(WeaponType.Staff):F1}";
        staffRangeText.text = $"Atk Range: {playerStats.GetWeaponRange(WeaponType.Staff):F1}";
        
        // Update upgrade button texts to show the current cost
        UpdateUpgradeButtonTexts();
    }
    
    private void UpdateUpgradeButtonTexts()
    {
        // Update sword upgrade costs
        for (int i = 0; i < swordUpgradeButtons.Length && i < swordUpgradeCostTexts.Length; i++)
        {
            string statType = swordUpgradeButtons[i].name;
            int cost = playerStats.GetUpgradeCost(WeaponType.Sword, statType);
            swordUpgradeCostTexts[i].text = $"Cost: {cost}";
        }
        
        // Update staff upgrade costs
        for (int i = 0; i < staffUpgradeButtons.Length && i < staffUpgradeCostTexts.Length; i++)
        {
            string statType = staffUpgradeButtons[i].name;
            int cost = playerStats.GetUpgradeCost(WeaponType.Staff, statType);
            staffUpgradeCostTexts[i].text = $"Cost: {cost}";
        }
    }

    private void UpdatePotionDisplay()
    {
        for (int i = 0; i < temporaryPotionCountText.Length; i++)
        {
            var potions = inventory.GetAllPotions();
            int count = potions.Count(p => !p.isPermanent && p.type == (PotionType)i);
            temporaryPotionCountText[i].text = $"Owned: {count}";
        }
    }

    private void UpdateEndlessModeUI()
    {
        bool canToggleEndless = GameManager.Instance.CanToggleEndlessMode();
        endlessModeToggle.gameObject.SetActive(canToggleEndless);
        
        if (canToggleEndless)
        {
            endlessModeToggle.isOn = GameManager.Instance.IsEndlessModeEnabled();
            startBattleButton.GetComponentInChildren<TextMeshProUGUI>().text = endlessModeToggle.isOn ? 
                "Continue Endless Mode" :  "Start Boss Fight";
        }
        else
        {
            startBattleButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Battle";
        }
    }

    private void UpdateFloorDisplay()
    {
        if (floorText != null && GameManager.Instance != null)
        {
            int nextFloor = GameManager.Instance.currentFloor; // This is already the next floor to play
            if (nextFloor >= GameManager.BOSS_FLOOR)
            {
                floorText.text = GameManager.Instance.IsEndlessModeEnabled() ?
                    $"Next: Floor {nextFloor} (Endless Mode)" :
                    $"Next: Floor {nextFloor}" + (nextFloor == GameManager.BOSS_FLOOR ? " (Boss Floor!)" : " (Boss Available)");
            }
            else
            {
                floorText.text = $"Next: Floor {nextFloor}";
            }
            
            // Update modifiers hint
            UpdateModifiersHint();
        }
    }
    
    private void UpdateModifiersHint()
    {
        if (modifiersHintText == null || FloorDifficultyManager.Instance == null) return;
        
        List<FloorModifier> modifiers = FloorDifficultyManager.Instance.GetActiveModifiers();
        
        int goodModifiers = 0;
        int badModifiers = 0;
        
        foreach (var modifier in modifiers)
        {
            if (modifier.isGood)
                goodModifiers++;
            else
                badModifiers++;
        }
        
        if (goodModifiers == 0 && badModifiers == 0)
        {
            modifiersHintText.text = "The journey ahead seems ordinary...";
        }
        else
        {
            string blessingText = goodModifiers > 0 ? $"<color=green>{goodModifiers} blessing{(goodModifiers > 1 ? "s" : "")}</color>" : "";
            string curseText = badModifiers > 0 ? $"<color=red>{badModifiers} curse{(badModifiers > 1 ? "s" : "")}</color>" : "";
            
            if (goodModifiers > 0 && badModifiers > 0)
            {
                modifiersHintText.text = $"The next floor holds {blessingText} and {curseText}...";
            }
            else if (goodModifiers > 0)
            {
                string prefix = goodModifiers == 1 ? "a " : "";
                modifiersHintText.text = $"Fortune smiles upon you with {prefix}{blessingText}!";
            }
            else
            {
                string prefix = badModifiers == 1 ? "a " : "";
                modifiersHintText.text = $"Beware, darkness lurks with {prefix}{curseText}...";
            }
        }
    }

    private void UpdateRefundButtons()
    {
        // Update sword refund button
        if (swordRefundButton != null && swordRefundText != null && playerStats != null)
        {
            int gemDustSpent = playerStats.GetTotalGemDustSpent(WeaponType.Sword);
            int refundAmount = Mathf.FloorToInt(gemDustSpent * 0.8f);
            
            swordRefundButton.interactable = (gemDustSpent > 0);
            swordRefundText.text = $"Reset (Refund {refundAmount} Gem Dust)";
        }
        
        // Update staff refund button
        if (staffRefundButton != null && staffRefundText != null && playerStats != null)
        {
            int gemDustSpent = playerStats.GetTotalGemDustSpent(WeaponType.Staff);
            int refundAmount = Mathf.FloorToInt(gemDustSpent * 0.8f);
            
            staffRefundButton.interactable = (gemDustSpent > 0);
            staffRefundText.text = $"Reset (Refund {refundAmount} Gem Dust)";
        }
    }

    private void OnWeaponRefund(WeaponType weaponType)
    {
        if (playerStats != null)
        {
            playerStats.ResetWeaponStats(weaponType);
            UpdateWeaponStats();
            UpdateResourceDisplay();
        }
    }

    private void OnWeaponUpgrade(WeaponType weaponType, string statType)
    {
        int cost = 10; // Base cost for weapon upgrades
        
        switch (statType)
        {
            case "Damage":
                if (playerStats.UpgradeWeaponDamage(weaponType))
                {
                    Debug.Log($"Upgraded {weaponType} damage successfully");
                }
                break;
            case "Speed":
                if (playerStats.UpgradeWeaponSpeed(weaponType))
                {
                    Debug.Log($"Upgraded {weaponType} speed successfully");
                }
                break;
            case "Range":
                if (playerStats.UpgradeWeaponRange(weaponType))
                {
                    Debug.Log($"Upgraded {weaponType} range successfully");
                }
                break;
        }

        UpdateWeaponStats();
        UpdateResourceDisplay();
    }

    private void OnPermanentPotionCraft(string potionName)
    {
        float value;
        int cost;

        // Apply permanent effect directly to player stats
        switch (potionName)
        {
            case "MaxHealth":
                cost = 10; // Cost for max health potion

                if (!inventory.UseResource(ResourceType.Mushroom, cost))
                {
                    Debug.Log($"Not enough Mushrooms for potion! Need {cost}");
                    return;
                }
                value = 10f;
                playerStats.IncreaseMaxHealth(value);
                Debug.Log($"Increased Max Health by {value}");
                break;
            case "Speed":
                cost = 5;

                if (!inventory.UseResource(ResourceType.Mushroom, cost))
                {
                    Debug.Log($"Not enough Mushrooms for potion! Need {cost}");
                    return;
                }
                value = 1f;
                playerStats.IncreaseSpeed(value);
                Debug.Log($"Increased Speed by {value}");
                break;
            default:
                Debug.LogError($"Unknown permanent potion type: {potionName}");
                return;
        }

        // Update UI to reflect changes
        UpdateResourceDisplay();
    }

    private void OnTemporaryPotionCraft(string potionName)
    {
        PotionType type;
        float value;
        int cost;

        switch (potionName)
        {
            case "Health":
                type = PotionType.HealthRestore;
                value = 50f;
                cost = 5; // Cost for temporary health potion
                break;
            case "Speed":
                type = PotionType.SpeedBoost;
                value = 2f;
                cost = 2; // Cost for temporary speed potion
                break;
            default:
                return;
        }

        Debug.Log($"using mushroms for {potionName}");
        if (!inventory.UseResource(ResourceType.Mushroom, cost))
        {
            Debug.Log($"Not enough Mushrooms for potion! Need {cost}");
            return;
        }

        inventory.CreatePotion(type, value, false);
        UpdatePotionDisplay();
    }

    private void OnSwordToggleChanged(bool isOn)
    {
        if (isOn)
        {
            equipStaffToggle.isOn = false;
        }
        else if (!equipStaffToggle.isOn)
        {
            // Don't allow both toggles to be off
            equipSwordToggle.isOn = true;
        }
        UpdateStartButtonState();
    }

    private void OnStaffToggleChanged(bool isOn)
    {
        if (isOn)
        {
            equipSwordToggle.isOn = false;
        }
        else if (!equipSwordToggle.isOn)
        {
            // Don't allow both toggles to be off
            equipStaffToggle.isOn = true;
        }
        UpdateStartButtonState();
    }

    private void OnEndlessModeChanged(bool isOn)
    {
        GameManager.Instance.SetEndlessMode(isOn);
        UpdateStartButtonState();
        UpdateEndlessModeUI(); // Update button text
    }

    private void UpdateStartButtonState()
    {
        // Enable start button only if exactly one weapon is selected
        bool weaponSelected = equipSwordToggle.isOn != equipStaffToggle.isOn;
        startBattleButton.interactable = weaponSelected;

        // Update button text to show requirement
        TextMeshProUGUI buttonText = startBattleButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = weaponSelected ? "Start Battle" : "Select a Weapon";
        }
    }

    private void OnStartBattle()
    {
        if (!equipSwordToggle.isOn && !equipStaffToggle.isOn)
        {
            Debug.LogWarning("No weapon selected!");
            return;
        }

        // Save selected weapon and mode
        PlayerPrefs.SetString("SelectedWeapon", equipSwordToggle.isOn ? "Sword" : "Staff");
        PlayerPrefs.SetInt("EndlessMode", endlessModeToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

        // Update GameManager state
        GameManager.Instance.SetSelectedWeapon(equipSwordToggle.isOn ? WeaponType.Sword : WeaponType.Staff);
        GameManager.Instance.SetEndlessMode(endlessModeToggle.isOn);

        // Load game scene
        SceneManager.LoadScene("Game");
    }

    private void AddDebugResource(ResourceType type, int amount)
    {
        if (inventory != null)
        {
            inventory.AddResource(type, amount);
            Debug.Log($"[DEBUG] Added {amount} {type}. New total: {inventory.GetResourceAmount(type)}");
        }
    }
} 