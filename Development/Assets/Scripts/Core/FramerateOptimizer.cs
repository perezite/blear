using UnityEngine;

// This is yet antoher attempt to fix the jerky ball movement problem.
// This class adjusts the time settings to ensure a smooth movement if there are unsteady framerates
public class FramerateOptimizer : MonoBehaviour
{
    [Tooltip("factor for smoothing the velocity measurement (1 = no smoothing, 0 = infinite smoothing)")]
    public float SmoothFactor = 1f;

    // previous delta time
    private float previousDeltaTime;

    // smoothed delta time
    private float smoothedDeltaTime;

    // original fixed delta time
    private float originalFixedDeltaTime;

    // original maximum delta time
    private float originalMaximumDeltaTime;

	// is the framerate optimizer enabled
	private bool isEnabled = false;

    private void OnEnable()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
        originalMaximumDeltaTime = Time.maximumDeltaTime;
    }

    private void OnDisable()
    {
		if (!isEnabled) 
		{
			return;
		}
				
		Time.fixedDeltaTime = originalFixedDeltaTime;
		Time.maximumDeltaTime = originalMaximumDeltaTime;
    }

    private void Start()
    {
		if (!isEnabled) 
		{
			return;
		}
		
        previousDeltaTime = Time.deltaTime;
        smoothedDeltaTime = Time.deltaTime;

        // init smooth factor
        if (SmoothFactor <= 0f || SmoothFactor > 1)
        {
            throw new System.Exception("SmoothFactor must be larger than 0 and smaller than or equal to 1!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
		if (!isEnabled) 
		{
			return;
		}
		
        // track
        smoothedDeltaTime += (Time.deltaTime - previousDeltaTime) * SmoothFactor;
        previousDeltaTime = Time.deltaTime;

        // adjust
        Time.fixedDeltaTime = smoothedDeltaTime;
        Time.maximumDeltaTime = 1.2f * smoothedDeltaTime;
    }
}
