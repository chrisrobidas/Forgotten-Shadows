using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image)), ExecuteInEditMode()]
public class SaturationValueSliderHandle : MonoBehaviour
{
    [SerializeField] private Slider _hueSlider;
    [SerializeField] private Slider _saturationSlider;
    [SerializeField] private Slider _valueSlider;

    private Image _handle;

    private void Start()
    {
        _handle = GetComponent<Image>();
        _hueSlider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });
        _saturationSlider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });
        _valueSlider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });

        OnSliderValueChange();
    }

    private void OnSliderValueChange()
    {
        _handle.color = Color.HSVToRGB(_hueSlider.value, _saturationSlider.value, _valueSlider.value);
    }
}
