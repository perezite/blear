using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

// Extension for UI.Toggle element: swaps the background sprite of a toggle ui element depending on the select state
[RequireComponent(typeof(Toggle))]
public class MenuToggle : MenuElement
{
    [Tooltip("Delay in seconds before swapping the sprite")]
    public float Delay = 0.1f;

    [Tooltip("Background sprite if toggle is not selected")]
    public Sprite DeselectedSprite;

    [Tooltip("Background sprite if toggle is selected")]
    public Sprite SelectedSprite;

    // the extended toggle
    private Toggle toggle;

    // click animation complete handler
    public virtual void OnClickAnimationComplete()
    {
    }

    public virtual void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    // click handler
    protected override IEnumerator OnClickHandler()
    {
        StartCoroutine("SetSprite");
        yield return null;
    }

    private IEnumerator SetSprite()
    {
        yield return new WaitForSeconds(Delay);

        if (toggle.isOn == true)
        {
            toggle.image.sprite = SelectedSprite;
        }

        if (toggle.isOn == false)
        {
            toggle.image.sprite = DeselectedSprite;
        }

        OnClickAnimationComplete();
    }
}