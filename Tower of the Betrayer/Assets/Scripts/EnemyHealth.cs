using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!(currentHealth <= 0)) return;
        Destroy(gameObject);
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
