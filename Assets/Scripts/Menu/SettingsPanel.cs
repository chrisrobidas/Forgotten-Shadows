using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _displayModeDropdown;

    [SerializeField]
    private TMP_Dropdown _resolutionDropdown;

    private List<Resolution> _filteredResolutions;

    private FullScreenMode _selectedDisplayMode;
    private Resolution _selectedResolution;

    public void SetDisplayMode(int displayModeIndex)
    {
        _selectedDisplayMode = OptionIndexToDisplayMode(displayModeIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        _selectedResolution = _filteredResolutions[resolutionIndex];
    }

    public void ApplySettings()
    {
        Screen.SetResolution(_selectedResolution.width, _selectedResolution.height, _selectedDisplayMode);
    }

    private void Start()
    {
        FillDisplayModeOptions();
        FillResolutionOptions();
    }

    private void FillDisplayModeOptions()
    {
        List<string> displayModeOptions = new List<string>();
        displayModeOptions.Add("Full Screen Window");
        displayModeOptions.Add("Windowed");
        displayModeOptions.Add("Exclusive Full Screen");

        int currentDisplayModeIndex = DisplayModeToOptionIndex(Screen.fullScreenMode);

        _displayModeDropdown.ClearOptions();

        _displayModeDropdown.AddOptions(displayModeOptions);
        _displayModeDropdown.value = currentDisplayModeIndex;
        _displayModeDropdown.RefreshShownValue();
    }

    private void FillResolutionOptions()
    {
        Resolution[] availableResolutions = Screen.resolutions;
        RefreshRate currentRefreshRate = Screen.currentResolution.refreshRateRatio;
        _filteredResolutions = new List<Resolution>();
        _resolutionDropdown.ClearOptions();

        for (int i = availableResolutions.Length - 1; i >= 0; i--)
        {
            if (availableResolutions[i].refreshRateRatio.value == currentRefreshRate.value)
            {
                _filteredResolutions.Add(availableResolutions[i]);
            }
        }

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOption = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height + " " + (int)_filteredResolutions[i].refreshRateRatio.value + "Hz";
            resolutionOptions.Add(resolutionOption);
            if (_filteredResolutions[i].width == Screen.width && _filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(resolutionOptions);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    private int DisplayModeToOptionIndex(FullScreenMode fullScreenMode)
    {
        switch (fullScreenMode)
        {
            case FullScreenMode.FullScreenWindow:
                return 0;
            case FullScreenMode.Windowed:
                return 1;
            case FullScreenMode.ExclusiveFullScreen:
                return 2;
            default:
                return 0;
        }
    }

    private FullScreenMode OptionIndexToDisplayMode(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0:
                return FullScreenMode.FullScreenWindow;
            case 1:
                return FullScreenMode.Windowed;
            case 2:
                return FullScreenMode.ExclusiveFullScreen;
            default:
                return FullScreenMode.FullScreenWindow;
        }
    }
}
