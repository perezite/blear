using System.Collections;
using UnityEngine;

// Welcome to the World of HACKS: This hacky script adjust the settings in the time manager to match the average framerate
// This is needed, because I update the ball using a rigidbody2D which gives jerky motions if the time settings are not properly tweaked 
public class TimeCorrector : MonoBehaviour
{
    // the total time elapsed
    private float totalTimeElapsed = 0f;

    // number of frames elapsed
    private long numberOfFramesElapsed = 0;

    // the average framerate
    private float averageFramesPerSecond = 0f;

    // is the framerate throttle enabled globally
    private bool isEnabledGlobally = false;

    private IEnumerator Start()
    {
        if (!isEnabledGlobally)
        {
            yield break;
        }

        // first, wait a few frames (maybe these frames load stuff or whatever..)
        for (int i = 0; i < 30; i++)
        {
            yield return null;
        }

        // track the first 100 frames and see, if we are at 30 hz or 60 hz screen refresh rate
        for (int i = 0; i < 100; i++)
        {
            totalTimeElapsed += 1 / Time.deltaTime;
            numberOfFramesElapsed++;
            averageFramesPerSecond = totalTimeElapsed / numberOfFramesElapsed;

            yield return null;
        }

        // change to 30 hz time settings if we are in this region
        // Debug.Log(averageFramesPerSecond);
        if (averageFramesPerSecond <= 35)
        {
            Time.maximumDeltaTime = 0.033333f;
            Time.fixedDeltaTime = 0.0333f;
        }
    }
}
