using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAnimator : MonoBehaviour
{
    [Tooltip("Menu element animators. Animations are applied in enlisted order.")]
    public MenuElementAnimator[] MenuElementAnimators;

    [Tooltip("Animation speed")]
    public float AnimationSpeed = 1f;

    // perform shrining animations. Gets called by UI element
    public void Shrink(MenuElement caller)
    {
        StartCoroutine(ShrinkInternal(caller));
    }

    // perform shrinking animations. Gets called by UI element
    public IEnumerator ShrinkInternal(MenuElement caller)
    {
        // make all buttons non-interactable
        var originallyInteractableSelectables = SetAllSelectablesNonInteractable();

        // shrink the UI elements
        foreach (var menuElementAnimator in MenuElementAnimators.Reverse())
        {
            yield return StartCoroutine(menuElementAnimator.Shrink());
        }

        // reset interactability of the elements
        originallyInteractableSelectables.ForEach(x => x.interactable = true);

        // perform actual click action on calling menu element
        caller.OnClick();
    }

    // perform growing animations
    private IEnumerator Grow()
    {
        // make all menu elements non-interactable
        var originallyInteractableSelectables = SetAllSelectablesNonInteractable();

        // grow the UI elements
        foreach (var menuElementAnimator in MenuElementAnimators)
        {
            yield return StartCoroutine(menuElementAnimator.Grow());
        }

        // make all the buttons interactable
        originallyInteractableSelectables.ForEach(x => x.interactable = true);
    }

    // Use this for initialization
    private void Start()
    {
        MenuElementAnimators.ToList().ForEach(x => x.SetAnimationSpeed(AnimationSpeed));

        StartCoroutine(Grow());
    }

    private System.Collections.Generic.List<Selectable> SetAllSelectablesNonInteractable()
    {
        var allSelectables = MenuElementAnimators.SelectMany(x => x.transform.GetComponentsInChildren<Selectable>()).ToList();
        var originallyInteractableSelectables = allSelectables.Where(x => x.interactable == true).ToList();
        allSelectables.ForEach(x => x.interactable = false);
        return originallyInteractableSelectables;
    }
}