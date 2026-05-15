using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIInputModeController : MonoBehaviour
{
    [Header("Optional UI")]
    [SerializeField] Button firstSelectable;
    [SerializeField] GameObject keyboardControlsPanel;
    [SerializeField] GameObject controllerControlsPanel;

    void OnEnable()
    {
        if (InputDeviceDetector.Instance != null)
        {
            InputDeviceDetector.Instance.OnInputDeviceChanged += OnInputDeviceChanged;
            ApplyMode(InputDeviceDetector.Instance.CurrentInputDevice);
        }
        else
        {
            // Try to detect once even if detector isn't present yet
            DetectAndApplyFallback();
        }
    }

    void OnDisable()
    {
        if (InputDeviceDetector.Instance != null)
            InputDeviceDetector.Instance.OnInputDeviceChanged -= OnInputDeviceChanged;
    }

    void OnInputDeviceChanged(InputDeviceDetector.InputDeviceType newDevice)
    {
        ApplyMode(newDevice);
    }

    void ApplyMode(InputDeviceDetector.InputDeviceType device)
    {
        if (device == InputDeviceDetector.InputDeviceType.Gamepad)
        {
            Cursor.visible = false;
            // Keep lockState None so EventSystem still receives navigation input
            Cursor.lockState = CursorLockMode.None;

            if (firstSelectable != null)
                EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);

            if (keyboardControlsPanel != null)
                keyboardControlsPanel.SetActive(false);
            if (controllerControlsPanel != null)
                controllerControlsPanel.SetActive(true);
        }
        else // KeyboardMouse
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Clear selected so mouse can hover/click normally
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);

            if (keyboardControlsPanel != null)
                keyboardControlsPanel.SetActive(true);
            if (controllerControlsPanel != null)
                controllerControlsPanel.SetActive(false);
        }
    }

    void DetectAndApplyFallback()
    {
        // If InputDeviceDetector isn't present, try a simple one-frame check
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            ApplyMode(InputDeviceDetector.InputDeviceType.Gamepad);
        }
        else
        {
            ApplyMode(InputDeviceDetector.InputDeviceType.KeyboardMouse);
        }
    }
}
