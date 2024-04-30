using UnityEngine;
using FMODUnity;

/// <summary>
/// Handles the audio for collisions between the Club Mate bottle and walls (ones tagged with 'Wall').
/// </summary>
public class BottleWall : MonoBehaviour
{
    [Tooltip("FMOD event for playing sounds when collision with a wall.")]
    [SerializeField]
    private StudioEventEmitter bottleWallSound;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if object has the "Wall" tag.
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Play sound.
            bottleWallSound.Play();
        }
    }
}
