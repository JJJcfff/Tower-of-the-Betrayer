// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//  Manages enemy instances
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    
    public List<Enemy> enemies;
    public UnityEvent onChanged;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicate EnemyManager", gameObject);
            Destroy(gameObject);
        }
    }
    
    //  Adds an enemy to the list and notifies listeners.
    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        onChanged.Invoke();
    }
    
    // Removes an enemy from the list and notifies listeners.
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        onChanged.Invoke();
    }
}
