using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] Button firstPauseButton;
    [SerializeField] GameObject keyboardMouseControlsPanel;
    [SerializeField] GameObject controllerControlsPanel;
    [SerializeField] bool pauseOnEscape = true;
    [SerializeField] string mainMenuSceneName = "MainMenu";

    // EventSystem eventSystem;

    InputSystem_Actions inputActions;
    InputAction pauseAction;
    bool isPaused;

    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        pauseAction = inputActions.Player.Pause;
        pauseAction.performed += OnPausePressed;
        // eventSystem = FindFirstObjectByType<EventSystem>();

        // Subscribe to input device changes from InputDeviceDetector
        if (InputDeviceDetector.Instance != null)
        {
            InputDeviceDetector.Instance.OnInputDeviceChanged += OnInputDeviceChanged;
            UpdateControlsPanels();
        }

        InputSystem.onActionChange += OnInputActionChange;
    }

    void OnDisable()
    {
        pauseAction.performed -= OnPausePressed;
        if (InputDeviceDetector.Instance != null)
        {
            InputDeviceDetector.Instance.OnInputDeviceChanged -= OnInputDeviceChanged;
        }
        inputActions.Disable();
        InputSystem.onActionChange -= OnInputActionChange;
    }

    void OnPausePressed(InputAction.CallbackContext context)
    {
        if (pauseOnEscape)
        {
            TogglePause();
        }
    }

    void OnInputDeviceChanged(InputDeviceDetector.InputDeviceType newDevice)
    {
        UpdateControlsPanels();
    }

    void OnInputActionChange(object actionOrBinding, InputActionChange change)
    {
        DetectInputDevice();
    }

    void DetectInputDevice()
    {
        if (InputDeviceDetector.Instance == null)
            return;

        if (keyboardMouseControlsPanel != null)
        {
            keyboardMouseControlsPanel.SetActive(InputDeviceDetector.Instance.IsKeyboardMouseActive);
        }

        if (controllerControlsPanel != null)
        {
            controllerControlsPanel.SetActive(InputDeviceDetector.Instance.IsGamepadActive);
        }
    }

    void UpdateControlsPanels()
    {
        if (InputDeviceDetector.Instance == null)
            return;

        if (keyboardMouseControlsPanel != null)
        {
            keyboardMouseControlsPanel.SetActive(InputDeviceDetector.Instance.IsKeyboardMouseActive);
        }

        if (controllerControlsPanel != null)
        {
            controllerControlsPanel.SetActive(InputDeviceDetector.Instance.IsGamepadActive);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        // Cursor.lockState = isPaused ? CursorLockMode.Locked : CursorLockMode.None;

        if (isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
            // Set first button as selected for gamepad/controller navigation
            if (firstPauseButton != null && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(firstPauseButton.gameObject);
            }
        }
        // Update control panels when pausing
        UpdateControlsPanels();
        Debug.Log("Game Paused");
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        Time.timeScale = 1f;
        isPaused = false;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        // Clear selected gameobject so gamepad navigation works during gameplay
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        Debug.Log("Game Resumed");
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public bool IsPaused => isPaused;
}

