using UnityEngine;
using UltimateXR.Core;
using UltimateXR.Manipulation;
using FMODUnity;
using System.Collections;
using UltimateXR.Haptics;
using UltimateXR.Haptics.Helpers;

/// <summary>
/// Handles the detection of shaking motions (rotations) for a grabbable item and plays a sound.
/// </summary>
public class CubeShakeScript : MonoBehaviour
{

    [Tooltip("Fmod Event for the shake sound.")]
    [SerializeField]
    private StudioEventEmitter shakeSound;

    [Tooltip("Rotation threshold of the item to consider being shaken.")]
    [SerializeField]
    [Range(0.1f, 60.0f)]
    // Threshold of rotation to mark when the cube is being shaken/rotated.
    private float shakeValue = 20.0f;

    // Flag if a sound is currently playing.
    private bool isSoundPlaying = false;

    // Flag to check if cube is currently grabbed by left or right hand.
    private bool isGrabbedByLeftHand = false;
    private bool isGrabbedByRightHand = false;

    // Saves last rotation of the item (comparing against the current rotation value)
    private Quaternion lastRotation;

    // Reference to the attached grabbable object component on item.
    private UxrGrabbableObject grabbableObject;

    // Set time for how long the sound effect should be played.
    [SerializeField]
    [Range(0.1f, 10.0f)]
    private float soundDuration = 0.2f;



    private void Awake()
    {
        grabbableObject = GetComponent<UxrGrabbableObject>();
    }

    private void OnEnable()
    {
        grabbableObject.Grabbed += OnGrabbed;
        grabbableObject.Released += OnReleased;
    }

    private void OnDisable()
    {
        grabbableObject.Grabbed -= OnGrabbed;
        grabbableObject.Released -= OnReleased;
    }


    //  Event handler for when the object is grabbed.
    private void OnGrabbed(object sender, UxrManipulationEventArgs e)
    {
        if (e.Grabber.Side == UxrHandSide.Left)
        {
            isGrabbedByLeftHand = true;
        }
        else if (e.Grabber.Side == UxrHandSide.Right)
        {
            isGrabbedByRightHand = true;
        }

        // Save current rotation.
        lastRotation = transform.rotation;
    }


    // Event handler for when the object is released.
    private void OnReleased(object sender, UxrManipulationEventArgs e)
    {
        if (e.Grabber.Side == UxrHandSide.Left)
        {
            isGrabbedByLeftHand = false;
        }
        else if (e.Grabber.Side == UxrHandSide.Right)
        {
            isGrabbedByRightHand = false;
        }
    }

    void Update()
    {
        Quaternion currentRotation = transform.rotation;

        // When item is grabbed by atleast left or right hand
        if (isGrabbedByLeftHand || isGrabbedByRightHand)
        {
            // Play the shake sound when angle between last and current rotiation is greater than the shakeValue threshold.
            if (Quaternion.Angle(lastRotation, currentRotation) > shakeValue)
            {
                Debug.Log("Cube is being shaken!");

                PlayShakeSound();
            }
        }

        lastRotation = currentRotation;
    }

    // Resets the sound playing flag after the duration of the sound.
    private IEnumerator ResetSoundAfterSoundIsPlayed()
    {
        yield return new WaitForSeconds(soundDuration);

        isSoundPlaying = false;
    }

    private void PlayShakeSound()
    {
        if (!isSoundPlaying)
        {
            shakeSound.Play();
            isSoundPlaying = true;

            // Short delay regarding to SoundDuration.
            StartCoroutine(ResetSoundAfterSoundIsPlayed());
        }
    }
}
