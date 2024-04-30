using UnityEngine;

/// <summary>
/// Handles the color change of the floor material.
/// </summary>
public class FloorLight : MonoBehaviour
{
    private Material originalMaterial;
    private Renderer floorRenderer;

    private void Start()
    {
        // Get the renderer component from the floor object.
        floorRenderer = GetComponent<Renderer>();

        // Save the original material for reset.
        originalMaterial = floorRenderer.material;
    }

    public void ChangeFloorColor(Material cubeMaterial)
    {
        // Set floor material to cube material.
        floorRenderer.material = cubeMaterial;
    }


    public void ResetToOriginalColor()
    {
        // Reset to original material.
        floorRenderer.material = originalMaterial;
    }
}
