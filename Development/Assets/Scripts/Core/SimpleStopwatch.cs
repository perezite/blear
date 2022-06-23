using System.Diagnostics;

// A simple stopwatch for querying a countdown
public class SimpleStopwatch
{
    private float countdownInSeconds;

    private Stopwatch stopwatch;

    public SimpleStopwatch(float countdownInSeconds = 0f)
    {
        stopwatch = new Stopwatch();
        stopwatch.Reset();
        SetCountdownInSeconds(countdownInSeconds);
    }

    // Tells if the interval has expired. The first call starts the time measurement.
    public bool HasCountdownExpired()
    {
        stopwatch.Start();
        bool hasExpired = stopwatch.Elapsed.TotalSeconds >= countdownInSeconds;
        if (hasExpired == true)
        {
            stopwatch.Reset();
        }

        return hasExpired;
    }

    // Set the expiration interval. Must be called before querying stopwatch
    public void SetCountdownInSeconds(float expirationIntervalInSeconds)
    {
        if (stopwatch.IsRunning)
        {
            throw new System.InvalidOperationException("The countdown must be set before the first query on the stopwatch");
        }

        countdownInSeconds = expirationIntervalInSeconds;
    }
}