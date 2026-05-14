using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] bool pauseOnEscape = true;
    [SerializeField] string mainMenuSceneName = "MainMenu";

    InputSystem_Actions inputActions;
    InputAction pauseAction;
    bool isPaused;

    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        pauseAction = inputActions.Player.Pause;
        pauseAction.performed += OnPausePressed;
    }

    void OnDisable()
    {
        pauseAction.performed -= OnPausePressed;
        inputActions.Disable();
    }

    void OnPausePressed(InputAction.CallbackContext context)
    {
        if (pauseOnEscape)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

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
        Time.timeScale = 0f;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        Debug.Log("Game Paused");
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
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

