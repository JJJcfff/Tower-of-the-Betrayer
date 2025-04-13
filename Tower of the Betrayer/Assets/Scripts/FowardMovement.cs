// Authors: Jeff Cui, Elaine Zhao
// Simple script to move the attached GameObject forward continuously based on its speed.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FowardMovement : MonoBehaviour
{
    public float speed = 1f;
    
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
}
