// Manages the boss health bar UI display
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public Image healthBarImage;
    public TextMeshProUGUI bossNameText;
    public Color healthBarColor = new Color(0.8f, 0.2f, 0.6f); // Purple for boss health
    
    [Header("Boss Types")]
    public string defaultBossName = "Tower Guardian";
    public string[] alternativeBossNames = { "Corrupted Warden", "Shadow Tyrant" };
    
    private EnemyHealth bossHealth;
    private bool hasInitialized = false;
    private int bossTypeIndex = 0;

    private void Start()
    {
        // Initially hide the health bar
        gameObject.SetActive(false);

        // Set health bar color
        if (healthBarImage != null)
        {
            healthBarImage.color = healthBarColor;
        }

        // Check if we're on a boss floor
        if (GameManager.Instance != null && GameManager.Instance.IsNextFloorBoss())
        {
            // Will wait for boss to be spawned
            // The boss health bar should remain hidden until FindBoss() succeeds
            InvokeRepeating(nameof(FindBoss), 1f, 0.5f);
        }
    }

    private void FindBoss()
    {
        if (hasInitialized)
        {
            CancelInvoke(nameof(FindBoss));
            return;
        }

        // Try to find boss using Enemy component
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        foreach (EnemyHealth enemy in enemies)
        {
            // Check if this enemy is marked as a boss or has high health
            if (enemy.isBoss || enemy.maxHealth > 150)
            {
                ConnectToBoss(enemy);
                break;
            }
        }
    }

    public void ConnectToBoss(EnemyHealth boss)
    {
        if (boss == null) return;

        bossHealth = boss;
        hasInitialized = true;

        // Determine boss type based on floor or other factors
        DetermineBossType();

        // Set boss name
        if (bossNameText != null)
        {
            if (bossTypeIndex == 0 || bossTypeIndex > alternativeBossNames.Length)
            {
                bossNameText.text = defaultBossName;
            }
            else
            {
                bossNameText.text = alternativeBossNames[bossTypeIndex - 1];
            }
        }

        // Initialize health bar with current values
        UpdateHealthBar(boss.currentHealth, boss.maxHealth);

        // Show the health bar
        gameObject.SetActive(true);
    }
    
    // Public method to update the health bar directly from EnemyHealth
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarImage != null)
        {
            float fillAmount = maxHealth > 0 ? currentHealth / maxHealth : 0;
            healthBarImage.fillAmount = Mathf.Clamp01(fillAmount); // Ensure value is between 0 and 1
        }
    }
    
    private void DetermineBossType()
    {
        // Determine boss type based on floor number in endless mode or other factors
        if (GameManager.Instance != null)
        {
            int currentFloor = GameManager.Instance.currentFloor;
            
            if (currentFloor == GameManager.BOSS_FLOOR)
            {
                bossTypeIndex = 0; // Default boss
            }
            else if (currentFloor > GameManager.BOSS_FLOOR)
            {
                // In endless mode, cycle through different boss types
                bossTypeIndex = (currentFloor - GameManager.BOSS_FLOOR) % (alternativeBossNames.Length + 1);
            }
        }
    }

    private void Update()
    {
        // Return if we're not initialized or on non-boss floor
        if (!hasInitialized || bossHealth == null)
        {
            return;
        }

        // Check if boss health component still exists
        if (bossHealth == null)
        {
            gameObject.SetActive(false);
            return;
        }

        // Update health bar fill amount - now handled directly from EnemyHealth with UpdateHealthBar method
        // This is only a fallback in case EnemyHealth doesn't call UpdateHealthBar
        UpdateHealthBar(bossHealth.currentHealth, bossHealth.maxHealth);
        
        // Hide bar if boss is marked as dead or has no health
        if (bossHealth.currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetBossHealthBar()
    {
        hasInitialized = false;
        bossHealth = null;
        gameObject.SetActive(false);
    }

    // Called when changing scenes or floors
    public void OnLevelChange()
    {
        bool isBossFloor = GameManager.Instance != null && GameManager.Instance.IsNextFloorBoss();
        
        if (!isBossFloor)
        {
            // Not a boss floor, hide the bar
            ResetBossHealthBar();
        }
        else if (!hasInitialized)
        {
            // Boss floor but not initialized yet, try to find boss
            InvokeRepeating(nameof(FindBoss), 1f, 0.5f);
        }
    }
} 