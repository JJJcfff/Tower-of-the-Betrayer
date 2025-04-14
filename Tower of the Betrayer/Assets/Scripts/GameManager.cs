// Authors: Jeff Cui, Elaine Zhao
// Singleton class that manages the overall game state, including floor progression, score, weapon selection, and scene transitions.

using UnityEngine;
using UnityEngine.SceneManagement;
using Inventory;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Core game state
    public int currentFloor = 1;
    public int score = 0;
    
    // Weapon state
    public bool hasSword = false;
    public bool hasStaff = false;
    
    // References to other managers
    public ScoreManager scoreManager;
    public EnemyManager enemyManager;
    public WaveManager waveManager;

    private WeaponType selectedWeapon = WeaponType.Sword;
    private bool endlessMode = false;
    private bool nextFloorIsBoss = false; // Flag to indicate if next floor should be boss
    public const int BOSS_FLOOR = 10; // Floor where boss fight becomes available

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeGame()
    {
        currentFloor = 1;
        score = 0;
        hasSword = false;
        hasStaff = false;
        endlessMode = false;
        nextFloorIsBoss = false;
    }

    public void StartNewGame()
    {
        // Reset game state
        selectedWeapon = WeaponType.Sword;
        endlessMode = false;
        nextFloorIsBoss = false;
        currentFloor = 1;
        
        // Reset player stats to default if needed
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetToDefault();
        }
    }

    public void SetSelectedWeapon(WeaponType weapon)
    {
        selectedWeapon = weapon;
        Debug.Log($"Selected weapon set to: {weapon}");
    }

    public WeaponType GetSelectedWeapon()
    {
        return selectedWeapon;
    }

    public void SetEndlessMode(bool enabled)
    {
        // Only allow endless mode toggle after reaching boss floor
        if (currentFloor >= BOSS_FLOOR)
        {
            // If turning endless mode off (was on before), make next floor the boss floor
            if (endlessMode && !enabled)
            {
                nextFloorIsBoss = true;
                PlayerPrefs.SetInt("NextFloorIsBoss", 1);
                PlayerPrefs.Save();
                Debug.Log("Endless mode disabled - next floor will be the boss floor!");
            }
            else
            {
                nextFloorIsBoss = false;
                PlayerPrefs.SetInt("NextFloorIsBoss", 0);
                PlayerPrefs.Save();
            }
            
            endlessMode = enabled;
            PlayerPrefs.SetInt("EndlessMode", enabled ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log($"Endless mode set to: {enabled}");
        }
        else
        {
            Debug.LogWarning($"Cannot toggle endless mode before floor {BOSS_FLOOR}");
            endlessMode = false;
            nextFloorIsBoss = false;
            PlayerPrefs.SetInt("NextFloorIsBoss", 0);
            PlayerPrefs.SetInt("EndlessMode", 0);
            PlayerPrefs.Save();
        }
    }

    public bool IsEndlessModeEnabled()
    {
        return endlessMode;
    }
    
    public bool IsNextFloorBoss()
    {
        // Check if this is explicitly marked as a boss floor from endless mode toggle
        if (nextFloorIsBoss)
        {
            Debug.Log($"[Boss Check] nextFloorIsBoss flag is TRUE - this is a boss floor");
            return true;
        }
            
        // Or if this is the standard boss floor (10) and not in endless mode
        if (currentFloor == BOSS_FLOOR && !endlessMode)
        {
            Debug.Log($"[Boss Check] Floor {currentFloor} is boss floor and endless mode is disabled");
            return true;
        }
        
        Debug.Log($"[Boss Check] Not a boss floor. Floor: {currentFloor}, Endless: {endlessMode}, NextFloorIsBoss: {nextFloorIsBoss}");
        return false;
    }

    public bool CanToggleEndlessMode()
    {
        return currentFloor >= BOSS_FLOOR;
    }

    public void CompleteFloor(bool success)
    {
        if (success)
        {
            Debug.Log($"Completed floor {currentFloor}");
            
            // Clear the boss flag if we just completed the boss floor
            if (nextFloorIsBoss)
            {
                nextFloorIsBoss = false;
            }
            
            currentFloor++;
            Debug.Log($"Next floor will be {currentFloor}");

            // Mark that new modifiers need to be generated for the next floor
            if (FloorDifficultyManager.Instance != null)
            {
                FloorDifficultyManager.Instance.MarkForModifierGeneration();
            }

            // Return to home screen between floors
            SceneManager.LoadScene("Home");
        }
        else
        {
            // Handle failure
            SceneManager.LoadScene("LoseScreen");
        }
    }

    // Called when entering the Game scene
    public void ApplyFloorDifficulty()
    {
        // Load boss flag from PlayerPrefs
        nextFloorIsBoss = PlayerPrefs.GetInt("NextFloorIsBoss", 0) == 1;
        
        // Apply the existing modifiers that were generated in the home scene
        if (FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ApplyExistingModifiers();
        }
        
        Debug.Log($"[Game Scene Start] Floor: {currentFloor}, Boss Flag: {nextFloorIsBoss}, Endless: {endlessMode}");
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene("Home");
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}