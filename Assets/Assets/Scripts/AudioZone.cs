using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// Handles audio zones  via entering and exiting zones (via snapshots) to configure room accoustic.
/// </summary>
public class AudioZone : MonoBehaviour
{
    [Tooltip("FMOD event for the sound played when entering the zone.")]
    public EventReference enterSound;

    [Tooltip("FMOD event for the sound played when exiting the zone.")]
    public EventReference exitSound;

    [Tooltip("FMOD snapshot for sound acoustics.")]
    // Snapshot for configuring  room acoustics.
    public EventReference soundSnapshot;

    // Instance of the underwater snapshot.
    private EventInstance soundSnapshotInstance;

    private void Start()
    {
        // Instance of sound snapshot at the start.
        soundSnapshotInstance = RuntimeManager.CreateInstance(soundSnapshot);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("AudioZone " + gameObject.name + " entered.");

            // Play enter sound.
            RuntimeManager.PlayOneShot(enterSound, transform.position);

            // Start the sound snapshot.
            soundSnapshotInstance.start();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("AudioZone " + gameObject.name + " left.");

            // Play exit sound.
            RuntimeManager.PlayOneShot(exitSound, transform.position);

            // Stop sound snapshot.
            soundSnapshotInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    private void OnDestroy()
    {
        // Release snapshot instance.
        soundSnapshotInstance.release();
    }
}
