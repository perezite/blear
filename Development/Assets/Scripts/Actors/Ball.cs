using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// controller for ball
[RequireComponent(typeof(BallPhysics))]
public class Ball : MonoBehaviour
{
    [Tooltip("Aiming angle in degrees")]
    public float AimingAngle = 70f;

    [Tooltip("Aiming rotation speed")]
    public float AimingOmega = 150f;

    [Tooltip("Speed of the ball")]
    public float Speed = 1f;

    // state changed action
    public Action<State> StateChanged;

    // static list of all balls
    private static List<Ball> balls = new List<Ball>();

    // angle of pointer
    private float currentAimingAngle;

    // transform at startup
    private TransformData startTransform;

    // current state of the ball
    private State currentState = State.Aiming;

    // is a new state queued
    private bool stateQueued = false;

    // ball physics
    private BallPhysics ballPhysics;

    // ball effects
    private BallEffects ballEffects;

    // states of the ball
    public enum State
    {
        Aiming, Flying, Dying, Paralysed
    }

    public static List<Ball> GetAllBalls()
    {
        return balls; 
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    // is the ball dying
    public bool IsDying()
    {
        return currentState == State.Dying;
    }

    // queue a new state
    public void QueueState(State state)
    {
        currentState = state;
        stateQueued = true;
    }

    private void OnEnable()
    {
        balls.Add(this);
    }

    private void OnDisable()
    {
        balls.Remove(this);
    }

    // Use this for initialization
    private IEnumerator Start()
    {
        // get components
        startTransform = new TransformData(transform);
        ballPhysics = GetComponent<BallPhysics>();
        ballEffects = GetComponent<BallEffects>();

        // ignore ball-ball collisions
        var balls = GameObject.FindGameObjectsWithTag(transform.tag);
        var ballColliders = balls.Select(x => x.GetComponent<Collider2D>()).ToList();
        GetComponent<Collider2D>().Try(x => ballColliders.ForEach(b => Physics2D.IgnoreCollision(b, x)));

        // state machine
        while (true)
        {
            stateQueued = false;
            ActionHelper.TryInvoke(StateChanged, currentState);
            yield return StartCoroutine(currentState.ToString());
        }
    }

    private IEnumerator Aiming()
    {
        // reset
        ballPhysics.Velocity = Vector2.zero;
        transform.Apply(startTransform);

        // perform aiming
        while (!stateQueued)
        {
            // compute and apply aiming angle
            currentAimingAngle = AimingAngle - Mathf.PingPong(Time.time * AimingOmega, 2f * AimingAngle);
            transform.rotation = Quaternion.identity;
            transform.RotateAround(transform.position, Vector3.forward, currentAimingAngle);

            yield return null;
        }
    }

    private IEnumerator Flying()
    {
        // set launch velocity
        Vector2 launchVelocity = Quaternion.AngleAxis(currentAimingAngle, Vector3.forward) * Vector3.up;
        ballPhysics.Velocity = launchVelocity.normalized * Speed;

        // wait for state change
        while (!stateQueued)
        {
            if (IsStuck())
            {
                Unstick();
            }

            yield return null;
        }
    }

    private IEnumerator Dying()
    {
        // stop and explode
        ballPhysics.Velocity = Vector2.zero;
        yield return ballEffects.TryStartCoroutine(this, x => ballEffects.PlayExplosion());
        Destroy(gameObject);

        while (!stateQueued)
        {
            yield return null;
        }
    }

    private IEnumerator Paralysed()
    {
        ballPhysics.Velocity = Vector2.zero;

        while (!stateQueued)
        {
            yield return null;
        }
    }

    private bool IsStuck()
    {
        return Mathf.Approximately(ballPhysics.Velocity.magnitude, 0f);
    }

    private void Unstick()
    {
        startTransform.ApplyTo(transform);
        var launchAngle = UnityEngine.Random.Range(-AimingAngle, AimingAngle);
        var launchVelocity = Quaternion.AngleAxis(launchAngle, Vector3.forward) * Vector3.up;
        ballPhysics.Velocity = launchVelocity.normalized * Speed;
    }
}