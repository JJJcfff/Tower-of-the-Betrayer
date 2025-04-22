// Authors: Jeff Cui, Elaine Zhao
// Initializes the game scene, including floor difficulty

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneInitializer : MonoBehaviour
{
    [Header("Boss Settings")]
    public GameObject bossPrefab;  // Assign the Enemy Boss prefab in the inspector
    public Transform bossSpawnPoint;  // Where to spawn the boss (optional)
    public float bossSpawnDelay = 2f;  // Delay before the boss appears
    public GameObject bossHealthBarObject; // Reference to the BossHealthBar GameObject
    public GameObject regularBulletPrefab; // Regular bullet prefab for boss
    public GameObject largeBulletPrefab; // Large bullet prefab for boss special attack

    // Start is called before the first frame update
    void Start()
    {
        // Check if GameManager exists
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found! The game cannot function properly.");
            return;
        }
        
        // Check if boss prefab is missing
        if (bossPrefab == null)
        {
            Debug.LogError("[CRITICAL ERROR] Boss prefab is not assigned to GameSceneInitializer in the inspector!");
            // Continue anyway as we might not need it for this floor
        }
        
        GameManager.Instance.ApplyFloorDifficulty();
        
        int currentFloor = GameManager.Instance.currentFloor;
        bool isBossFloor = GameManager.Instance.IsNextFloorBoss();
        
        Debug.Log($"[GameSceneInitializer] Initializing floor {currentFloor}. Boss Floor? {isBossFloor}");
        
        // Setup boss health bar visibility
        if (bossHealthBarObject != null)
        {
            bossHealthBarObject.SetActive(isBossFloor);
        }
        
        if (isBossFloor)
        {
            Debug.Log("[GameSceneInitializer] Setting up BOSS floor!");
            SetupBossFloor();
        }
        else
        {
            Debug.Log("[GameSceneInitializer] Setting up normal floor with waves.");
            // Initialize normal floor
        }
    }
    
    void SetupBossFloor()
    {
        // Validation - make sure we have the boss prefab assigned
        if (bossPrefab == null)
        {
            Debug.LogError("[CRITICAL ERROR] Boss prefab not assigned to GameSceneInitializer!");
            
            // Look for the Enemy prefab as fallback
            GameObject enemyPrefab = Resources.Load<GameObject>("Enemy") ?? // Try Resources folder
                                   GameObject.FindObjectOfType<Enemy>()?.gameObject; // Or find first enemy in scene
            
            if (enemyPrefab != null)
            {
                Debug.Log($"[FALLBACK] Using {enemyPrefab.name} as boss fallback");
                bossPrefab = enemyPrefab;
            }
            else
            {
                // Create an empty boss object if all else fails
                GameObject emptyBoss = new GameObject("EmptyBoss");
                emptyBoss.AddComponent<Enemy>();
                Debug.Log("[FALLBACK] Created empty boss object");
            }
        }
        
        // Find all active GameObjects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        Debug.Log($"[SCENE CHECK] Found {allObjects.Length} objects in scene");
        foreach (var obj in allObjects)
        {
            if (obj.GetComponent<WaveSpawner>() != null)
            {
                Debug.Log($"[SCENE CHECK] WaveSpawner found: {obj.name}");
            }
        }
        
        // Properly disable and clean up all regular wave spawners
        WaveSpawner[] waveSpawners = FindObjectsOfType<WaveSpawner>();
        Debug.Log($"[BOSS SETUP] Found {waveSpawners.Length} wave spawners to disable");
        
        foreach (WaveSpawner spawner in waveSpawners)
        {
            // Cancel all invoked methods (including Spawn and EndSpawner)
            spawner.CancelInvoke();
            
            // Remove from wave manager
            if (WaveManager.instance != null)
            {
                WaveManager.instance.RemoveWave(spawner);
            }
            
            // Disable the component
            spawner.enabled = false;
            
            // Optionally, deactivate the GameObject
            spawner.gameObject.SetActive(false);
        }
        
        // Make sure there are no enemies already in the scene
        Enemy[] existingEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in existingEnemies)
        {
            Destroy(enemy.gameObject);
        }
        
        // Spawn the boss after a short delay
        Invoke(nameof(SpawnBoss), bossSpawnDelay);
        
        Debug.Log("Boss floor setup complete. Boss will spawn shortly...");
    }
    
    void SpawnBoss()
    {
        // Determine where to spawn the boss
        Vector3 spawnPosition;
        if (bossSpawnPoint != null)
        {
            // Use the assigned spawn point
            spawnPosition = bossSpawnPoint.position;
        }
        else
        {
            // Default to center of the map if no spawn point is assigned
            spawnPosition = new Vector3(0, 1, 0);
        }
        
        // Make sure we have a boss prefab
        if (bossPrefab == null)
        {
            Debug.LogError("Cannot spawn boss - no prefab assigned!");
            return;
        }
        
        // Spawn the boss
        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        
        // Make sure it has an Enemy component
        if (boss.GetComponent<Enemy>() == null)
        {
            boss.AddComponent<Enemy>();
        }
        
        // Make it more challenging
        var enemyHealth = boss.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.maxHealth *= 3.0f; // Triple the health for the boss
            enemyHealth.currentHealth = enemyHealth.maxHealth;
            enemyHealth.isBoss = true; // Mark as boss explicitly
        }
        
        // Disable the regular enemy FSM and add our boss combat component
        var enemyFSM = boss.GetComponentInChildren<EnemyFSM>();
        if (enemyFSM != null)
        {
            enemyFSM.enabled = false; // Disable regular enemy behavior
        }
        
        // Add boss combat component
        var bossCombat = boss.AddComponent<BossCombat>();
        if (bossCombat != null)
        {
            // Configure boss combat settings
            bossCombat.bulletPrefab = regularBulletPrefab;
            bossCombat.largeBulletPrefab = largeBulletPrefab;
            bossCombat.regularFireRate = 1.5f; // Slightly slower than regular enemies
            bossCombat.attackRange = 20f; // Larger attack range
            bossCombat.stoppingDistance = 3f; // Stop closer to player
            bossCombat.specialAttackCooldown = 5f; // Special attack every 5 seconds
            
            // If boss already has a NavMeshAgent, configure it
            var agent = boss.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.speed = 3.5f; // Make boss move faster
                agent.acceleration = 12f; // Faster acceleration
                agent.stoppingDistance = bossCombat.stoppingDistance;
            }
            
            Debug.Log("BossCombat component added to boss");
        }
        else
        {
            Debug.LogError("Failed to add BossCombat component to boss!");
        }
        
        Debug.Log($"Boss spawned! GameObject name: {boss.name}");
    }
} 