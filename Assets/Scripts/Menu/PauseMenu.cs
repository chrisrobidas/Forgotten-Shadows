using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _pausePanel;

    [SerializeField]
    private GameObject _settingsPanel;

    [SerializeField]
    private GameObject _globalVolume;

    public void ToggleSettings()
    {
        _pausePanel.SetActive(!_pausePanel.activeSelf);
        _settingsPanel.SetActive(!_settingsPanel.activeSelf);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        _pausePanel.SetActive(false);
        _globalVolume.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TestScene");
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_settingsPanel.activeSelf)
            {
                _pausePanel.SetActive(true);
                _globalVolume.SetActive(true);
                _settingsPanel.SetActive(false);
            }
            else if (_pausePanel.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        Time.timeScale = 0.0f;
        _pausePanel.SetActive(true);
        _globalVolume.SetActive(true);
        _settingsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }
}
