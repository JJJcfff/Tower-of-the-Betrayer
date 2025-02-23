// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Automatically destroys the GameObject after a specified time.
public class AutoDestroy : MonoBehaviour
{
    public float destroyTime = 5f; // Time in seconds before the GameObject is destroyed.

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
