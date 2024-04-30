using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using System.IO;

/// <summary>
/// Handles the logging for the simulatio and logs the correct and wrong guessed rooms in log files.
/// </summary>
public class SimulationLogger : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Reference to GlobalAnchorManager (manages the simulation state).")]
    [SerializeField] private GlobalAnchorManager globalAnchorManager;

    [Tooltip("Button to trigger simulation logging (red button).")]
    [SerializeField] private Button button;

    [Tooltip("Canvas with the success / failure texts.")]
    [SerializeField] private GameObject canvas;

    [Header("Success Configuration")]
    [Tooltip("FMOD event for playing the winning sound.")]
    [SerializeField] private StudioEventEmitter winningSoundEmitter;

    [Tooltip("Textobject to display winning text.")]
    [SerializeField] private GameObject winningText;

    [Header("Failure Configuration")]
    [Tooltip("FMOD event for playing the losing sound.")]
    [SerializeField] private StudioEventEmitter losingSoundEmitter;

    [Tooltip("Textobject to display losing text.")]
    [SerializeField] private GameObject losingText;

    [Header("Incomplete Configuration")]
    [Tooltip("Textobject to display incomplete text.")]
    [SerializeField] private GameObject incompleteText;

    void Start()
    {
        globalAnchorManager.OnAllSocketsCorrectlyOccupiedChanged += HandleAllSocketsCorrectlyOccupiedChanged;

        // Setup initial UI states.
        winningText.SetActive(false);
        losingText.SetActive(false);
        incompleteText.SetActive(false);
    }

    void OnDestroy()
    {
        // Unsubscribe from events when destroyed.
        globalAnchorManager.OnAllSocketsCorrectlyOccupiedChanged -= HandleAllSocketsCorrectlyOccupiedChanged;
    }

    private void HandleAllSocketsCorrectlyOccupiedChanged(bool allOccupied)
    {
        //button.interactable = allOccupied;
        //canvas.SetActive(allOccupied);
    }

    public void DeactivateInteractionWithButton()
    {
        button.interactable = false;
    }

    public void ShowSuccess()
    {
        DeactivateInteractionWithButton();

        incompleteText.SetActive(false);
        winningSoundEmitter.Play();
        winningText.SetActive(true);

        Debug.Log("Success: All cubes are placed correctly! (SL)");
    }

    public void ShowFailure()
    {
        incompleteText.SetActive(false);

        losingSoundEmitter.Play();
        losingText.SetActive(true);

        Debug.Log("Failure: At least one cube mismatched. (SL)");

        DeactivateInteractionWithButton();
    }

    public void ShowIncomplete()
    {
        winningText.SetActive(false);
        losingText.SetActive(false);

        incompleteText.SetActive(true);
        Debug.Log("Incomplete: Some slots are not yet filled. (SL)");
    }

    public void OnButtonPressed()
    {
        // Check simulation state.
        if (globalAnchorManager.AreAllSocketsCorrectlyOccupied())
        {
            ShowSuccess();

            WriteLog();
        }
        else if (globalAnchorManager.AreAnySocketsUnoccupied())
        {
            ShowIncomplete();
            // No log written because only completed game is relevant.
        }
        else
        {
            ShowFailure();

            WriteLog();
        }
    }

    private void WriteLog()
    {
        // Write log messages (defined in GenerateLogMessageForAllAnchors())
        string logMessage = globalAnchorManager.GenerateLogMessageForAllAnchors();

        // Write log in "Logs" dir, otherwise create it
        string logsPath = Path.Combine(Application.dataPath, "Logs");

        if (!Directory.Exists(logsPath))
        {
            Directory.CreateDirectory(logsPath);
        }

        // Write log file with DD-MM-YYYY_HH-mm-ss format.
        string filePath = Path.Combine(logsPath, $"Log_{System.DateTime.Now:dd-MM-yyyy_HH-mm-ss}.log");

        File.WriteAllText(filePath, logMessage);

        Debug.Log($"Log file erstellt: {filePath}");
    }
}
