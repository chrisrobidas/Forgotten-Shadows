using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image)), ExecuteInEditMode()]
public class HueSliderHandle : MonoBehaviour
{
    [SerializeField] private Slider _hueSlider;

    private Image _handle;

    private void Start()
    {
        _handle = GetComponent<Image>();
        _hueSlider.onValueChanged.AddListener(delegate { OnHueSliderValueChange(); });

        OnHueSliderValueChange();
    }

    private void OnHueSliderValueChange()
    {
        _handle.color = Color.HSVToRGB(_hueSlider.value, 1, 1);
    }
}
