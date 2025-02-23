// Authors: Jeff Cui, Elaine Zhao

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Detects objects within a specified distance and angle (considering obstacles).
public class Sight : MonoBehaviour
{
    public float distance;           // Maximum detection distance.
    public float angle;              // Field of view angle.
    public LayerMask objectsLayers;  // Layers to detect objects.
    public LayerMask obstaclesLayers;// Layers considered as obstacles.
    public Collider detectedObject;  // The detected object.

    // Checks for objects within the detection range and field of view each frame
    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position, distance, objectsLayers);

        detectedObject = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];

            Vector3 directionToController = Vector3.Normalize(
                collider.bounds.center - transform.position);

            float angleToCollider = Vector3.Angle(
                transform.forward, directionToController);

            if (angleToCollider < angle)
            {
                if (!Physics.Linecast(transform.position,
                        collider.bounds.center, obstaclesLayers))
                {
                    detectedObject = collider;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);

        Vector3 rightDirection = Quaternion.Euler(0, angle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, rightDirection * distance);
        
        Vector3 leftDirection = Quaternion.Euler(0, -angle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftDirection * distance);
    }
}
