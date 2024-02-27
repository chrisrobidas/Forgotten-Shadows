using UnityEngine;
using UnityEngine.EventSystems;

public abstract class NavigableMenu : MonoBehaviour
{
    [SerializeField] private GameObject DefaultSelection;

    public void SelectDefaultGameObject()
    {
        EventSystem.current.SetSelectedGameObject(DefaultSelection);
    }
}
