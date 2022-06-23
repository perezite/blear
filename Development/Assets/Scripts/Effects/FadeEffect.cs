using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// a fade effect between levels
// reference: https://unity3d.com/de/learn/tutorials/topics/graphics/fading-between-scenes
public class FadeEffect : MonoBehaviour
{
    [Tooltip("The fade texture. Can for example be a 2x2 texture with wrap mode=clamp, filtermode=point, format=16bits")]
    public Texture2D FadeTexture;

    [Tooltip("The level fade in duration in seconds")]
    public float LevelFadeInDuration = 1f;

    [Tooltip("If activated, fade in is only applied once for this level")]
    public bool IsOneShot = false;

    // map of already faded levels
    private static List<int> fadedLevels = new List<int>();

    // the instance
    private static FadeEffect instance = null;

    // the current fade texture alpha value
    private float fadeTextureAlpha = 1f;

    // is fading
    private bool isFading = false;

    // the current fade direction
    private int currentFadeDirection = 0;

    // cheesy singleton instance
    public static FadeEffect GetInstance()
    {
        return instance;
    }

    // fade the level in (fadeDirection = -1) or out (fadeDirection = 1) 
    public IEnumerator FadeLevel(int fadeDirection, float fadeDuration = 0.25f)
    {
        currentFadeDirection = fadeDirection;
        var elapsedTime = 0f;
        isFading = true;

        // fade
        while (elapsedTime <= fadeDuration)
        {
            fadeTextureAlpha += fadeDirection * Time.deltaTime / fadeDuration;
            fadeTextureAlpha = Mathf.Clamp01(fadeTextureAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isFading = false;
    } 

    private void OnGUI()
    {
        if (isFading || currentFadeDirection == 1)
        {
            UnityEngine.GUI.color = new Color(UnityEngine.GUI.color.r, UnityEngine.GUI.color.g, UnityEngine.GUI.color.b, fadeTextureAlpha);
            UnityEngine.GUI.depth = -1000;
            UnityEngine.GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);
        }
    }

    private void Awake()
    {
        // assert singleton
        Debug.Assert(FindObjectsOfType(typeof(FadeEffect)).Length == 1, "Only one component of type " + GetType().Name + " is allowed per scene");

        // track instance
        instance = this;
    }

    private void Start()
    {
        // fade in
        if (!IsOneShot || !fadedLevels.Contains(Application.loadedLevel))
        {
            StartCoroutine(FadeLevel(-1, LevelFadeInDuration));
            fadedLevels.Add(Application.loadedLevel);
            fadedLevels = fadedLevels.Distinct().ToList();
        }
    }
}
