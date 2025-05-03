// Authors: Jeff Cui, Elaine Zhao
// Handles player movement and UI interaction triggers in the home scene.
using UnityEngine;

public class SmoothMoverWithUITrigger : MonoBehaviour
{
    public float moveSpeed = 4.5f;
    public float rotationSpeed = 10f;

    private Vector3? targetPosition = null;

    private Vector3 weaponPosition = new Vector3(-11f, 1.5f, -5f);
    private Vector3 potionsPosition = new Vector3(4f, 1.5f, -5f);
    private Vector3 neutralPosition = new Vector3(-2f, 1.5f, -9f);

    public HomeCanvasUIController canvasController;

    private Quaternion originalRotation;
    public Animator animator;

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

        if (animator == null)
        {
            Debug.LogError("Animator component not found on player object.");
        }
        else
        {
            Debug.Log("Animator found: Ready to control animations.");
        }
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

        if (startedMovement && canvasController != null)
        {
            canvasController.CloseAllUI();

                
            if (animator != null)
            {
                animator.SetBool("isMoving", true);
                Debug.Log("Animator: movement just started → isMoving = true");
            }
        }




        bool currentlyMoving = false;

        if (targetPosition.HasValue)
        {
            float distance = Vector3.Distance(transform.position, targetPosition.Value);
            currentlyMoving = distance > 0.05f;

            if (currentlyMoving)
            {
                Vector3 direction = targetPosition.Value - transform.position;
                direction.y = 0f;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position, targetPosition.Value, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = targetPosition.Value;
                transform.rotation = originalRotation;

                HandleArrival();
                targetPosition = null;
            }
        }

        if (animator != null)
        {
            bool previous = animator.GetBool("isMoving");
            if (previous != currentlyMoving)
            {
                Debug.Log("Setting isMoving = " + currentlyMoving);
                animator.SetBool("isMoving", currentlyMoving);
            }
        }
        else
        {
            Debug.LogWarning("Animator still not assigned.");
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
                break;
        }

        targetUI = UIType.None;
    }

    public void GoToNeutral()
    {
        targetPosition = neutralPosition;
        targetUI = UIType.None;

        if (canvasController != null)
        {
            canvasController.CloseAllUI();
        }
    }
}
