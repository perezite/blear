using System.Collections;
using UnityEngine;

public class WobbleEffect : MonoBehaviour
{
    [Tooltip("Minimal effect frequency in periods per second")]
    public float MinimalFrequency = 2f;

    [Tooltip("Effect frequency in periods per second")]
    public float MaximalFrequency = 1f;

    [Tooltip("Minimal frequency duration")]
    public float MinimalFrequencyDuration = 2f;

    [Tooltip("Maximal frequency duration")]
    public float MaximalFrequencyDuration = 4;

    [Tooltip("Minimal percentage the up scale can attain")]
    public float MinimalUpScalePercentage = 0.75f;

    [Tooltip("Minimal percentage the left scale can attain")]
    public float MinimalLeftScalePercentage = 0.95f;

    // balls original scale
    private Vector3 originalScale;

    // current frquency
    private float currentFrequency;

    // duration of current frequency domain
    private float currentFrequencyDuration;

    // time elapsed in current frequency domain
    private float timeElapsedInCurrentFrequency;

    // current angle
    private float alpha;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    // Use this for initialization
    private void Start()
    {
        // setup variables
        currentFrequencyDuration = Random.Range(MinimalFrequencyDuration, MaximalFrequencyDuration);
        currentFrequency = Random.Range(MinimalFrequency, MaximalFrequency);
        timeElapsedInCurrentFrequency = 0f;
        alpha = 0f;
    }

    private void OnDisable()
    {
        transform.localScale = originalScale;
    }

    private void Update()
    {
        // update the target frequency
        timeElapsedInCurrentFrequency += Time.deltaTime;
        if (timeElapsedInCurrentFrequency > currentFrequencyDuration)
        {
            timeElapsedInCurrentFrequency = 0f;
            currentFrequency = Random.Range(MinimalFrequency, MaximalFrequency);
        }

        // scale the ball
        float minimalUpScale = MinimalUpScalePercentage * originalScale.y;
        float minimalLeftScale = MinimalLeftScalePercentage * originalScale.x;
        alpha = alpha + (currentFrequency * Time.deltaTime);
        alpha = Mathf.Repeat(alpha, 2f * Mathf.PI);
        float multiplier = (Mathf.Sin(alpha) + 1f) * 0.5f;
        float currentUpScale = Mathf.Lerp(minimalUpScale, originalScale.y, multiplier);
        float currentLeftScale = Mathf.Lerp(minimalLeftScale, originalScale.x, multiplier);

        transform.localScale = new Vector3(currentLeftScale, currentUpScale, transform.localScale.z);
    }
}
