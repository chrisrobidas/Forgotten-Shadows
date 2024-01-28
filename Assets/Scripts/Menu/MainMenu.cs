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

    public void Play()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void ShowMainPanel()
    {
        _mainPanel.SetActive(true);
        _customizationPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }

    public void ShowCustomizationPanel()
    {
        _mainPanel.SetActive(false);
        _customizationPanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    public void ShowSettingsPanel()
    {
        _mainPanel.SetActive(false);
        _customizationPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMainPanel();
        }
    }
}
