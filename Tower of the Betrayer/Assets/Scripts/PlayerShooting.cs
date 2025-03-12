// // Authors: Jeff Cui, Elaine Zhao

using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("General Attack Settings")]
    public float attackSpeed = 1f; // For ranged attacks
    private float attackTimer;
    private float meleeTimer;

    [Header("Ranged Attack Settings")]
    public float rangedAttackRange = 10f;
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab;

    [Header("Melee Attack Settings")]
    public float meleeDamage = 25f;
    public float meleeAttackRange = 0.5f;
    public float meleeCooldown = 0.5f; // Cooldown for melee attacks

    [Header("Sword Settings")]
    public GameObject swordObject; // 3D sword capsule
    public GameObject staffObject; // 3D staff cube
    private Transform currentTarget; // Enemy being hit
    private Vector3 originalSwordScale; // Store original sword scale

    private bool hasSword;
    private bool hasStaff;

    private Vector3 movementDirection; // Store the current movement direction

    private void Start()
    {
        hasSword = WeaponSelection.hasSword;
        hasStaff = WeaponSelection.hasStaff;

        Debug.Log("Weapons Loaded: Sword - " + hasSword + ", Staff - " + hasStaff);

        // Store the original sword scale
        if (hasSword && swordObject != null)
        {
            originalSwordScale = swordObject.transform.localScale;
            swordObject.SetActive(true);
        } else {
            swordObject.SetActive(false); 
        }

        if (hasStaff && staffObject != null)
        {
            staffObject.SetActive(true); 
        } else {
            staffObject.SetActive(false); 
        }

        // Initialize melee cooldown timer
        meleeTimer = meleeCooldown;
    }

    // Method to receive movement direction from PlayerMovement
    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        meleeTimer += Time.deltaTime;

        // Find nearest enemy regardless of range
        Transform nearestEnemy = FindNearestEnemy();
        
        if (nearestEnemy != null)
        {
            // Face the nearest enemy
            Vector3 direction = (nearestEnemy.position - transform.position).normalized;
            Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
            transform.rotation = Quaternion.LookRotation(horizontalDirection);
            
            // Rotate sword if we have a current target
            if (currentTarget != null && swordObject != null)
            {
                swordObject.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        else if (movementDirection.sqrMagnitude > 0.01f) // If moving and no enemies
        {
            // Face movement direction
            Vector3 horizontalMovement = new Vector3(movementDirection.x, 0, movementDirection.z).normalized;
            transform.rotation = Quaternion.LookRotation(horizontalMovement);
        }

        // Fire ranged weapon (staff) if available and enemy in range
        if (hasStaff && attackTimer >= 1f / attackSpeed && nearestEnemy != null && 
            Vector3.Distance(transform.position, nearestEnemy.position) <= rangedAttackRange)
        {
            ShootProjectile();
            attackTimer = 0f;
        }

        // Perform melee attack if available and enemy in range
        if (hasSword && meleeTimer >= meleeCooldown && nearestEnemy != null &&
            Vector3.Distance(transform.position, nearestEnemy.position) <= meleeAttackRange)
        {
            MeleeAttack();
            meleeTimer = 0f;
        }
    }

    private Transform FindNearestEnemy()
    {
        // Find ALL enemies in the scene with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    private void ShootProjectile()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, rangedAttackRange);
        Transform nearestEnemy = null;
        float nearestDistance = rangedAttackRange;

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

        if (nearestEnemy != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector3 direction = (nearestEnemy.position - transform.position).normalized;

            if (projectile.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.velocity = direction * projectileSpeed;
            }

            projectile.transform.forward = direction;
        }
    }

    private void MeleeAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeAttackRange);
        Transform nearestEnemy = null;
        float nearestDistance = meleeAttackRange;

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

        if (nearestEnemy != null)
        {
            EnemyHealth enemyHealth = nearestEnemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(meleeDamage);
                Debug.Log("⚔️ Melee Attack Hit: " + enemyHealth.gameObject.name);

                // Rotate the sword toward the enemy
                currentTarget = nearestEnemy;

                // Increase sword Y scale (double height)
                swordObject.transform.localScale = new Vector3(
                    originalSwordScale.x,
                    originalSwordScale.y * 2,
                    originalSwordScale.z
                );

                // Reset sword size after 0.1s
                Invoke(nameof(ResetSwordSize), 0.1f);
            }
        }
    }

    private void ResetSwordSize()
    {
        if (swordObject != null)
        {
            swordObject.transform.localScale = new Vector3(
                originalSwordScale.x, 
                originalSwordScale.y,
                originalSwordScale.z
            );
        }
        currentTarget = null;
    }
}
