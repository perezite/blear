using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelResultScreenEffects : MonoBehaviour
{
    [Tooltip("Fade duration in seconds")]
    public float FadeDuration = 1f;

    [Tooltip("UI grow effect duration in seconds")]
    public float UiGrowDuration = 1f;

    // sprite renderer alphas before fading out
    private Dictionary<SpriteRenderer, float> spriteRendererInitialAlphas = new Dictionary<SpriteRenderer, float>();

    // ui elements alphas before fading out 
    private Dictionary<Graphic, float> uiElementInitialAlphas = new Dictionary<Graphic, float>();

    // children scales before scaling
    private Dictionary<GameObject, Vector3> initialScales = new Dictionary<GameObject, Vector3>();

    public void Reset()
    {
        spriteRendererInitialAlphas.Clear();
        uiElementInitialAlphas.Clear();
        initialScales.Select(x => new { child = x.Key, initialScale = x.Value })
            .Where(x => x.child).ToList().ForEach(x => x.child.transform.localScale = x.initialScale);
        initialScales.Clear();
    }

    public IEnumerator WaitFadeAndGrow()
    {
        // hide children
        transform.GetAllChildrenRecursively().ToList().ForEach(x => x.gameObject.SetActive(false));

        // wait one frame to prevent fade effect at startup
        yield return null;

        // fade out game objects
        yield return StartCoroutine(FadeOutGameObjects());

        // grow in child ui elements
        yield return StartCoroutine(PlayGrowAnimation());
    }

    public IEnumerator PlayShrinkAnimation()
    {
        // make all ui elements non interactable
        var children = transform.GetAllChildrenRecursively().ToList();
        var selectableComponents = children.SelectMany(x => x.GetComponentsInChildren<Selectable>()).ToList();
        selectableComponents.ForEach(x => x.interactable = false);

        // set up start and target scales
        var startScales = new Dictionary<GameObject, Vector3>();
        var targetScales = new Dictionary<GameObject, Vector3>();
        children.ForEach(x => startScales.Add(x.gameObject, x.transform.localScale));
        children.ForEach(x => targetScales.Add(x.gameObject, Vector3.zero));

        // shrink
        yield return StartCoroutine(Scale(startScales, targetScales));

        // make all elements interactable again
        selectableComponents.ForEach(x => x.interactable = true);
    }

    public void RevealGameObjects()
    {
        // unhide sprite renderers
        spriteRendererInitialAlphas.Where(x => x.Key).Select(x => new { sr = x.Key, initialAlpha = x.Value })
            .ToList().ForEach(x => x.sr.color = new Color(x.sr.color.r, x.sr.color.g, x.sr.color.b, x.initialAlpha));

        // unhide ui elements
        uiElementInitialAlphas.Where(x => x.Key).Select(x => new { uie = x.Key, initialAlpha = x.Value })
            .ToList().ForEach(x => x.uie.color = new Color(x.uie.color.r, x.uie.color.g, x.uie.color.b, x.initialAlpha));
    }

    private IEnumerator PlayGrowAnimation()
    {
        // set up start and target scales
        var children = transform.GetAllChildrenRecursively().ToList();
        children.ForEach(x => x.gameObject.SetActive(true));
        var startScales = new Dictionary<GameObject, Vector3>();
        var targetScales = new Dictionary<GameObject, Vector3>();
        children.ForEach(x => startScales.Add(x.gameObject, Vector3.zero));
        children.ForEach(x => targetScales.Add(x.gameObject, x.transform.localScale));
        children.ForEach(x => initialScales.Add(x.gameObject, x.transform.localScale));

        // grow
        yield return StartCoroutine(Scale(startScales, targetScales));
    }

    private IEnumerator Scale(Dictionary<GameObject, Vector3> startScales, Dictionary<GameObject, Vector3> targetScales)
    {
        var children = transform.GetAllChildrenRecursively().ToList();

        float t = 0;
        float startTime = Time.time;
        while (t <= 1)
        {
            t = (Time.time - startTime) / FadeDuration;

            foreach (var child in children)
            {
                if (child != null)
                {
                    var targetScale = targetScales[child.gameObject];
                    var startScale = startScales[child.gameObject];
                    child.transform.localScale = VectorHelper.SmoothStep(startScale, targetScale, t);
                }
            }

            yield return null;
        }
    }

    private IEnumerator FadeOutGameObjects()
    {
        // collect potential objects to fade
        var objectsExcludedFromFade = new List<GameObject>();
        objectsExcludedFromFade.AddRange(GameObject.FindGameObjectsWithTag("Backdrop"));
        objectsExcludedFromFade.AddRange(transform.GetAllChildrenRecursively().ToList().Select(x => x.gameObject));
        var objectsToFade = FindObjectsOfType(typeof(Transform)).Select(x => (Transform)x).Select(x => x.gameObject).ToList();
        objectsToFade = objectsToFade.Except(objectsExcludedFromFade).ToList();

        // collect components to fade
        var spriteRenderersToFade = objectsToFade.Select(x => x.GetComponent<SpriteRenderer>()).Where(x => x != null).ToList();
        var uiElementsToFade = objectsToFade.Select(x => x.GetComponent<Graphic>()).Where(x => x != null).ToList();

        // collect initial alpha values of components to fade
        spriteRenderersToFade.ForEach(x => spriteRendererInitialAlphas.Add(x, x.color.a));
        uiElementsToFade.ForEach(x => uiElementInitialAlphas.Add(x, x.color.a));

        // fade
        float t = 0;
        float startTime = Time.time;
        while (t <= 1)
        {
            t = (Time.time - startTime) / FadeDuration;

            foreach (var sr in spriteRenderersToFade)
            {
                if (sr != null)
                {
                    var initialAlpha = spriteRendererInitialAlphas[sr];
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.SmoothStep(initialAlpha, 0, t));
                }
            }

            foreach (var uie in uiElementsToFade)
            {
                if (uie != null)
                {
                    var initialAlpha = uiElementInitialAlphas[uie];
                    uie.color = new Color(uie.color.r, uie.color.g, uie.color.b, Mathf.SmoothStep(initialAlpha, 0, t));
                }
            }

            yield return null;
        }
    }
}