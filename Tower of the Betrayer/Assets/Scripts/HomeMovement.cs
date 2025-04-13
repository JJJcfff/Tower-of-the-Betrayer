// Authors: Jeff Cui, Elaine Zhao
// Handles player movement and UI interaction triggers in the home scene.

using UnityEngine;

public class SmoothMoverWithUITrigger : MonoBehaviour
{
    public float moveSpeed = 5.5f;
    private Vector3? targetPosition = null;

    private Vector3 weaponPosition = new Vector3(-11f, 1f, -5f);
    private Vector3 potionsPosition = new Vector3(5.5f, 1f, -5f);
    private Vector3 neutralPosition = new Vector3(-2f, 1f, -9f);

    public HomeCanvasUIController canvasController;

void Start()
{
}
    void Update()
    {
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
            Debug.Log("Moving towards: " + targetPosition.Value);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.Value, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition.Value) < 0.01f)
            {
                Debug.Log("Prepare display");
                transform.position = targetPosition.Value;
                HandleArrival(targetPosition.Value);
                targetPosition = null;
            }
        }
    }

void HandleArrival(Vector3 arrivedPosition)

{
    Debug.Log("Arrived at: " + arrivedPosition);
    if (canvasController == null) return;

    if (Vector3.Distance(arrivedPosition, weaponPosition) < 0.1f)
    {
        Debug.Log("Arrived at weapon position");
        canvasController.ShowWeaponUI();
    }
    else if (Vector3.Distance(arrivedPosition, potionsPosition) < 0.1f)
    {
        Debug.Log("Arrived at potions position");
        canvasController.ShowPotionsUI();
    }
}

}
