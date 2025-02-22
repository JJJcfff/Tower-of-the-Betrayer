using UnityEngine;

public class CameraFollow2_5D : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;  

    private void LateUpdate()
    {
        if (player)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, player.position.z) + offset;
            transform.position = targetPosition;
        }
    }
}
