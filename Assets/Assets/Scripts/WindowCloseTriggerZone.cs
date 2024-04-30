using DG.Tweening;
using UnityEngine;

/// <summary>
/// Handles the opening and closing of a window when a player enters or leaves a trigger zone.
/// </summary>
public class WindowOpenTriggerZone : MonoBehaviour
{
    [Tooltip("The window GameObject to be moved.")]
    public GameObject windowGO;

    [Tooltip("Ease mode for the animation.")]
    public Ease animationEase = Ease.Linear;

    [Tooltip("Duration of the animation (in seconds).")]
    [Range(0.1f, 5f)]
    public float openDuration = 1.0f;

    [Tooltip("Movement distance of the window.")]
    [Range(-5f, 5f)]
    public float distance = 1.0f;

    // Tween instance.
    private Tween windowTween;

    // State of the window.
    private bool windowIsOpen = false;

  
    private void OpenWindow()
    {
        if (windowTween != null && windowTween.IsActive())
            windowTween.Kill();

        windowTween = windowGO.transform.DOMoveY(distance, openDuration)
            .SetEase(animationEase)
            .SetRelative(true)
            .OnStart(() =>
            {
                Debug.Log("Starting to move the window.");
            })
            .OnComplete(() =>
            {
                Debug.Log("Window has been opened.");

                windowIsOpen = true;
            });
    }

 
    private void CloseWindow()
    {
        if (windowTween != null && windowTween.IsActive())
        {
            windowTween.Kill();
        }
            

        windowTween = windowGO.transform.DOMoveY(-distance, openDuration)
            .SetEase(animationEase)
            .SetRelative(true)
            .OnStart(() =>
            {
                Debug.Log("Starting to close the window.");
            })
            .OnComplete(() =>
            {
                Debug.Log("Window has been closed.");

                windowIsOpen = false;
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !windowIsOpen)
        {
            Debug.Log("Player has entered window area!");

            OpenWindow();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && windowIsOpen)
        {
            Debug.Log("Player has left window area!");

            CloseWindow();
        }
    }
}
