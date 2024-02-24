using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image)), ExecuteInEditMode()]
public class ValueImage : MonoBehaviour
{
    [SerializeField]
    private Slider _hueSlider;

    [SerializeField]
    private Slider _saturationSlider;

    private Image _image;

    private const int TEXTURE_WIDTH = 100;

    private void Start()
    {
        _image = GetComponent<Image>();
        UpdateValueSprite();
        _hueSlider.onValueChanged.AddListener(delegate { OnHueOrSaturationSliderValueChange(); });
        _saturationSlider.onValueChanged.AddListener(delegate { OnHueOrSaturationSliderValueChange(); });
    }

    private void UpdateValueSprite()
    {
        float hue = _hueSlider.value;
        float saturation = _saturationSlider.value;

        Texture2D texture = new Texture2D(TEXTURE_WIDTH, 1);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.hideFlags = HideFlags.DontSave;

        Color32[] colors = new Color32[TEXTURE_WIDTH];

        for (int i = 0; i < TEXTURE_WIDTH; i++)
        {
            colors[i] = Color.HSVToRGB(hue, saturation, (float)i / TEXTURE_WIDTH);
        }

        texture.SetPixels32(colors);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, TEXTURE_WIDTH, 1), Vector2.zero);
        _image.sprite = sprite;
    }

    private void OnHueOrSaturationSliderValueChange()
    {
        UpdateValueSprite();
    }
}
