using System.Collections;
using System.Linq;
using UnityEngine;

// effects for the ball
[RequireComponent(typeof(Ball))]
public class BallEffects : MonoBehaviour
{
    [Tooltip("Collision Effect particle system")]
    public ParticleSystem CollisionEffectParticleSystem;

    [Tooltip("The center propulsion particle system")]
    public ParticleSystem CenterPropulsion;

    [Tooltip("The rear propulsion particle system")]
    public ParticleSystem RearPropulsion;

    [Tooltip("The explosion particle system")]
    public ParticleSystem ExplosionParticleSystem;

    [Tooltip("The explosion animator")]
    public Animator ExplosionAnimator;

    [Tooltip("The turret")]
    public Transform Turret;

    [Tooltip("The explosion sound")]
    public SoundSource2D ExplosionSound;

    // the ball
    private Ball ball;

    // the wobble effect
    private WobbleEffect wobbleEffect;

    // is the explosion sequence completed
    private bool explosionCompleted = false;

    // the animator
    private Animator animator;

    public IEnumerator Scale(float duration, Vector3 targetScale)
    {
        animator.enabled = false;
        yield return StartCoroutine(TransformHelper.Scale(transform, duration, Vector3.zero));
        animator.enabled = true;
    }

    public IEnumerator PlayExplosion()
    {
        animator.enabled = true;
        animator.SetBool("IsDying", true);

        // explosionCompleted is set in a function below, which in turn gets called by the animator
        while (!explosionCompleted)
        {
            yield return null;
        }
    }

    private void Awake()
    {
        ball = GetComponent<Ball>();
        wobbleEffect = GetComponentInChildren<WobbleEffect>();
        animator = GetComponent<Animator>();
        ball.StateChanged += OnBallStateChanged;
    }

    private void Start()
    {
        Reset();
    }

    // ball state changed handler
    private void OnBallStateChanged(Ball.State newState)
    {
        if (newState == Ball.State.Aiming)
        {
            Reset();
            EnablePropulsions(false, true);
            EnableWobble(false);
            Turret.gameObject.SetActive(true);
        }
        else if (newState == Ball.State.Flying)
        {
            EnablePropulsions(true, true);
            EnableWobble(true);
            Turret.gameObject.SetActive(false);
        }
        else if (newState == Ball.State.Dying)
        {
            EnablePropulsions(false, false);
            EnableWobble(false);
            Turret.gameObject.SetActive(false);
        }
        else if (newState == Ball.State.Paralysed)
        {
            EnablePropulsions(false, false);
            EnableWobble(false);
        }
    }

    private void Reset()
    {
        animator.enabled = false;
        animator.SetBool("IsDying", false);
    }

    private void EnablePropulsions(bool enableCenterPropulsion, bool enableRearPropulsion)
    {
        CenterPropulsion.enableEmission = enableCenterPropulsion;
        RearPropulsion.enableEmission = enableRearPropulsion;
    }

    private void EnableWobble(bool enabled)
    {
        wobbleEffect.enabled = enabled;
    }

    // gets called by the animator
    private IEnumerator Explode()
    {
        ExplosionParticleSystem.Play();
        ExplosionSound.Try(x => x.Play());

        while (ExplosionParticleSystem.isPlaying || ExplosionSound.Try(x => x.IsPlaying()))
        {
            yield return null;
        }

        ExplosionParticleSystem.Clear();
        animator.enabled = false;
        explosionCompleted = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayCollisionEffect(collision);
    }

    private void PlayCollisionEffect(Collision2D collision)
    {
        // determine contact point
        var contactPoint = collision.contacts.Select(x => x.point).Aggregate((x, y) => x + y);
        contactPoint = contactPoint / collision.contacts.Length;

        // burst particle system at contact point
        var previousLocalPosition = CollisionEffectParticleSystem.transform.localPosition;
        CollisionEffectParticleSystem.transform.position = contactPoint;
        CollisionEffectParticleSystem.Play();
        CollisionEffectParticleSystem.transform.localPosition = previousLocalPosition;
    }
}
