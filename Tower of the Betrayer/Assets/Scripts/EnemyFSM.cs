using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float attackRange = 5f;
    public float stoppingDistance = 4f; 

    private NavMeshAgent agent;
    private float lastShootTime;
    private Transform playerTransform;

    private void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        agent.stoppingDistance = stoppingDistance;
    }

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

    private void LookTo(Vector3 targetPosition)
    {
        var directionToPosition = Vector3.Normalize(targetPosition - transform.parent.position);
        directionToPosition.y = 0;
        transform.parent.forward = directionToPosition;
    }

    private void Shoot()
    {
        var timeSinceLastShoot = Time.time - lastShootTime;
        if (timeSinceLastShoot < fireRate)
            return;

        lastShootTime = Time.time;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}