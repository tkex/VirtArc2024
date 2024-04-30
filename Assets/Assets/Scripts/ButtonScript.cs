using UnityEngine;
using FMODUnity; // Import für FMOD Unity Integration
using System.Collections; // Dieser Import ist für IEnumerator erforderlich
using UnityEngine.UI;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    private bool isOn = false;

    [Header("FMOD Emitters")]
    // Emitter list for songs (configured in editor).
    [SerializeField]
    private StudioEventEmitter[] emitters;

    [SerializeField]
    private StudioEventEmitter buttonPushedEmitter;
    [SerializeField]
    private StudioEventEmitter buttonReleasedEmitter;

    private StudioEventEmitter currentEmitter;

    [Header("Settings")]
    [SerializeField]
    [Range(0.0F, 10.0F)]
    private float delayInSeconds; // Verzögerung in Sekunden zwischen Knopfdruck und Soundwiedergabe

    [Header("UI Elements")]
    [SerializeField]
    private TMP_Text radioText; // TextMesh Pro Textobjekt

    /// <summary>
    /// Toggles the radio state between on and off.
    /// </summary>
    public void ToggleRadioState()
    {
        isOn = !isOn;
        Debug.Log(isOn ? "Radio is now on." : "Radio is now off.");

        // Update TextMesh Pro Text
        radioText.text = isOn ? "An" : "Aus";

        if (isOn)
        {
            buttonPushedEmitter.Play(); // Play button push sound immediately
            StartCoroutine(DelayAction(PlayRandomSong)); // Play random song after delay
        }
        else
        {
            buttonReleasedEmitter.Play(); // Play button release sound immediately
            StopCurrentSong(); // Stop the song immediately
        }
    }

    /// <summary>
    /// Coroutine to execute an action after a delay.
    /// </summary>
    private IEnumerator DelayAction(System.Action action)
    {
        yield return new WaitForSeconds(delayInSeconds);
        action();
    }

    /// <summary>
    /// Plays a random song from the emitter list.
    /// </summary>
    private void PlayRandomSong()
    {
        if (emitters.Length == 0)
        {
            Debug.LogWarning("No emitters configured!");
            return;
        }

        // Stop the currently playing song before starting a new one.
        StopCurrentSong();

        // Choose a random emitter from the configured song list.
        int randomIndex = Random.Range(0, emitters.Length);
        currentEmitter = emitters[randomIndex];

        // Play the song.
        currentEmitter.Play();
    }

    /// <summary>
    /// Stops the currently playing song.
    /// </summary>
    private void StopCurrentSong()
    {
        if (currentEmitter != null && currentEmitter.IsPlaying())
        {
            // Stop the song.
            currentEmitter.Stop();
        }
    }

    /// <summary>
    /// Clean up FMOD ressources onces object is destroyed-
    /// </summary>
    private void OnDestroy()
    {
        StopCurrentSong();
    }
}
