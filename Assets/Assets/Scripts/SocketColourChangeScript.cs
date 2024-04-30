using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the color changes of a socket based on the rightness / wrongness of a object placed within the socket.
/// </summary>
public class SocketColourChangeScript : MonoBehaviour
{

    private Renderer socketRenderer;
    private Material originalSocketMaterial;

    [Tooltip("Material when the correct cube is placed.")]
    public Material rightMaterial;

    [Tooltip("Material when the wrong cube is placed.")]
    public Material wrongMaterial;

    void Start()
    {
        // Get renderer component.
        socketRenderer = GetComponent<Renderer>();

        // Save original material (for later reset).
        originalSocketMaterial = socketRenderer.material;
    }

       public void ChangeColor(bool isCorrectCube)
    {
        // Change material if cube is placed into right socket.
        if (isCorrectCube)
        {
            // Set material to the right color.
            socketRenderer.material = rightMaterial;
        }
        else
        {
            // If wrong, set to wrong color.
            socketRenderer.material = wrongMaterial;
        }
    }

  
    public void ResetToOriginalColor()
    {
        // Reset to original material.
        socketRenderer.material = originalSocketMaterial;
    }
}
