public class PauseMenu : NavigableMenu
{
    public void Resume()
    {
        UIManager.Instance.Resume();
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    }

    public void Settings()
    {
        UIManager.Instance.ShowSettingsMenuPanel();
    }

    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
