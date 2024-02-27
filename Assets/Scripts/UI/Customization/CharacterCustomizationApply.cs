using UnityEngine;

public class CharacterCustomizationApply : MonoBehaviour
{
    [HideInInspector] public Material BodyPartsMaterialInstance;
    [HideInInspector] public Material EyesMaterialInstance;

    [SerializeField] private Renderer[] _bodyPartsRenderers;
    [SerializeField] private Renderer[] _eyesRenderers;

    [SerializeField] private Material _bodyMaterial;
    [SerializeField] private Material _eyesMaterial;

    public void ApplyCustomization()
    {
        BodyPartsMaterialInstance.color = Color.HSVToRGB(
            PlayerPrefs.GetFloat("BodyHueValue", 0),
            PlayerPrefs.GetFloat("BodySaturationValue", 0),
            PlayerPrefs.GetFloat("BodyValueValue", 1)
        );

        EyesMaterialInstance.color = Color.HSVToRGB(
            PlayerPrefs.GetFloat("EyesHueValue", 0),
            PlayerPrefs.GetFloat("EyesSaturationValue", 1),
            PlayerPrefs.GetFloat("EyesValueValue", 0)
        );
    }

    private void Start()
    {
        PrepareBodyMaterial();
        PrepareEyesMaterial();
        ApplyCustomization();
    }

    private void PrepareBodyMaterial()
    {
        BodyPartsMaterialInstance = Instantiate(_bodyMaterial);

        foreach (Renderer renderer in _bodyPartsRenderers)
        {
            renderer.material = BodyPartsMaterialInstance;
        }
    }

    private void PrepareEyesMaterial()
    {
        EyesMaterialInstance = Instantiate(_eyesMaterial);

        foreach (Renderer renderer in _eyesRenderers)
        {
            renderer.material = EyesMaterialInstance;
        }
    }
}
