using UnityEngine;

// This class forces the loading of the AdManager, because the first startup is quite expensive
// The class can then be added to a level where the loading time is not noticed by the user (for example in a loading or splash screen)
public class AdManagerPreloader : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        var lastNegotiationResult = AdManager.GetInstance().GetLastNegotiationResult();
        if (lastNegotiationResult == true)
        {
            // nothing to do. We just made the if-statement to prevent compiler warnings
        }
    }
}
