using System;
using System.Collections;
using UnityEngine;

public class AdManager : MonoBehaviour
{     
    // singleton instance
    private static AdManager instance = null;

    // the last negotiation result
    private bool lastNegotiationResult = false;

    // the last negotiation value
    private float? lastNegotiationValue = null;

    // is advertisement system in test mode
    // private bool isTestMode = true;

    // private ctor
    private AdManager()
    {
    }

    // singleton instance
    public static AdManager GetInstance()
    {
        if (instance == null)
        {
            var newGameObject = new GameObject();
            newGameObject.name = typeof(AdManager).Name;
            instance = newGameObject.AddComponent<AdManager>();
            DontDestroyOnLoad(newGameObject);
        }

        return instance;
    }

    // is the advertisement ready
    public bool IsAdvertisementReady(string zone = "")
    {
		return false;
    }
    
    // get the last negotiation result
    public bool GetLastNegotiationResult()
    {
        return lastNegotiationResult;
    }

    // get the last negotiation value
    public float? GetLastNegotiationValue()
    {
        return lastNegotiationValue;
    }

    // show an ad if possible
    public IEnumerator ShowAd(string zone = "", bool showInPremiumVersion = false)
    {
		yield break;
    }

    // negotiate if an ad shall be placed for the current ad placement opportunity
    public void NegotiateAdPlacement()
    {
		lastNegotiationResult = false;
    }

    // stop the main game until ad placement is completed
    private IEnumerator WaitForAdResult()
    {
        float currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return null;

        // while (Advertisement.isShowing)
        // {
            // yield return null;
        // }

        Time.timeScale = currentTimeScale;
    }

    private void Start()
    {
    }
}
