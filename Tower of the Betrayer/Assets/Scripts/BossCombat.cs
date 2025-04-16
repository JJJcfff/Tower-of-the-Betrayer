using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossCombat : MonoBehaviour
{
    [Header("Regular Attack")]
    public GameObject bulletPrefab;
    public float regularFireRate = 1f;
    public float attackRange = 15f;
    public float stoppingDistance = 10f;

    [Header("Special Attacks")]
    public float specialAttackCooldown = 5f;
    public GameObject largeBulletPrefab;  // Larger bullet for special attack
    public float largeBulletSpeed = 15f;  // Speed for the large bullet
    public float dashSpeed = 20f;         // Speed for the dash attack
    public float dashDuration = 0.5f;     // Duration of the dash
    public int circularBulletCount = 12;  // Number of bullets in the circular pattern
    
    [Header("Boss State")]
    public bool isAttacking = false;
    public bool isDashing = false;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private float lastRegularAttackTime;
    private float lastSpecialAttackTime;
    private Rigidbody rb;
    private Vector3 dashDirection;
    private EnemyHealth bossHealth;
    private bool canPerformSpecialAttacks = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        bossHealth = GetComponent<EnemyHealth>();
        
        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        // Set up the agent
        if (agent != null)
        {
            agent.stoppingDistance = stoppingDistance;
        }
        
        // Initialize attack timers
        lastRegularAttackTime = 0f;
        lastSpecialAttackTime = 0f;
    }

    private void Update()
    {
        if (playerTransform == null || bossHealth.currentHealth <= 0)
            return;
            
        // Don't perform actions while dashing
        if (isDashing)
            return;
            
        // Check if we're on a boss floor
        if (GameManager.Instance != null && !GameManager.Instance.IsNextFloorBoss())
        {
            canPerformSpecialAttacks = false;
            return;
        }
        else
        {
            canPerformSpecialAttacks = true;
        }

        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        // Look at player
        LookAtPlayer();
        
        // Regular attack
        if (Time.time - lastRegularAttackTime >= regularFireRate && distanceToPlayer <= attackRange)
        {
            RegularAttack();
        }
        
        // Special attack
        if (canPerformSpecialAttacks && Time.time - lastSpecialAttackTime >= specialAttackCooldown)
        {
            // Choose a random special attack
            int attackType = Random.Range(0, 3);
            
            switch (attackType)
            {
                case 0:
                    StartCoroutine(DashAttack());
                    break;
                case 1:
                    CircularAttack();
                    break;
                case 2:
                    LargeBulletAttack();
                    break;
            }
            
            lastSpecialAttackTime = Time.time;
        }
        
        // Move towards player if not in attack range
        if (agent != null && agent.isActiveAndEnabled && !isDashing)
        {
            agent.SetDestination(playerTransform.position);
        }
    }
    
    private void LookAtPlayer()
    {
        if (playerTransform == null)
            return;
            
        // Calculate direction to player (only on the XZ plane)
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0;
        
        // Rotate to face the player
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    private void RegularAttack()
    {
        if (bulletPrefab == null)
            return;
            
        // Create bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 1.5f, transform.rotation);
        
        // Set bullet velocity if it has a Rigidbody
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = transform.forward * 10f;
        }
        
        lastRegularAttackTime = Time.time;
    }
    
    private void CircularAttack()
    {
        if (bulletPrefab == null)
            return;
            
        isAttacking = true;
        
        // Create bullets in a circle
        float angleStep = 360f / circularBulletCount;
        
        for (int i = 0; i < circularBulletCount; i++)
        {
            // Calculate the angle
            float angle = i * angleStep;
            
            // Calculate direction
            Vector3 direction = new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0,
                Mathf.Cos(angle * Mathf.Deg2Rad)
            );
            
            // Create a bullet
            GameObject bullet = Instantiate(bulletPrefab, transform.position + direction * 1.5f, Quaternion.LookRotation(direction));
            
            // Set bullet velocity if it has a Rigidbody
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = direction * 8f;
            }
        }
        
        isAttacking = false;
    }
    
    private void LargeBulletAttack()
    {
        if (largeBulletPrefab == null)
        {
            // Fallback to regular bullet but make it larger
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 1.5f, transform.rotation);
            bullet.transform.localScale *= 2f;
            
            // Increase damage
            ContactDamager damager = bullet.GetComponent<ContactDamager>();
            if (damager != null)
            {
                damager.damage *= 2f;
            }
            
            // Set bullet velocity
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = transform.forward * largeBulletSpeed;
            }
        }
        else
        {
            // Create a large bullet
            GameObject largeBullet = Instantiate(largeBulletPrefab, transform.position + transform.forward * 1.5f, transform.rotation);
            
            // Set bullet velocity
            Rigidbody bulletRb = largeBullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = transform.forward * largeBulletSpeed;
            }
        }
    }
    
    private IEnumerator DashAttack()
    {
        if (playerTransform == null || agent == null)
            yield break;
            
        // Start dashing
        isDashing = true;
        isAttacking = true;
        
        // Disable NavMeshAgent during dash
        agent.enabled = false;
        
        // Calculate dash direction
        dashDirection = (playerTransform.position - transform.position).normalized;
        dashDirection.y = 0;
        
        // Get current position
        Vector3 startPos = transform.position;
        Vector3 targetPos = playerTransform.position;
        
        // Store time
        float startTime = Time.time;
        float dashTime = 0;
        
        // Enable Rigidbody for physics
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        
        // Dash toward the player
        while (dashTime < dashDuration)
        {
            dashTime = Time.time - startTime;
            
            // Calculate progress (0 to 1)
            float progress = dashTime / dashDuration;
            
            // Move using Rigidbody
            if (rb != null)
            {
                rb.velocity = dashDirection * dashSpeed;
            }
            else
            {
                // Fallback to Transform movement
                transform.position = Vector3.Lerp(startPos, targetPos, progress);
            }
            
            yield return null;
        }
        
        // End dash
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
        
        // Re-enable NavMeshAgent
        agent.enabled = true;
        isDashing = false;
        isAttacking = false;
        
        // After dashing, perform a circular attack for added effect
        CircularAttack();
    }
    
    // Draw attack ranges
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
} 