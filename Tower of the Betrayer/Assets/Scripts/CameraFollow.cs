// Authors: Jeff Cui, Elaine Zhao
// Updates the camera's position to follow the player with a specified offset.

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    [Header("Shake Settings")]
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.2f;

    private float shakeTimer = 0f;
    private Vector3 originalPos;

    private void LateUpdate()
    {
        if (player)
        {
            Vector3 targetPosition = player.position + offset;

            if (shakeTimer > 0)
            {
                Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
                shakeOffset.z = 0; // Optional: Remove Z shake if you want it fixed
                transform.position = targetPosition + shakeOffset;

                shakeTimer -= Time.deltaTime;
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        shakeTimer = duration;
        shakeMagnitude = magnitude;
    }
}

