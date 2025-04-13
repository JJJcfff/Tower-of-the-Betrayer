// Authors: Jeff Cui, Elaine Zhao
// Manages player attacks, including ranged (staff) and melee (sword) logic.

using UnityEngine;
using Inventory;

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

    [Header("Weapon Settings")]
    public GameObject swordObject; // 3D sword capsule
    public GameObject staffObject; // 3D staff cube
    private Transform currentTarget; // Enemy being hit
    private Vector3 originalSwordScale; // Store original sword scale

    private WeaponType selectedWeapon;
    private Vector3 movementDirection; // Store the current movement direction

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        selectedWeapon = GameManager.Instance.GetSelectedWeapon();
        Debug.Log($"Selected weapon: {selectedWeapon}");

        // Store the original sword scale and setup weapon objects
        if (selectedWeapon == WeaponType.Sword)
        {
            if (swordObject != null)
            {
                originalSwordScale = swordObject.transform.localScale;
                swordObject.SetActive(true);
            }
            if (staffObject != null)
            {
                staffObject.SetActive(false);
            }
        }
        else // Staff
        {
            if (swordObject != null)
            {
                swordObject.SetActive(false);
            }
            if (staffObject != null)
            {
                staffObject.SetActive(true);
            }
        }

        // Initialize melee cooldown timer
        meleeTimer = meleeCooldown;

        // Update weapon stats from PlayerStats
        UpdateWeaponStats();
    }

    private void UpdateWeaponStats()
    {
        if (PlayerStats.Instance == null)
        {
            Debug.LogError("PlayerStats not found!");
            return;
        }

        // Update stats based on selected weapon
        if (selectedWeapon == WeaponType.Sword)
        {
            meleeDamage = PlayerStats.Instance.GetWeaponDamage(WeaponType.Sword);
            meleeAttackRange = PlayerStats.Instance.GetWeaponRange(WeaponType.Sword);
            meleeCooldown = 1f / PlayerStats.Instance.GetWeaponSpeed(WeaponType.Sword);
        }
        else // Staff
        {
            attackSpeed = PlayerStats.Instance.GetWeaponSpeed(WeaponType.Staff);
            rangedAttackRange = PlayerStats.Instance.GetWeaponRange(WeaponType.Staff);
            // Set projectile damage in the ContactDamager component of the projectile prefab
            if (projectilePrefab != null)
            {
                ContactDamager damager = projectilePrefab.GetComponent<ContactDamager>();
                if (damager != null)
                {
                    damager.damage = PlayerStats.Instance.GetWeaponDamage(WeaponType.Staff);
                }
            }
        }
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
            // Calculate direction to enemy
            Vector3 direction = (nearestEnemy.position - transform.position).normalized;
            Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
            
            // Make player face the enemy instantly
            transform.forward = horizontalDirection;
            
            // Weapons follow the exact aim direction (including vertical)
            if (selectedWeapon == WeaponType.Sword && swordObject != null)
            {
                swordObject.transform.rotation = Quaternion.LookRotation(direction);
            }
            else if (selectedWeapon == WeaponType.Staff && staffObject != null)
            {
                staffObject.transform.rotation = Quaternion.LookRotation(direction);
            }

            // Fire ranged weapon (staff) if available and enemy in range
            if (selectedWeapon == WeaponType.Staff && attackTimer >= 1f / attackSpeed && 
                Vector3.Distance(transform.position, nearestEnemy.position) <= rangedAttackRange)
            {
                ShootProjectile();
                attackTimer = 0f;
            }

            // Perform melee attack if available and enemy in range
            if (selectedWeapon == WeaponType.Sword && meleeTimer >= meleeCooldown &&
                Vector3.Distance(transform.position, nearestEnemy.position) <= meleeAttackRange)
            {
                MeleeAttack();
                meleeTimer = 0f;
            }
        }
        else if (movementDirection.sqrMagnitude > 0.01f)
        {
            // If no enemies but moving, face movement direction instantly
            Vector3 horizontalMovement = new Vector3(movementDirection.x, 0, movementDirection.z).normalized;
            transform.forward = horizontalMovement;
            
            // Weapons follow the movement direction
            if (selectedWeapon == WeaponType.Sword && swordObject != null)
            {
                swordObject.transform.rotation = Quaternion.LookRotation(horizontalMovement);
            }
            else if (selectedWeapon == WeaponType.Staff && staffObject != null)
            {
                staffObject.transform.rotation = Quaternion.LookRotation(horizontalMovement);
            }
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
