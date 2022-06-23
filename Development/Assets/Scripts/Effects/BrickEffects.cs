using System.Collections;
using UnityEngine;

public class BrickEffects : MonoBehaviour
{
    [Tooltip("Explosion particle system")]
    public ParticleSystem Explosion;

    [Tooltip("Should the marker effect be applied")]
    public bool ShowMarker = false;

    [Tooltip("Shrink duration before the brick explodes")]
    public float ExplosionShrinkDuration = 0.5f;

    [Tooltip("Shrink duration when the brick changes its durability")]
    public float ShrinkDuration = 0.25f;

    [Tooltip("Grow duration when the brick changes its durability")]
    public float GrowDuration = 0.25f;

    [Tooltip("The shadow caster")]
    public Collider2D ShadowCaster;

    [Tooltip("Sound")]
    public BrickSound Sound;

    // collider
    private Collider2D brickCollider;

    // sprite renderer
    private SpriteRenderer spriteRenderer;

    // initial scale
    private Vector3 initialScale;

    public IEnumerator PlayExplosion()
    {
        // play
        Explosion.Play();
        Sound.Try(x => x.Play());

        // scale
        yield return StartCoroutine(TransformHelper.Scale(transform, ExplosionShrinkDuration, Vector3.zero));

        // hide
        brickCollider.enabled = false;
        spriteRenderer.enabled = false;
        ShadowCaster.Try(x => x.gameObject.SetActive(false));

        // wait
        while (Explosion.isPlaying || Sound.Try(x => x.IsPlaying()))
        {
            yield return null;
        }
    }

    public IEnumerator PlayShrinkEffect()
    {
        Sound.Try(x => x.Play());

        yield return StartCoroutine(TransformHelper.Scale(transform, ShrinkDuration, Vector3.zero));
    }

    public IEnumerator PlayGrowEffect()
    {
        yield return StartCoroutine(TransformHelper.Scale(transform, GrowDuration, initialScale));
    }

    private void Start()
    {
        brickCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        initialScale = transform.localScale;
    }
}
