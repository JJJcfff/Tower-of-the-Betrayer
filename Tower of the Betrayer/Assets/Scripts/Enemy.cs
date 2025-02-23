// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemey object: able to register and unregister itself with the EnemyManager.
public class Enemy : MonoBehaviour
{
    
    void Start()
    {
       EnemyManager.instance.AddEnemy(this);
    }
    
    void OnDestroy()
    {
        EnemyManager.instance.RemoveEnemy(this);
    }
}
