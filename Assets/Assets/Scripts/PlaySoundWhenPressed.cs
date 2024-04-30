using UnityEngine;
using UltimateXR.Devices;
using FMODUnity;
using UltimateXR.Manipulation;
using UltimateXR.Avatar;
using UltimateXR.Core;

/// <summary>
/// Plays a sound when a grabbable object is pressed with a trigger button while being held (Airhorn, Duck).
/// </summary>
public class PlaySoundWhenPressed : MonoBehaviour
{
    [SerializeField]
    private EventReference playedSound;

    // Reference to the UxrGrabbableObject component.
    private UxrGrabbableObject grabbableObject;

    // Flag to check if the object is grabbed by the right hand.
    private bool isGrabbedByRightHand = false;
    // Flag to check if the object is grabbed by the left hand.
    private bool isGrabbedByLeftHand = false;

    private void Awake()
    {
        // Get UxrGrabbableObject component.
        grabbableObject = GetComponent<UxrGrabbableObject>();
    }

    private void OnEnable()
    {
        // Subscribe to grab and release events.
        grabbableObject.Grabbed += OnGrabbed;
        grabbableObject.Released += OnReleased;
    }

    private void OnDisable()
    {
        // Unsubscribe from grab and release events.
        grabbableObject.Grabbed -= OnGrabbed;
        grabbableObject.Released -= OnReleased;
    }

    private void OnGrabbed(object sender, UxrManipulationEventArgs e)
    {
        // Set flag when object is being grabbed.
        if (e.Grabber.Side == UxrHandSide.Right)
        {
            isGrabbedByRightHand = true;
        }
        else if (e.Grabber.Side == UxrHandSide.Left)
        {
            isGrabbedByLeftHand = true;
        }
    }

    private void OnReleased(object sender, UxrManipulationEventArgs e)
    {
        // Reset flag when object is released (not grabbed anymore).
        if (e.Grabber.Side == UxrHandSide.Right)
        {
            isGrabbedByRightHand = false;
        }
        else if (e.Grabber.Side == UxrHandSide.Left)
        {
            isGrabbedByLeftHand = false;
        }
    }

    void Update()
    {
        // Check if trigger button is pressed (while the object is being grabbed).
        if ((isGrabbedByRightHand && UxrAvatar.LocalAvatarInput.GetButtonsPressDown(UxrHandSide.Right, UxrInputButtons.Trigger)) ||
            (isGrabbedByLeftHand && UxrAvatar.LocalAvatarInput.GetButtonsPressDown(UxrHandSide.Left, UxrInputButtons.Trigger)))
        {
            // And then play the sound.
            PlaySound();

            //Debug.Log("Right or left button is pressed");
        }
    }

    private void PlaySound()
    {
        // Play sound (posiiton at gameobject).
        RuntimeManager.PlayOneShot(playedSound, transform.position);
    }
}
