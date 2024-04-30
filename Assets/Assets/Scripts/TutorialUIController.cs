using UnityEngine;
using TMPro; // Library for TextMeshPro elements
using UltimateXR.UI.UnityInputModule.Controls;
using UltimateXR.UI.UnityInputModule;

/// <summary>
/// Handle the tutorial panels on the tablet.
/// </summary>
public class TutorialUIController : MonoBehaviour
{
    [Tooltip("Array of the Text GameObjects that contain the tutorial how to handle the item.")]
    public GameObject[] tutorials;

    [Tooltip("Reference to the 1/N GameObject to show current state of tutorial.")]
    // Tutorial step number.
    public TMP_Text counterText;

    [SerializeField]
    // Index of the tutorial that is shown (0: first one).
    private int currentIndex = 0;

    private void Start()
    {
        // Initialize.
        UpdateTutorialState();
    }

    private void UpdateTutorialState()
    {
        // Debug.Log("UI Update!");

        if (tutorials.Length == 0)
        {
            Debug.LogError("No tutorials assigned.");

            return;
        }

        // Update tutorials and display the current one
        foreach (GameObject tutorial in tutorials)
        {
            tutorial.SetActive(false);
        }

        tutorials[currentIndex].SetActive(true);

        // Update counter text
        counterText.text = $"{currentIndex + 1}/{tutorials.Length}";
    }

  
    public void NextTutorial()
    {
        if (currentIndex < tutorials.Length - 1)
        {
            // Set to next tutorial.
            currentIndex++;
        }
        else
        {
            // Reset index back to the start (if end is reached).
            currentIndex = 0;
        }

        // Update to the next tutorial.
        UpdateTutorialState();
    }
}
