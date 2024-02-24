using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image)), ExecuteInEditMode()]
public class HueImage : MonoBehaviour
{
    private Image _image;

    private const int TEXTURE_WIDTH = 360;

    private void Start()
    {
        _image = GetComponent<Image>();
        UpdateHueSprite();
    }

    private void UpdateHueSprite()
    {
        Texture2D texture = new Texture2D(TEXTURE_WIDTH, 1);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.hideFlags = HideFlags.DontSave;

        Color32[] colors = new Color32[TEXTURE_WIDTH];

        for (int i = 0; i < TEXTURE_WIDTH; i++)
        {
            colors[i] = Color.HSVToRGB((float)i / 360, 1, 1);
        }

        texture.SetPixels32(colors);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, TEXTURE_WIDTH, 1), Vector2.zero);
        _image.sprite = sprite;
    }
}
