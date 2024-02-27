using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGamePaused { get; private set; }

    public void SetIsGamePaused(bool isGamePaused)
    {
        if (isGamePaused == IsGamePaused) return;

        IsGamePaused = isGamePaused;

        if (IsGamePaused)
        {
            InputManager.Instance.PlayerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            InputManager.Instance.PlayerInput.SwitchCurrentActionMap("Player");
        }
    }

    public void MainMenu()
    {
        SetIsGamePaused(true);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_NAME);
    }

    public void Play()
    {
        SetIsGamePaused(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(Constants.TEST_SCENE_NAME);
    }

    public void Restart()
    {
        SetIsGamePaused(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator LoadSceneAfterDelay(string sceneName, bool gameIsPaused, float delay, CursorLockMode cursorLockMode)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
        SetIsGamePaused(gameIsPaused);
        Cursor.lockState = cursorLockMode;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
