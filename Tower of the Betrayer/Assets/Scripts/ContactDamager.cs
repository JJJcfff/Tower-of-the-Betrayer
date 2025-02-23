// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Applies damage to any PlayerHealth or EnemyHealth component on objects that collide with this object.
public class ContactDamager : MonoBehaviour
{
    public float damage; // Amount of damage to apply on contact.
    
    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
        
        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
