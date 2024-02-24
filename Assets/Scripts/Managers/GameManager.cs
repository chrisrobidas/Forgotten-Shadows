using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGamePaused { get; private set; }

    public void SetIsGamePaused(bool isGamePaused)
    {
        IsGamePaused = isGamePaused;

        if (IsGamePaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void MainMenu()
    {
        SetIsGamePaused(false);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_NAME);
    }

    public void Play()
    {
        SetIsGamePaused(false);
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(Constants.TEST_SCENE_NAME);
    }

    public void Restart()
    {
        SetIsGamePaused(false);
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator LoadSceneAfterDelay(string sceneName, float delay, CursorLockMode cursorLockMode)
    {
        SetIsGamePaused(false);
        yield return new WaitForSeconds(delay);
        Cursor.lockState = cursorLockMode;
        SceneManager.LoadScene(sceneName);
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
