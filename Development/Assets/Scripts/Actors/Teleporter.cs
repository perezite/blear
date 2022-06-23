using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("The other end of the teleporter")]
    public Teleporter Exit;

    [Tooltip("Ball shrink duration")]
    public float BallScaleDuration = 0.2f;

    // balls which are prepared for incoming teleport
    private List<Ball> incomingBalls = new List<Ball>();

    public void AddIncomingBall(Ball ball)
    {
        incomingBalls.Add(ball);
    }

    private void Start()
    {
        Debug.Assert(Exit != null, "Teleporter.Exit must not be null!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Ball")
        {
            var ball = other.GetComponent<Ball>();

            // ball hit the collider, it wasn't teleported into the collider
            if (incomingBalls.Contains(ball) == false)
            {
                StartCoroutine(TeleportBall(ball));
            }
            else
            {
                incomingBalls.Remove(ball);
            }
        }
    }

    private IEnumerator TeleportBall(Ball ball)
    {
        var ballPhysics = ball.GetComponent<BallPhysics>();
        var ballEffects = ball.GetComponent<BallEffects>();
        var ballOriginalVelocity = ballPhysics.Velocity;
        var ballOriginalScale = ball.transform.localScale;

        // stop the ball
        ball.QueueState(Ball.State.Paralysed);

        // shrink the ball
        yield return StartCoroutine(ballEffects.Scale(BallScaleDuration, Vector3.zero));

        // teleport the ball
        Exit.AddIncomingBall(ball);
        ball.transform.position = Exit.transform.position;

        // grow the ball
        yield return StartCoroutine(ballEffects.Scale(BallScaleDuration, ballOriginalScale));

        // relaunch the ball
        // note: we need the random tilt, because otherwise teleporter infinite loops are possible
        ball.QueueState(Ball.State.Flying);
        yield return null;
        var newVelocity = Quaternion.AngleAxis(-10f + (UnityEngine.Random.value * 20f), Vector3.forward) * ballOriginalVelocity;
        ballPhysics.Velocity = newVelocity;
    }
}
