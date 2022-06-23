using System.Collections;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    // the effects
    private LevelResultScreenEffects effects;

    // was home button clicked
    private bool wasHomeButtonClicked = false;

    // was proceed button clicked
    private bool wasProceedButtonClicked = false;

    // was play again button clicked
    private bool wasPlayAgainButtonClicked = false;

    // activate the screen
    public void Activate()
    {
        gameObject.SetActive(true);
        AdManager.GetInstance().NegotiateAdPlacement();
        StartCoroutine(effects.WaitFadeAndGrow());
    }

    public bool WasHomeButtonClicked()
    {
        return wasHomeButtonClicked;
    }

    public bool WasProceedButtonClicked()
    {
        return wasProceedButtonClicked;
    }

    public bool WasPlayAgainButtonClicked()
    {
        return wasPlayAgainButtonClicked;
    }

    public void OnHomeButtonClicked()
    {
        StartCoroutine(OnHomeButtonClickedInternal());
    }

    public void OnProceedButtonClicked()
    {
        StartCoroutine(OnProceedButtonClickedInternal());
    }

    public void OnPlayAgainButtonClicked()
    {
        StartCoroutine(OnPlayAgainButtonClickedInternal());
    }

    private IEnumerator OnHomeButtonClickedInternal()
    {
        yield return StartCoroutine(effects.PlayShrinkAnimation());
        wasHomeButtonClicked = true;
    }

    private IEnumerator OnProceedButtonClickedInternal()
    {
        yield return StartCoroutine(effects.PlayShrinkAnimation());
        if (AdManager.GetInstance().GetLastNegotiationResult() == true)
        {
            // yield return StartCoroutine(AdManager.GetInstance().ShowAd(AdManager.VideoZone));
			yield return null;
        }

        wasProceedButtonClicked = true;
    }

    private IEnumerator OnPlayAgainButtonClickedInternal()
    {
        yield return StartCoroutine(effects.PlayShrinkAnimation());
        if (AdManager.GetInstance().GetLastNegotiationResult() == true)
        {
            // yield return StartCoroutine(AdManager.GetInstance().ShowAd(AdManager.VideoZone));
			yield return null;
        }

        wasPlayAgainButtonClicked = true;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        effects = GetComponent<LevelResultScreenEffects>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
