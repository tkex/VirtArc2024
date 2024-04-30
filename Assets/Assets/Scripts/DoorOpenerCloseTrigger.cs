using FMODUnity;
using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the opening and closing of a door when the player enters or exits its trigger area.
/// </summary>
public class DoorOpenerCloseTrigger : MonoBehaviour
{
    [Tooltip("Door GameObject that is opening and closing")]
    public GameObject doorObject;

    [Tooltip("Pivot point around which the door rotates.")]
    public Transform pivotPoint;

    [Tooltip("Maximum angle the door can open to.")]
    [Range(0, 180)]
    public float openAngle = 90.0f;

    [Tooltip("Speed at which the door opens and closes.")]
    [Range(0.1f, 100f)]
    public float openSpeed = 20.0f;

    // Current door angle during animation.
    private float currentAngle = 0.0f;

    // Coroutine for handlung the door movement.
    private Coroutine movingDoorCoroutine;

    [Tooltip("FMOD event for the door opening sound.")]
    public StudioEventEmitter doorSoundOpenEmitter;

    [Tooltip("FMOD event for the door closing sound.")]
    public StudioEventEmitter doorSoundCloseEmitter;
    private void StartMovingDoor(float newTargetAngle)
    {
        // Stops door movement coroutine and start a new one when called.
        if (movingDoorCoroutine != null)
        {
            StopCoroutine(movingDoorCoroutine);
        }
        movingDoorCoroutine = StartCoroutine(MoveDoor(newTargetAngle, newTargetAngle == openAngle ? doorSoundOpenEmitter : doorSoundCloseEmitter));
    }

    IEnumerator MoveDoor(float targetAngle, StudioEventEmitter soundEmitter)
    {
        // Play door movement sound when door is moving.
        soundEmitter.Play();

        // Set direction of rotation (regarding the configured target angle).
        int dir = currentAngle < targetAngle ? 1 : -1;

        // Rotate the door up to the the target angle.
        while ((dir == 1 && currentAngle < targetAngle) || (dir == -1 && currentAngle > targetAngle))
        {
            float angleStep = openSpeed * Time.deltaTime;
            currentAngle += angleStep * dir;
            doorObject.transform.RotateAround(pivotPoint.position, Vector3.up, angleStep * dir);
            yield return null;
        }

        // Stop the sound once the door movement is finished.
        soundEmitter.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player is in the trigger area.
        if (other.CompareTag("Player"))
        {            
            Debug.Log("Player has entered door area!");

            // Door is opening
            StartMovingDoor(openAngle);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if player is in the trigger area.
        if (other.CompareTag("Player"))
        {           
            Debug.Log("Player has left door area!");

            // Door is closing
            StartMovingDoor(0.0f);
        }
    }
}
