// Authors: Jeff Cui, Elaine Zhao

using UnityEngine;

// Updates the camera's position based on the player's position.
public class CameraFollow2_5D : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;  

    // Updates the camera's position.
    private void LateUpdate()
    {
        if (player)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, player.position.z) + offset;
            transform.position = targetPosition;
        }
    }
}
