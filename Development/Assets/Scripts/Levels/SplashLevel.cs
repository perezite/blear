using System.Collections;
using UnityEngine;

public class SplashLevel : MonoBehaviour
{
    [Tooltip("Duration in seconds the logo is displayed (excluding fading)")]
    public float DisplayDuration = 2.5f;

    [Tooltip("Level fade out duration in seconds")]
    public float LevelFadeOutDuration = 0.5f;

    // Use this for initialization
    private IEnumerator Start()
    {
        yield return SceneLoadFramerateOptimizer.GetInstance().TryStartCoroutine(this, x => x.Await());
        yield return new WaitForSeconds(DisplayDuration);
        yield return StartCoroutine(FadeEffect.GetInstance().FadeLevel(1, LevelFadeOutDuration));

        GameManager.GetInstance().GoToLevel(Levels.TitleMenu);

        yield return null;
    }
}
