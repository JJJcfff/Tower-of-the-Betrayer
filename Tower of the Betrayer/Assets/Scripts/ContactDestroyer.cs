// Authors: Jeff Cui, Elaine Zhao

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destroys both the current GameObject and any GameObject that collides with it.
public class ContactDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Destroy(other.gameObject);
        print("Destroyed");
    }
}
