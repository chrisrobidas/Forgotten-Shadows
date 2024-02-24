using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        GameManager.Instance.Play();
    }

    public void Customize()
    {
        UIManager.Instance.ShowCustomizationMenuPanel();
    }

    public void Settings()
    {
        UIManager.Instance.ShowSettingsMenuPanel();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
