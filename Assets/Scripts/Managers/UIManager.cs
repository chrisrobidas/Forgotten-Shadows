using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _customizationMenuPanel;
    [SerializeField] private GameObject _settingsMenuPanel;
    [SerializeField] private GameObject _winMenuPanel;

    [SerializeField] private GameObject _globalVolume;

    private List<GameObject> _allPanels;

    public void OnResumeInput(CallbackContext context)
    {
        if (context.started)
        {
            Resume();
        }
    }

    public void OnBackInput(CallbackContext context)
    {
        if (context.started)
        {
            Back();
        }
    }

    public void Resume()
    {
        if (_pauseMenuPanel == null) return;

        HideAllPanels();
        _globalVolume.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.SetIsGamePaused(false);
        Time.timeScale = 1.0f;
    }

    public void Back()
    {
        if (_mainMenuPanel != null && !_mainMenuPanel.activeSelf)
        {
            ShowMainMenuPanel();
        }
        else if (_pauseMenuPanel != null && !_pauseMenuPanel.activeSelf)
        {
            ShowPauseMenuPanel();
        }
        else if (_pauseMenuPanel != null)
        {
            Resume();
        }
    }

    public void ShowMainMenuPanel()
    {
        ShowPanel(_mainMenuPanel);
        _globalVolume.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.SetIsGamePaused(true);
        Time.timeScale = 1.0f;
    }

    public void ShowPauseMenuPanel()
    {
        ShowPanel(_pauseMenuPanel);
        _globalVolume.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.SetIsGamePaused(true);
        Time.timeScale = 0.0f;
    }

    public void ShowCustomizationMenuPanel()
    {
        ShowPanel(_customizationMenuPanel);
        _globalVolume.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.SetIsGamePaused(true);
        Time.timeScale = 1.0f;
    }

    public void ShowSettingsMenuPanel()
    {
        ShowPanel(_settingsMenuPanel);
        _globalVolume.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.SetIsGamePaused(true);
        Time.timeScale = 0.0f;
    }

    public void ShowWinMenuPanel()
    {
        ShowPanel(_winMenuPanel);
        _globalVolume.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.SetIsGamePaused(false);
        Time.timeScale = 1.0f;
    }

    private void ShowPanel(GameObject panelToShow)
    {
        HideAllPanels();
        panelToShow.SetActive(true);

        NavigableMenu navigableMenuComponent = panelToShow.GetComponent<NavigableMenu>();
        if (navigableMenuComponent != null)
        {
            navigableMenuComponent.SelectDefaultGameObject();
        }
    }

    private void HideAllPanels()
    {
        foreach (GameObject panelToHide in _allPanels)
        {
            if (panelToHide != null)
            {
                panelToHide.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        _allPanels = new List<GameObject>
        {
            _mainMenuPanel,
            _pauseMenuPanel,
            _customizationMenuPanel,
            _settingsMenuPanel,
            _winMenuPanel
        };
    }
}
