using System.Collections.Generic;
using UnityEngine;

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

    public void Resume()
    {
        HideAllPanels();
        _globalVolume.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.SetIsGamePaused(false);
    }

    public void ShowMainMenuPanel()
    {
        ShowPanel(_mainMenuPanel);
        _globalVolume.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.SetIsGamePaused(false);
    }

    private void ShowPauseMenuPanel()
    {
        ShowPanel(_pauseMenuPanel);
        _globalVolume.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.SetIsGamePaused(true);
    }

    public void ShowCustomizationMenuPanel()
    {
        ShowPanel(_customizationMenuPanel);
        _globalVolume.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.SetIsGamePaused(false);
    }

    public void ShowSettingsMenuPanel()
    {
        ShowPanel(_settingsMenuPanel);
        _globalVolume.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.SetIsGamePaused(true);
    }

    public void ShowWinMenuPanel()
    {
        ShowPanel(_winMenuPanel);
        _globalVolume.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.SetIsGamePaused(false);
    }

    public void PauseOrBackSettings()
    {
        if (_settingsMenuPanel.activeSelf)
        {
            if (_mainMenuPanel != null)
            {
                ShowMainMenuPanel();
            }
            else
            {
                ShowPauseMenuPanel();
            }
        }
        else if (_pauseMenuPanel != null)
        {
            if (_pauseMenuPanel.activeSelf)
            {
                Resume();
            }
            else
            {
                ShowPauseMenuPanel();
            }
        }
    }

    private void ShowPanel(GameObject panelToShow)
    {
        HideAllPanels();
        panelToShow.SetActive(true);
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
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // TODO: Move the input handling in PlayerController, maybe in 2 separate methods/input profiles, one for the main menu and one for the play scenes
            PauseOrBackSettings();
        }
    }
}
