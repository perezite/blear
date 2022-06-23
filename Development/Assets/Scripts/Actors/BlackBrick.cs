using System.Collections;
using UnityEngine;

public class BlackBrick : MonoBehaviour
{
    // the brick effects
    private BrickEffects effects;

    public IEnumerator Destroy()
    {
        yield return StartCoroutine(effects.PlayShrinkEffect());
        Destroy(gameObject);
    }

    private void Start()
    {
        effects = GetComponent<BrickEffects>();
    }
}
