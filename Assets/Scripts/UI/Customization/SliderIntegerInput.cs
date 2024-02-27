using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField)), ExecuteInEditMode()]
public class SliderIntegerInput : MonoBehaviour
{
    [SerializeField] private int _maxValue;
    [SerializeField] private Slider _slider;

    private TMP_InputField _integerInputField;
    private bool _ignoreNotify;
    
    public void OnIntegerInputValueChange()
    {
        if (_integerInputField.text == "" || _integerInputField.text == "-")
        {
            _integerInputField.SetTextWithoutNotify("0");
        }

        _integerInputField.SetTextWithoutNotify(AdjustIntegerValue(int.Parse(_integerInputField.text)).ToString());

        _ignoreNotify = true;
        _slider.value = float.Parse(_integerInputField.text) / _maxValue;
    }

    private void Start()
    {
        _integerInputField = GetComponent<TMP_InputField>();
        _slider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });

        OnSliderValueChange();
    }

    private void OnSliderValueChange()
    {
        if (_ignoreNotify)
        {
            _ignoreNotify = false;
        }
        else
        {
            _integerInputField.SetTextWithoutNotify(((int)(_slider.value * _maxValue)).ToString());
        }
    }

    private int AdjustIntegerValue(int value)
    {
        if (value < 0)
        {
            return 0;
        }
        else if (value > _maxValue)
        {
            return _maxValue;
        }
        else
        {
            return value;
        }
    }
}
