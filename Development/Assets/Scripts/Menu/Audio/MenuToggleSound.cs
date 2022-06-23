using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuToggleSound : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("The sounds")]
    public SoundSource2D Sound;

    // the toggle
    private Toggle toggle;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (toggle.isOn)
        {
            Sound.ForcePlay();
        }
    }

    private void Start()
    {
        toggle = GetComponent<Toggle>();
    }
}
