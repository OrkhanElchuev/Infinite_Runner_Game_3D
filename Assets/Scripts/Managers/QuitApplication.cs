using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Quits the application when a ESC key is pressed.
/// </summary>

public class QuitApplication : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
