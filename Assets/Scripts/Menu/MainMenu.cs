using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
