using System;
using System.Collections;
using UnityEngine;

// animator component extension for menu buttons
public class MenuElementAnimator : MonoBehaviour
{
    // grow animation completed action
    public Action GrowAnimationCompleted;

    // IsVisible animator property
    private const string IsVisibleAnimatorProperty = "IsVisible";

    // is the grow animation clip completed
    private bool isGrowAnimationClipCompleted = false;

    // is the shrink animation clip completed
    private bool isShrinkAnimationClipCompleted = false;

    // the extended animator
    private Animator animator;

    // set animation speed
    public void SetAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }

    // Called by animation clip
    public void OnGrowAnimationClipCompleted()
    {
        isGrowAnimationClipCompleted = true;
    }

    // Called by animation clip
    public void OnShrinkAnimationClipCompleted()
    {
        isShrinkAnimationClipCompleted = true;
    }

    // start the shrink animation clip
    public IEnumerator Shrink()
    {
        animator.SetBool(IsVisibleAnimatorProperty, false);
        isShrinkAnimationClipCompleted = false;

        while (!isShrinkAnimationClipCompleted)
        {
            yield return null;
        }
    }

    public IEnumerator Grow()
    {
        animator.SetBool(IsVisibleAnimatorProperty, true);
        isGrowAnimationClipCompleted = false;

        while (!isGrowAnimationClipCompleted)
        {
            yield return null;
        }

        if (GrowAnimationCompleted != null)
        {
            GrowAnimationCompleted();
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}