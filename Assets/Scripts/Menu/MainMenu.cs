using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainPanel;

    [SerializeField]
    private GameObject _customizationPanel;

    [SerializeField]
    private GameObject _settingsPanel;

    [SerializeField]
    private GameObject _globalVolume;

    public void Play()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void ShowMainPanel()
    {
        Time.timeScale = 1.0f;
        _mainPanel.SetActive(true);
        _customizationPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _globalVolume.SetActive(false);
    }

    public void ShowCustomizationPanel()
    {
        Time.timeScale = 1.0f;
        _mainPanel.SetActive(false);
        _customizationPanel.SetActive(true);
        _settingsPanel.SetActive(false);
        _globalVolume.SetActive(false);
    }

    public void ShowSettingsPanel()
    {
        _mainPanel.SetActive(false);
        _customizationPanel.SetActive(false);
        _settingsPanel.SetActive(true);
        _globalVolume.SetActive(true);
        Time.timeScale = 0.0f;
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
            ShowMainPanel();
        }
    }
}
