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
    
    // Track which UI to show when player stops
    private UIType targetUI = UIType.None;
    
    private enum UIType
    {
        None,
        Weapon,
        Potion
    }

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

        bool startedMovement = false;

        if (Input.GetKeyDown(KeyCode.G))
        {
            targetPosition = weaponPosition;
            targetUI = UIType.Weapon;
            startedMovement = true;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            targetPosition = potionsPosition;
            targetUI = UIType.Potion;
            startedMovement = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            targetPosition = neutralPosition;
            targetUI = UIType.None;
            startedMovement = true;
        }
        
        // Hide UI when movement starts
        if (startedMovement && canvasController != null)
        {
            canvasController.CloseAllUI();
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

                HandleArrival();
                targetPosition = null;
            }
        }
    }

    void HandleArrival()
    {
        if (canvasController == null) return;

        switch (targetUI)
        {
            case UIType.Weapon:
                canvasController.ShowWeaponUI();
                break;
            case UIType.Potion:
                canvasController.ShowPotionsUI();
                break;
            case UIType.None:
                // Keep all UI closed
                break;
        }
        
        // Reset target UI
        targetUI = UIType.None;
    }

    public void GoToNeutral()
    {
        targetPosition = neutralPosition;
        targetUI = UIType.None;
        
        // Hide UI during movement
        if (canvasController != null)
        {
            canvasController.CloseAllUI();
        }
    }
}
