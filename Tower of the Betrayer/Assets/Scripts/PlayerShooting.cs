using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 10f;
    public float attackSpeed = 1f;    // Attacks per second
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab;

    private float attackTimer;

    private void Update()
    {
        attackTimer += Time.deltaTime;
        
        // Check if it's time for next attack
        if (attackTimer >= 1f / attackSpeed)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    private void Attack()
    {
        // Find closest enemy within range using OverlapSphere (more efficient than FindGameObjectsWithTag)
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        Transform nearestEnemy = null;
        float nearestDistance = attackRange;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = hitCollider.transform;
                }
            }
        }

        // If we found an enemy, shoot at it
        if (nearestEnemy != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector3 direction = (nearestEnemy.position - transform.position).normalized;
            
            // If your projectile has a Rigidbody
            if (projectile.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.velocity = direction * projectileSpeed;
            }
            
            // Optional: Make projectile face the direction it's moving
            projectile.transform.forward = direction;
        }
    }

    // Visualize attack range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
