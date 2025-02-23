// Authors: Jeff Cui, Elaine Zhao

using UnityEngine;

// Manages the spawning of enemies in a wave.
public class WaveSpawner : MonoBehaviour
{
    public GameObject prefab;       // Enemy prefab to spawn.
    public float spawnRate;         // Interval between spawns.
    public float startTime;         // Time to start spawning.
    public float endTime;           // Time to stop spawning.
    
    
    [Header("Spawn Area Settings")]
    public float spawnRadius = 10f; // How far from center enemies can spawn
    public bool useCircularSpawn = true; // True for circular area, False for square area
    public Transform mapCenter; // Center of the spawn area

    void Start()
    {
        if (mapCenter == null)
        {
            mapCenter = transform;
        }
        
        WaveManager.instance.AddWave(this);
        InvokeRepeating(nameof(Spawn), startTime, spawnRate);
        Invoke(nameof(EndSpawner), endTime);
    }
    
    void Spawn()
    {
        Vector3 randomPosition = GetRandomSpawnPosition();
        Instantiate(prefab, randomPosition, Quaternion.identity);
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
