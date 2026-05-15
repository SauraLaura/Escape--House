using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceDetector : MonoBehaviour
{
    public enum InputDeviceType { KeyboardMouse, Gamepad }

    static InputDeviceDetector _instance;
    public static InputDeviceDetector Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing instance in the scene
                _instance = FindFirstObjectByType<InputDeviceDetector>();
                if (_instance == null)
                {
                    // Create a new persistent GameObject with the detector
                    var go = new GameObject("InputDeviceDetector");
                    _instance = go.AddComponent<InputDeviceDetector>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
        private set { _instance = value; }
    }

    private InputDeviceType currentInputDevice = InputDeviceType.KeyboardMouse;

    public InputDeviceType CurrentInputDevice => currentInputDevice;

    public delegate void InputDeviceChangedDelegate(InputDeviceType newDevice);
    public event InputDeviceChangedDelegate OnInputDeviceChanged;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        InputSystem.onActionChange += OnInputActionChange;
        DetectInputDevice();
    }

    void OnDisable()
    {
        InputSystem.onActionChange -= OnInputActionChange;
    }

    void OnInputActionChange(object actionOrBinding, InputActionChange change)
    {
        DetectInputDevice();
    }

    void DetectInputDevice()
    {
        InputDeviceType newInputDevice = currentInputDevice;

        // Check if gamepad is being used
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            newInputDevice = InputDeviceType.Gamepad;
        }
        // Check if keyboard or mouse is being used
        else if ((Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame) ||
                 (Mouse.current != null && Mouse.current.wasUpdatedThisFrame))
        {
            newInputDevice = InputDeviceType.KeyboardMouse;
        }

        // If input device changed, notify subscribers
        if (newInputDevice != currentInputDevice)
        {
            currentInputDevice = newInputDevice;
            OnInputDeviceChanged?.Invoke(currentInputDevice);
        }
    }

    public bool IsGamepadActive => currentInputDevice == InputDeviceType.Gamepad;
    public bool IsKeyboardMouseActive => currentInputDevice == InputDeviceType.KeyboardMouse;
}
