using UnityEngine;
using UltimateXR.Manipulation;
using FMODUnity;

/// <summary>
/// Handle sound when cube is placed or removed form an anchor (socket).
/// </summary>
public class AnchorCubeSoundPlaced : MonoBehaviour
{
    [Tooltip("FMOD event for the sound played when the cube is placed.")]
    [SerializeField]
    private EventReference cubePlacedSound;

    [Tooltip("FMOD event for the sound played when the cube is removed.")]
    [SerializeField]
    private EventReference cubeRemovedSound;

    private UxrGrabbableObjectAnchor grabbableObjectAnchor;

    private void Start()
    {
        // Get UxrGrab component of current game object.
        grabbableObjectAnchor = GetComponent<UxrGrabbableObjectAnchor>();

        // Subscribe to events for placing and removing the cube.
        if (grabbableObjectAnchor != null)
        {
            grabbableObjectAnchor.Placed += OnObjectPlaced;
            grabbableObjectAnchor.Removed += OnObjectRemoved;
        }
    }

    private void OnObjectPlaced(object sender, UxrManipulationEventArgs e)
    {
        // Play the entered sound (once the cube is placed).
        RuntimeManager.PlayOneShot(cubePlacedSound, transform.position);
    }

    private void OnObjectRemoved(object sender, UxrManipulationEventArgs e)
    {
        // Play the remove sound (once the cube is removed).
        RuntimeManager.PlayOneShot(cubeRemovedSound, transform.position);
    }
}
