using UnityEngine;
using UnityEngine.UI;

public class CustomizationPanel : MonoBehaviour
{
    [SerializeField]
    private CharacterCustomizationApply _characterCustomizationApply;

    [SerializeField]
    private Slider _bodyHueSlider;

    [SerializeField]
    private Slider _bodySaturationSlider;

    [SerializeField]
    private Slider _bodyValueSlider;

    [SerializeField]
    private Slider _eyesHueSlider;

    [SerializeField]
    private Slider _eyesSaturationSlider;

    [SerializeField]
    private Slider _eyesValueSlider;

    public void Save()
    {
        PlayerPrefs.SetFloat("BodyHueValue", _bodyHueSlider.value);
        PlayerPrefs.SetFloat("BodySaturationValue", _bodySaturationSlider.value);
        PlayerPrefs.SetFloat("BodyValueValue", _bodyValueSlider.value);

        PlayerPrefs.SetFloat("EyesHueValue", _eyesHueSlider.value);
        PlayerPrefs.SetFloat("EyesSaturationValue", _eyesSaturationSlider.value);
        PlayerPrefs.SetFloat("EyesValueValue", _eyesValueSlider.value);
    }

    public void LoadSlidersValues()
    {
        _bodyHueSlider.value = PlayerPrefs.GetFloat("BodyHueValue", 0);
        _bodySaturationSlider.value = PlayerPrefs.GetFloat("BodySaturationValue", 0);
        _bodyValueSlider.value = PlayerPrefs.GetFloat("BodyValueValue", 1);

        _eyesHueSlider.value = PlayerPrefs.GetFloat("EyesHueValue", 0);
        _eyesSaturationSlider.value = PlayerPrefs.GetFloat("EyesSaturationValue", 1);
        _eyesValueSlider.value = PlayerPrefs.GetFloat("EyesValueValue", 0);
    }

    private void Start()
    {
        _bodyHueSlider.onValueChanged.AddListener(delegate { OnBodySliderValueChange(); });
        _bodySaturationSlider.onValueChanged.AddListener(delegate { OnBodySliderValueChange(); });
        _bodyValueSlider.onValueChanged.AddListener(delegate { OnBodySliderValueChange(); });

        _eyesHueSlider.onValueChanged.AddListener(delegate { OnEyesSliderValueChange(); });
        _eyesSaturationSlider.onValueChanged.AddListener(delegate { OnEyesSliderValueChange(); });
        _eyesValueSlider.onValueChanged.AddListener(delegate { OnEyesSliderValueChange(); });

        LoadSlidersValues();
    }

    private void OnBodySliderValueChange()
    {
        _characterCustomizationApply.BodyPartsMaterialInstance.color = Color.HSVToRGB(_bodyHueSlider.value, _bodySaturationSlider.value, _bodyValueSlider.value);
    }

    private void OnEyesSliderValueChange()
    {
        _characterCustomizationApply.EyesMaterialInstance.color = Color.HSVToRGB(_eyesHueSlider.value, _eyesSaturationSlider.value, _eyesValueSlider.value);
    }
}
