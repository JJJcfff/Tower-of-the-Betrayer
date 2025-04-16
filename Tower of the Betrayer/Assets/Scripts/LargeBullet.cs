// Authors: Jeff Cui, Elaine Zhao
// Manages the boss's large bullet projectile with enhanced damage, visual effects, and impact behavior.

using UnityEngine;

public class LargeBullet : MonoBehaviour
{
    [Header("Bullet Properties")]
    public float damage = 20f;
    public float speed = 15f;
    public float lifetime = 5f;
    
    [Header("Visual Effects")]
    public Color bulletColor = new Color(1f, 0.2f, 0.2f);
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.2f;
    
    private Vector3 originalScale;
    private float startTime;
    private Renderer bulletRenderer;
    
    private void Start()
    {
        // Store original scale for pulsing effect
        originalScale = transform.localScale;
        startTime = Time.time;
        
        // Get renderer for color changes
        bulletRenderer = GetComponent<Renderer>();
        if (bulletRenderer != null && bulletRenderer.material != null)
        {
            bulletRenderer.material.color = bulletColor;
            bulletRenderer.material.EnableKeyword("_EMISSION");
            bulletRenderer.material.SetColor("_EmissionColor", bulletColor * 2f);
        }
        
        // Apply force if not already moving
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && rb.velocity.magnitude < 0.1f)
        {
            rb.velocity = transform.forward * speed;
        }
        
        // Set up contact damager if present
        ContactDamager damager = GetComponent<ContactDamager>();
        if (damager != null)
        {
            damager.damage = damage;
        }
        
        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }
    
    private void Update()
    {
        // Pulse the scale for visual effect
        float pulse = 1f + Mathf.Sin((Time.time - startTime) * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * pulse;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // If we hit a wall, create a small explosion effect
        if (other.CompareTag("Wall"))
        {
            CreateImpactEffect();
            Destroy(gameObject);
        }
        
        // Let ContactDamager handle damage
    }
    
    private void CreateImpactEffect()
    {
        // Simple impact effect - scale up rapidly and change color before destruction
        transform.localScale = originalScale * 2f;
        
        if (bulletRenderer != null)
        {
            bulletRenderer.material.color = Color.white;
            bulletRenderer.material.SetColor("_EmissionColor", Color.white * 5f);
        }
        
        // Disable collider to prevent multiple hits
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }
} 