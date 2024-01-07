using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseCanvas.activeSelf)
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
        _pauseCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        _pauseCanvas.SetActive(false);
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
        Application.Quit();
    }
}
