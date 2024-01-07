using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic _backgroundMusicInstance = null;

    public static BackgroundMusic BackgroundMusicInstance
    {
        get { return _backgroundMusicInstance; }
    }

    private void Awake()
    {
        if (_backgroundMusicInstance != null && _backgroundMusicInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        _backgroundMusicInstance = this;
        DontDestroyOnLoad(gameObject);
    }
}
