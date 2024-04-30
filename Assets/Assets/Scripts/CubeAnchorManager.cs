using UnityEngine;
using UltimateXR.Manipulation;

/// <summary>
/// Handles the placement and removal of cubes into sockets and sets the socket colours and updates the UI.
/// </summary>
public class CubeAnchorManager : MonoBehaviour
{
    [Tooltip("Add cube tag here. Each anchor shall have one unique cube (= color) tag.")]
    // Tag required for the cube to be accepted in this anchor.
    [SerializeField] private string requiredCubeTag;

    // GameObject that will change the color dependend if it has the right or wrong cube in it.
    public GameObject socketColourObject;

    // GameObject that will change the color to the color of the placed cube inside.
    public GameObject floorLightObject;

    private void Start()
    {
        // Get UxrGrabbableObjectAnchor component from GameObject.
        UxrGrabbableObjectAnchor anchor = GetComponent<UxrGrabbableObjectAnchor>();

        if (anchor != null)
        {
            // Register anchor (via GlobalAnchorManager)
            GlobalAnchorManager.Instance.RegisterAnchor(gameObject, requiredCubeTag);

            // Subscribe to OnObjectPlaced and OnObjectRemoved events.
            anchor.Placed += OnObjectPlaced;
            anchor.Removed += OnObjectRemoved;
        }
        else
        {
            Debug.LogError("GameObject does not have a UxrGrabbableObjectAnchor script - check it!");
        }
    }

    private void OnObjectPlaced(object sender, UxrManipulationEventArgs e)
    {
        // Check and then update the socket status once a cube is placed in the socket.
        GlobalAnchorManager.Instance.CheckAnchor(gameObject, e.GrabbableObject.gameObject);

        bool isCorrectCube = e.GrabbableObject.gameObject.CompareTag(requiredCubeTag);

        GlobalAnchorManager.Instance.UpdateSocketStatusUI();

        // Change color in socket.
        if (socketColourObject != null)
        {
            SocketColourChangeScript socketScript = socketColourObject.GetComponent<SocketColourChangeScript>();
            if (socketScript != null)
            {
                socketScript.ChangeColor(isCorrectCube);
            }
            else
            {
                Debug.LogError("GameObject has no SocketColourChangeScript!");
            }
        }
        else
        {
            Debug.LogError("No GameObject has socketColourObject!");
        }

        // Change color in floor lightning.
        if (floorLightObject != null)
        {
            FloorLight floorLight = floorLightObject.GetComponent<FloorLight>();

            if (floorLight != null)
            {
                Material cubeMaterial = e.GrabbableObject.gameObject.GetComponent<Renderer>().material;
                floorLight.ChangeFloorColor(cubeMaterial);
            }
            else
            {
                Debug.LogError("GameObject has no FloorLight script!");
            }
        }
        else
        {
            Debug.LogError("No GameObject assigned for floorLightObject!");
        }
    }

    private void OnObjectRemoved(object sender, UxrManipulationEventArgs e)
    {
        // Update anchor status when a cube is removed (in GlobalAnchorManager).
        if (GlobalAnchorManager.Instance != null)
        {
            GlobalAnchorManager.Instance.AnchorCubeRemoved(gameObject);
        }

        Debug.Log("Cube was removed!");

        // Update the Socket UI dashboard (UI)
        GlobalAnchorManager.Instance.UpdateSocketStatusUI();

        // Reset color to original.
        if (socketColourObject != null)
        {
            SocketColourChangeScript socketScript = socketColourObject.GetComponent<SocketColourChangeScript>();
            if (socketScript != null)
            {
                socketScript.ResetToOriginalColor();
            }
        }

        if (floorLightObject != null)
        {
            FloorLight floorLight = floorLightObject.GetComponent<FloorLight>();

            if (floorLight != null)
            {
                floorLight.ResetToOriginalColor();
            }
        }
    }
}
