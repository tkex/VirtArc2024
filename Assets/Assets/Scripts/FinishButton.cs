using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Handlt the winning button and toggles its state once the game is finished.
/// </summary>
public class FinishButton : MonoBehaviour
{
    // If finish button is currently pressed or not.
    private bool isPressed = false;

    [Header("FMOD Emitters")]
    [SerializeField]
    private StudioEventEmitter finishButtonPushedEmitter;

    [SerializeField]
    private StudioEventEmitter finishButtonReleasedEmitter;


    public void ToggleFinishButtonState()
    {
        // Toggle state of the button.
        isPressed = !isPressed;

        // Play sound if button is pressed.
        if (isPressed)
        {
            // Play button push sound.
            finishButtonPushedEmitter.Play();
        }
        else
        {
            // Play button release sound if button is toggled to none.
            finishButtonReleasedEmitter.Play();
        }
    }
}
