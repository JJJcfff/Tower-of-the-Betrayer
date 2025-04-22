// Authors: Jeff Cui, Elaine Zhao

using UnityEngine;
using UnityEngine.AI;

// Manages the spawning of enemies in a wave.
public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;  // Array to hold all enemy prefabs
    
    [Header("Wave Settings")]
    public float spawnRate;         // Interval between spawns.
    public float startTime;         // Time to start spawning.
    public float endTime;           // Time to stop spawning.
    
    [Header("Enemy Settings")]
    public float yellowEnemySpeedMultiplier = 0.6f;  // Speed multiplier for yellow enemies
    public float redEnemySpeedMultiplier = 1.2f;     // Speed multiplier for red enemies
    public float defaultEnemySpeedMultiplier = 1.0f; // Default speed multiplier for other enemies
    
    [Header("Spawn Area Settings")]
    public float spawnRadius = 10f; // How far from center enemies can spawn
    public bool useCircularSpawn = true; // True for circular area, False for square area
    public Transform mapCenter; // Center of the spawn area

    void Start()
    {
        // Validate enemy prefabs array
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("No enemy prefabs assigned to WaveSpawner!");
            return;
        }

        if (mapCenter == null)
        {
            mapCenter = transform;
        }
        
        // Keep spawn rate the same but adjust spawn duration based on floor difficulty
        if (FloorDifficultyManager.Instance != null)
        {
            // Get the spawn rate (stays constant)
            FloorDifficultyManager.Instance.ModifyEnemySpawnRate(ref spawnRate);
            
            // Increase the spawn duration instead
            FloorDifficultyManager.Instance.ModifyEnemySpawnDuration(ref endTime);
            Debug.Log($"Using spawn rate: {spawnRate}, Adjusted end time to {endTime} based on floor difficulty");
        }
        
        WaveManager.instance.AddWave(this);
        InvokeRepeating(nameof(Spawn), startTime, spawnRate);
        Invoke(nameof(EndSpawner), endTime);
    }
    
    void Spawn()
    {
        // Randomly select an enemy prefab from the array
        GameObject selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        
        Vector3 randomPosition = GetRandomSpawnPosition();
        GameObject spawnedEnemy = Instantiate(selectedPrefab, randomPosition, Quaternion.identity);
        
        // Apply appropriate speed multiplier based on enemy type
        NavMeshAgent agent = spawnedEnemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            // Get the Animator from the spawned enemy
            Animator enemyAnimator = spawnedEnemy.GetComponent<Animator>();
            
            // Base speed multiplier based on enemy type
            float baseMultiplier = defaultEnemySpeedMultiplier;
            if (spawnedEnemy.name.Contains("Yellow"))
            {
                baseMultiplier = yellowEnemySpeedMultiplier;
            }
            else if (spawnedEnemy.name.Contains("Red"))
            {
                baseMultiplier = redEnemySpeedMultiplier;
            }
            
            // Apply the base multiplier
            float finalSpeed = agent.speed * baseMultiplier;
            
            // Apply floor modifier for enemy speed if exists
            if (PlayerPrefs.HasKey("EnemySpeedModifier"))
            {
                float enemySpeedMod = PlayerPrefs.GetFloat("EnemySpeedModifier");
                // Apply the modifier as a multiplier (1 + modifier)
                finalSpeed *= (1f + enemySpeedMod);
                Debug.Log($"Applied enemy speed modifier: {enemySpeedMod}, Final speed: {finalSpeed}");
            }
            
            // Set the final speed
            agent.speed = finalSpeed;
            
            // Update the animator speed parameter
            if (enemyAnimator != null)
            {
                enemyAnimator.SetFloat("speed", agent.speed);
            }
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPos;
        
        if (useCircularSpawn)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            randomPos = new Vector3(
                mapCenter.position.x + randomCircle.x,
                mapCenter.position.y,
                mapCenter.position.z + randomCircle.y
            );
        }
        else
        {
            randomPos = new Vector3(
                mapCenter.position.x + Random.Range(-spawnRadius, spawnRadius),
                mapCenter.position.y,
                mapCenter.position.z + Random.Range(-spawnRadius, spawnRadius)
            );
        }

        return randomPos;
    }

    void EndSpawner()
    { 
        WaveManager.instance.RemoveWave(this);
        CancelInvoke();
    }

    void OnDrawGizmosSelected()
    {
        if (mapCenter == null) return;
        
        Gizmos.color = Color.red;
        if (useCircularSpawn)
        {
            Gizmos.DrawWireSphere(mapCenter.position, spawnRadius);
        }
        else
        {
            Vector3 center = mapCenter.position;
            Vector3 size = new Vector3(spawnRadius * 2, 0, spawnRadius * 2);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
