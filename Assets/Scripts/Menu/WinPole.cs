using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPole : MonoBehaviour
{
    [SerializeField]
    private GameObject _winPanel;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_winPanel.activeSelf && other.gameObject.CompareTag("Player"))
        {
            _winPanel.SetActive(true);
            _audioSource.Play();
            StartCoroutine(LoadLevelAfterDelay());
        }
    }

    private IEnumerator LoadLevelAfterDelay()
    {
        yield return new WaitForSeconds(7);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }
}
