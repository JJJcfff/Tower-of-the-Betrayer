// Authors: Jeff Cui, Elaine Zhao

using UnityEngine;
using UnityEngine.AI;

// Finite State Machine for an enemy that follows the player using NavMeshAgent and attacks when in range.
public class EnemyFSM : MonoBehaviour
{
    public GameObject bulletPrefab;    // Prefab for the bullet to shoot.
    public float fireRate = 1f;          // Time interval between shots.
    public float attackRange = 5f;       // Range within which the enemy can attack.
    public float stoppingDistance = 4f;  // Distance at which the enemy stops approaching the player.

    private NavMeshAgent agent;        // Reference to the enemy's NavMeshAgent component.
    private float lastShootTime;       // Timestamp of the last shot fired.
    private Transform playerTransform; // Reference to the player's transform.

    private void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        agent.stoppingDistance = stoppingDistance;
    }
    
    // Updates enemy behavior
    private void FixedUpdate()
    {
        if (!playerTransform) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= attackRange)
        {
            LookTo(playerTransform.position);
            Shoot();
        }
        
        agent.SetDestination(playerTransform.position);
    }

    // Rotates the enemy to face the target position
    private void LookTo(Vector3 targetPosition)
    {
        var directionToPosition = Vector3.Normalize(targetPosition - transform.parent.position);
        directionToPosition.y = 0;
        transform.parent.forward = directionToPosition;
    }

    // Shoots a bullet if the fire rate cooldown has passed
    private void Shoot()
    {
        var timeSinceLastShoot = Time.time - lastShootTime;
        if (timeSinceLastShoot < fireRate)
            return;

        lastShootTime = Time.time;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    // Draws gizmos to visualize the attack and stopping ranges.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}