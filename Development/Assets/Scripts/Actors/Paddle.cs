using System.Collections;
using UnityEngine;

// controller for paddle
[RequireComponent(typeof(PaddlePhysics))]
public class Paddle : MonoBehaviour
{
    // start transform
    private TransformData startTransform;

    // current state
    private State currentState;

    // is new state queued
    private bool stateQueued = false;

    // the paddle physics
    private PaddlePhysics physics;

    // state enum
    public enum State
    {
        Paralysed, Aiming, Running
    }

    // queue new state
    public void QueueState(State state)
    {
        currentState = state;
        stateQueued = true;
    }

    private IEnumerator Start()
    {
        physics = GetComponent<PaddlePhysics>();
        startTransform = new TransformData(transform);
        currentState = State.Aiming;

        // state machine
        while (true)
        {
            stateQueued = false;
            yield return StartCoroutine(currentState.ToString());
        }
    }

    private IEnumerator Paralysed()
    {
        // reset
        physics.Velocity = Vector2.zero;

        // wait for state change
        while (!stateQueued)
        {
            yield return null;
        }
    }

    private IEnumerator Aiming()
    {
        // reset
        transform.Apply(startTransform);
        physics.Velocity = Vector2.zero;

        // wait for state change
        while (!stateQueued)
        {
            yield return null;
        }
    }
  
    private IEnumerator Running()
    {
        // wait for state change
        while (!stateQueued)
        {
            if (InputHelper.GetTap() || InputHelper.GetTapDown())
            {
                var tapPosition = VectorHelper.ScreenPointToWorldPoint(InputHelper.GetTapPosition());
                if (tapPosition.HasValue)
                {
                    physics.SetPosition(tapPosition.Value);
                    physics.Velocity = Vector2.zero;
                }
            }

            if (InputHelper.GetTapUp())
            {
                physics.Velocity = GetComponent<Velocimeter>().CurrentVelocity;
            }

            yield return null;
        }
    }
}