using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image)), ExecuteInEditMode()]
public class SaturationValueImage : MonoBehaviour
{
    [SerializeField]
    private Slider _hueSlider;

    private Image _image;

    private const int TEXTURE_WIDTH = 128;
    private const int TEXTURE_HEIGHT = 128;

    private void Start()
    {
        _image = GetComponent<Image>();
        UpdateSaturationValueSprite();
        _hueSlider.onValueChanged.AddListener(delegate { OnHueSliderValueChange(); });
    }

    private void UpdateSaturationValueSprite()
    {
        float hue = _hueSlider.value;

        Texture2D texture = new Texture2D(TEXTURE_WIDTH, TEXTURE_HEIGHT);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.hideFlags = HideFlags.DontSave;

        for (int saturationIndex = 0; saturationIndex < TEXTURE_WIDTH; saturationIndex++)
        {
            Color[] colors = new Color[TEXTURE_HEIGHT];
            for (int valueIndex = 0; valueIndex < TEXTURE_HEIGHT; valueIndex++)
            {
                colors[valueIndex] = Color.HSVToRGB(hue, (float)saturationIndex / TEXTURE_WIDTH, (float)valueIndex / TEXTURE_HEIGHT);
            }
            texture.SetPixels(saturationIndex, 0, 1, TEXTURE_HEIGHT, colors);
        }
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, TEXTURE_WIDTH, TEXTURE_HEIGHT), Vector2.zero);
        _image.sprite = sprite;
    }

    private void OnHueSliderValueChange()
    {
        UpdateSaturationValueSprite();
    }
}
