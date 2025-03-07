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
    public float meleeDamage = 50f;
    public float meleeAttackRange = 3f;
    public float meleeCooldown = 0.5f; // Cooldown for melee attacks

    [Header("Sword Settings")]
    public GameObject swordObject; // 3D sword capsule
    public GameObject staffObject; // 3D staff cube
    private Transform currentTarget; // Enemy being hit
    private Vector3 originalSwordScale; // Store original sword scale

    private bool hasSword;
    private bool hasStaff;

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

    private void Update()
    {
        attackTimer += Time.deltaTime;
        meleeTimer += Time.deltaTime;

        // Fire ranged weapon (staff) if available
        if (hasStaff && attackTimer >= 1f / attackSpeed)
        {
            ShootProjectile();
            attackTimer = 0f;
        }

        // Perform melee attack if available
        if (hasSword && meleeTimer >= meleeCooldown)
        {
            MeleeAttack();
            meleeTimer = 0f;
        }

        // Rotate Sword to Face Enemy
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            swordObject.transform.rotation = Quaternion.LookRotation(direction);
        }
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
