using FMODUnity;
using UnityEngine;

/// <summary>
/// Manages the behavior of a floating balloon (animation and popping sound).
/// </summary>
public class BallonScript : MonoBehaviour
{
    [Tooltip("FMOD event for the sound played when the balloon pops.")]
    public EventReference ballonPopSound;

    [Tooltip("Prefab for confetti particle effects when the balloon pops.")]
    public GameObject konfettiPrefab;

    [Tooltip("Prefab for the balloons cord.")]
    public GameObject cordPrefab;

    [Tooltip("Vertical floating speed of the balloon.")]
    public float floatSpeed = 0.2f;

    [Tooltip("Amplitude of the balloons floating movement.")]
    public float floatAmp = 0.1f;

    // Floating height of the balloon.
    private float startHeight;

    // Timer to control the floating duration of the ballon.
    private float floatTime;

    private void Start()
    {
        // Save start position of the balloon (for animation and pivot point).
        startHeight = transform.position.y;
    }

    private void Update()
    {
        // Y-Movement up and down (based on a sine wave)
        floatTime += Time.deltaTime;
        float newY = startHeight + Mathf.Sin(floatTime * floatSpeed) * floatAmp;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Needle")
        {
            // Play balloon pop sound at its position.
            RuntimeManager.PlayOneShot(ballonPopSound, transform.position);

            // Destroy the balloon gameobject.
            Destroy(gameObject);

            // Destroy the cord.
            if (cordPrefab != null)
            {
                Destroy(cordPrefab);
            }

            // Instantiate confetti particles at the balloons position.
            if (konfettiPrefab != null)
            {
                Instantiate(konfettiPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
