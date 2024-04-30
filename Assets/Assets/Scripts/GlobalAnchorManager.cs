using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles the anchor sockets tag logic for the to be placed cubes via registration in sockets and updates the UI at the same time.
/// </summary>
public class GlobalAnchorManager : MonoBehaviour
{
    public static GlobalAnchorManager Instance;

    [Tooltip("Event triggered when all sockets are correctly or incorrectly occupied.")]
    public event Action<bool> OnAllSocketsCorrectlyOccupiedChanged;

    [SerializeField]
    [Tooltip("Text field for displaying the status of each socket.")]
    private TextMeshProUGUI socketStatusText;

    /// <summary>
    /// Save the required cube tags for each socket.
    /// </summary>
    public class AnchorTag
    {
        public string RequiredCubeTag;
        public GameObject PlacedCubeObject;
    }

    // Dictionary to map the anchor (socket) and required cube tag as KV-pairs.
    // Used to check if the cube with the required tag is 'allowed' (right) in the socket.
    private Dictionary<GameObject, AnchorTag> anchorData = new Dictionary<GameObject, AnchorTag>();

    // By default.
    private bool allSocketsCorrectlyOccupied = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GlobalAnchorManager - check!");

            Destroy(gameObject);

            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Registers an anchor with its cube tag (and updates the status UI)
    /// </summary>
    public void RegisterAnchor(GameObject socketAnchor, string requiredCubeTag)
    {
        anchorData[socketAnchor] = new AnchorTag { RequiredCubeTag = requiredCubeTag };

        // Update UI
        UpdateSocketStatusUI();
    }

    /// <summary>
    /// Checks the tag of the cube placed in a socket (and updates socket status).
    /// </summary>
    public void CheckAnchor(GameObject socketAnchor, GameObject placedCube)
    {
        if (anchorData.TryGetValue(socketAnchor, out AnchorTag anchorTag))
        {

            anchorTag.PlacedCubeObject = placedCube;

            if (!placedCube.CompareTag(anchorTag.RequiredCubeTag))
            {
                Debug.Log($"Incorrect cube in socket anchor {socketAnchor.name}. (Right tag: {anchorTag.RequiredCubeTag}, placed tag: {placedCube.tag}) (GAM)");
            }
            else
            {
                Debug.Log("Correct cube placed in the socket anchor! (GAM)");
            }
        }

        // Check if every socket is (in)correct occupied.
        CheckEveryAnchorSocket();

        // Update the UI.
        UpdateSocketStatusUI();
    }

    /// <summary>
    /// Check all anchors if all are correctly occupied by the right cubes (dependend on anchor tag).
    /// </summary>
    private void CheckEveryAnchorSocket()
    {
        foreach (var pair in anchorData)
        {
            if (pair.Value.PlacedCubeObject == null || !pair.Value.PlacedCubeObject.CompareTag(pair.Value.RequiredCubeTag))
            {
                if (allSocketsCorrectlyOccupied)
                {
                    allSocketsCorrectlyOccupied = false;

                    // Notify subscribed listeners that not all sockets are correctly occupied.
                    OnAllSocketsCorrectlyOccupiedChanged?.Invoke(false);

                    Debug.Log("Not all socket anchors are occupied by the correct cubes yet. (GAM)");
                }

                return;
            }
        }

        if (!allSocketsCorrectlyOccupied)
        {
            allSocketsCorrectlyOccupied = true;

            // Notify subscribed listeners that all sockets are correctly occupied.
            OnAllSocketsCorrectlyOccupiedChanged?.Invoke(true);

            Debug.Log("Nice! All cubes are placed correctly! (GAM)");
        }
    }

    /// <summary>
    /// For SimulationLogger. Check if all sockets are correctly occupied. 
    /// WIP: Eventually refactor since CheckEveryAnchorSocket() exists.
    /// </summary>
    public bool AreAllSocketsCorrectlyOccupied()
    {
        // Check if sockets are correctly occupied. Check if sockets are corrctly occupied.
        foreach (var pair in anchorData)
        {
            if (pair.Value.PlacedCubeObject == null || !pair.Value.PlacedCubeObject.CompareTag(pair.Value.RequiredCubeTag))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// For SimulationLogger. Check if any sockets are (still) unoccupied. 
    /// WIP: Eventually refactor since CheckEveryAnchorSocket() exists.
    /// </summary>
    public bool AreAnySocketsUnoccupied()
    {
        foreach (var pair in anchorData)
        {
            // Check if socket is unoccupied.
            if (pair.Value.PlacedCubeObject == null)
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Updates the socket status UI to the current state of each socket anchor.
    /// </summary>
    public void UpdateSocketStatusUI()
    {
        // Empty string.
        string statusText = "";

        foreach (var pair in anchorData)
        {
            string cubeName = pair.Value.PlacedCubeObject != null ? pair.Value.PlacedCubeObject.name : "Not assigned";
            statusText += $"{pair.Key.name}: {cubeName}\n";
        }
        if (socketStatusText != null)
        {
            socketStatusText.text = statusText;
        }
        else
        {
            Debug.LogError("SocketStatusText is not assigned!");
        }
    }

    /// <summary>
    /// Handles the removal of a cube from a socket (and updates the status).
    /// </summary>
    public void AnchorCubeRemoved(GameObject socketAnchor)
    {
        if (anchorData.TryGetValue(socketAnchor, out AnchorTag anchorTag))
        {
            anchorTag.PlacedCubeObject = null;
        }
        UpdateSocketStatusUI();
    }

    /// <summary>
    /// Generates log message that has the status of all anchors (used in SimulationLogger).
    /// </summary>
    public string GenerateLogMessageForAllAnchors()
    {
        string logmsg = $"Log-Datum: {DateTime.Now}\n\n";

        foreach (var pair in anchorData)
        {
            string status = pair.Value.PlacedCubeObject != null && pair.Value.PlacedCubeObject.CompareTag(pair.Value.RequiredCubeTag) ? "Richtig" : "Falsch";

            logmsg += $"Socket {pair.Key.name}: {status}\n";
        }

        return logmsg;
    }

}

