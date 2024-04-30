using UnityEngine;

/// <summary>
/// Handles the functionality for switching between locomotion and teleportation via the UI buttons.
/// </summary>
public class MovementController : MonoBehaviour
{
    [Tooltip("GameObject that has the component for locomotion in Avatar.")]
    public GameObject locomotionGameObject;

    [Tooltip("GameObject that has the component for teleportation on the right hand.")]
    public GameObject teleportRightHandGameObject;

    [Tooltip("GameObject that has the component for teleportation on the left hand.")]
    public GameObject teleportLeftHandGameObject;

    void Start()
    {
        // Initially enable locomotion and disable teleportation.
        EnableLocomotion();
    }

   
    public void EnableLocomotion()
    {
        // Enable the locomotion system and disable the teleport system.
        if (locomotionGameObject != null)
        {
            // Enable locomotion.
            locomotionGameObject.SetActive(true);
        }

        if (teleportRightHandGameObject != null && teleportLeftHandGameObject != null)
        {
            // Disable right and left hand teleportation
            teleportRightHandGameObject.SetActive(false);
            teleportLeftHandGameObject.SetActive(false);
        }
    }

    public void EnableTeleport()
    {
        // Enable the teleport system and disable the locomotion system.
        if (teleportRightHandGameObject != null && teleportLeftHandGameObject != null)
        {
            // Enable right and left hand teleportation.
            teleportRightHandGameObject.SetActive(true);
            teleportLeftHandGameObject.SetActive(true);
        }

        if (locomotionGameObject != null)
        {
            // Disable locomotion.
            locomotionGameObject.SetActive(false);
        }
    }
}
