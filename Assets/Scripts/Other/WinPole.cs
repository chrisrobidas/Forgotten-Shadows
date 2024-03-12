using UnityEngine;

public class WinPole : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool _hasWin;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasWin && (other.gameObject.CompareTag(Constants.PLAYER_TAG) || other.gameObject.CompareTag(Constants.PLAYER_FOOT_TAG)))
        {
            UIManager.Instance.ShowWinMenuPanel();
            _audioSource?.Play();
            StartCoroutine(GameManager.Instance.LoadSceneAfterDelay(Constants.MAIN_MENU_SCENE_NAME, true, 7, CursorLockMode.None));
            _hasWin = true;
        }
    }
}
