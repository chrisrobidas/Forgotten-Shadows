using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class FPSCounter : MonoBehaviour
{
    [SerializeField] 
    private float _hudRefreshRate = 1f;

    private TMP_Text _fpsValueText;

    private float _timer;

    private void Start()
    {
        _fpsValueText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsValueText.text = "" + fps;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }
}
