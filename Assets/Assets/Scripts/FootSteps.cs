using UnityEngine;
using FMODUnity;

/// <summary>
/// Handles the footstep sounds based on avatars movement speed. Need to be finetuned witht the avatar movement speed (LocoMotion).
/// </summary>
public class Footsteps : MonoBehaviour
{
    
    [Tooltip("Movement distance to be counted as m inimummovement. Keep it as it is.")]
    // Minimum movement distance to count as a step.
    [SerializeField] private float movementThreshold = 0.01f;

    // Sounds for slow, normal and fast walking (at the moment configured with only one sound in Fmod)
    public EventReference footstepSlow;
    public EventReference footstepNormal;
    public EventReference footstepFast;

    // Saves the last position of the movement (to calculate movement distance).
    private Vector3 lastPosition;

    // Timer for controlling the play interval of sound.
    private float stepTimer;

    [Tooltip("Time interval for footsteps when moving slowly.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float stepIntervalSlow = 0.5f;

    [Tooltip("Time interval for footsteps when walking.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float stepIntervalWalking = 0.3f;

    [Tooltip("Time interval for footsteps when running.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float stepIntervalRunning = 0.2f;

    [Tooltip("Speed threshold for slow movement. See LocoMotion walking speed in avatar to match according movement m/sec.")]
    [Range(0.5f, 10.0f)]
    [SerializeField] private float slowSpeedThreshold = 1.8f;

    [Tooltip("Speed threshold for running. See LocoMotion running speed in avatar to match according movement m/sec.")]
    [Range(0.5f, 10.0f)]
    [SerializeField] private float runningSpeedThreshold = 4.2f;

    void Start()
    {
        // Init. last position.
        lastPosition = transform.position;
    }

    void Update()
    {
        // Up the time for each step.
        stepTimer += Time.deltaTime;

        // Current horizontal position and movement distance.
        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);
        float distance = Vector3.Distance(currentPosition, lastPosition);

        // Check if the current movement counted as proper footstep.
        if (distance > movementThreshold)
        {
            // Speed of movement.
            float movementSpeed = distance / Time.deltaTime;
            EventReference selectedSound;

            float selectedInterval;

            // Determine the right sound baed on interval and speed.
            if (movementSpeed < slowSpeedThreshold)
            {
                selectedSound = footstepSlow;
                selectedInterval = stepIntervalSlow;
            }
            else if (movementSpeed < runningSpeedThreshold)
            {
                selectedSound = footstepNormal;
                selectedInterval = stepIntervalWalking;
            }
            else
            {
                selectedSound = footstepFast;
                selectedInterval = stepIntervalRunning;
            }

            if (stepTimer >= selectedInterval)
            {
                PlayFootstepSound(selectedSound);
                stepTimer = 0f; // Reset the step timer.
            }
        }

        // Update last position.
        lastPosition = currentPosition;
    }

    void PlayFootstepSound(EventReference soundEvent)
    {
        // Play footstep sound at Avatars position.
        RuntimeManager.PlayOneShot(soundEvent, transform.position);
    }
}
