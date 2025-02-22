using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject shootPoint;
    
    public void OnFire(InputValue value)
    {
        if (value.isPressed)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = shootPoint.transform.position;
            bullet.transform.rotation = shootPoint.transform.rotation;
        }
    }



}
