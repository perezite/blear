using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    // the effects
    private LevelResultScreenEffects effects;

    // was the reclaim life button clicked
    private bool isReclaimLifeButtonClicked = false;

    // was the game over button clicked
    private bool isGameOverButtonClicked = false;

    // the reclaim life button
    private Button reclaimLifeButton;

    // activate the screen
    public void Activate()
    {
        gameObject.SetActive(true);
        reclaimLifeButton = transform.GetAllChildrenRecursively().Single(x => x.tag == "LoseScreenReclaimLifeButton").GetComponent<Button>();
        StartCoroutine(effects.WaitFadeAndGrow());
        reclaimLifeButton.interactable = false;
        StartCoroutine(LazyUpdate());
    }

    public bool IsReclaimLifeButtonClicked()
    {
        return isReclaimLifeButtonClicked;
    }

    public bool IsGameOverButtonClicked()
    {
        return isGameOverButtonClicked;
    }

    public void Reset()
    {
        isReclaimLifeButtonClicked = false;
        isGameOverButtonClicked = false;
        reclaimLifeButton.interactable = true;
        effects.Reset();
    }

    public void OnReclaimLifeButtonClicked()
    {
        StartCoroutine(OnReclaimLifeButtonClickedInternal());
    }

    public void OnGameOverButtonClicked()
    {
        StartCoroutine(OnGameOverButtonClickedInternal());
    }

    private IEnumerator OnReclaimLifeButtonClickedInternal()
    {
        // if (AdManager.GetInstance().IsAdvertisementReady(AdManager.RewardedVideoZone))
        // {
            // yield return StartCoroutine(AdManager.GetInstance().ShowAd(AdManager.RewardedVideoZone, showInPremiumVersion: true));
            // if (AdManager.GetInstance().GetLastShowResult() == ShowResult.Finished)
            // { 
                // yield return StartCoroutine(effects.PlayShrinkAnimation());
                // effects.RevealGameObjects();
                // isReclaimLifeButtonClicked = true;
            // }
        // }
		
		yield return null;
    }

    private IEnumerator OnGameOverButtonClickedInternal()
    {
        yield return StartCoroutine(effects.PlayShrinkAnimation());
        isGameOverButtonClicked = true;
    }

    private IEnumerator LazyUpdate()
    {
        while (true)
        {
            // reclaimLifeButton.interactable = AdManager.GetInstance().IsAdvertisementReady(AdManager.RewardedVideoZone);  
            yield return new WaitForSeconds(1.5f);
        }
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
