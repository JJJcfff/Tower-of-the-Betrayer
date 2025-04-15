// Authors: Jeff Cui, Elaine Zhao
// Handles player movement and UI interaction triggers in the home scene.

using UnityEngine;

public class SmoothMoverWithUITrigger : MonoBehaviour
{
    public float moveSpeed = 4.5f;
    public float rotationSpeed = 10f;

    private Vector3? targetPosition = null;

    private Vector3 weaponPosition = new Vector3(-11f, 1f, -5f);
    private Vector3 potionsPosition = new Vector3(5.5f, 1f, -5f);
    private Vector3 neutralPosition = new Vector3(-2f, 1f, -9f);

    public HomeCanvasUIController canvasController;

    // Store original rotation at game start
    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (canvasController != null)
        {
            if (Input.GetKeyDown(KeyCode.G) && canvasController.IsWeaponUIOpen())
            {
                canvasController.CloseAllUI();
                return;
            }

            if (Input.GetKeyDown(KeyCode.P) && canvasController.IsPotionsUIOpen())
            {
                canvasController.CloseAllUI();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            targetPosition = weaponPosition;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            targetPosition = potionsPosition;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            targetPosition = neutralPosition;
        }

        if (targetPosition.HasValue)
        {
            Vector3 direction = targetPosition.Value - transform.position;
            direction.y = 0f; // ignore vertical rotation

            if (direction.magnitude > 0.01f)
            {
                // Rotate toward target
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Move toward target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.Value, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition.Value) < 0.01f)
            {
                transform.position = targetPosition.Value;

                // Reset to original facing direction
                transform.rotation = originalRotation;

                HandleArrival(targetPosition.Value);
                targetPosition = null;
            }
        }
    }

    void HandleArrival(Vector3 arrivedPosition)
    {
        if (canvasController == null) return;

        if (Vector3.Distance(arrivedPosition, weaponPosition) < 0.1f)
        {
            canvasController.ShowWeaponUI();
        }
        else if (Vector3.Distance(arrivedPosition, potionsPosition) < 0.1f)
        {
            canvasController.ShowPotionsUI();
        }
    }

    public void GoToNeutral()
    {
        targetPosition = neutralPosition;
    }
}
