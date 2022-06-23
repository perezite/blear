using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackingBrick : Brick
{
    [Tooltip("Movement speed when attacking")]
    public float Speed = 1f;

    // current state of the attacking brick
    private State currentState = State.Idle;

    // is a new state queued
    private bool stateQueued = false;

    // main position
    private Camera mainCamera;

    // paddle
    private GameObject paddle;

    // the collider
    private Collider2D coll;

    // ball effects
    private BrickEffects brickEffects;

    // states of the attacking brick
    private enum State
    {
        Idle, Attacking, Exploding, Dying
    }

    protected override void Start()
    {
        base.Start();

        paddle = GameObject.FindGameObjectWithTag("Paddle");
        mainCamera = Camera.main;
        coll = GetComponent<Collider2D>();
        brickEffects = GetComponent<BrickEffects>();

        // start state machine
        StartCoroutine(StateMachine());
    }

    protected override IEnumerator OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Ball")
        {
            QueueState(State.Attacking);
        }

        yield return null;
    }

    // queue a new state
    private void QueueState(State state)
    {
        currentState = state;
        stateQueued = true;
    }

    // state machine
    private IEnumerator StateMachine()
    {
        while (true)
        {
            stateQueued = false;
            yield return StartCoroutine(currentState.ToString());
        }
    }

    private IEnumerator Idle()
    {
        // wait for state change
        while (!stateQueued)
        {
            yield return null;
        }
    }

    private IEnumerator Attacking()
    {
        // make the collider, in order to only detect paddle collisions
        coll.isTrigger = true;

        // compute a vector which translates the brick to the camera bottom and passes through the paddle position
        // this involves some geometry. to understand it, it's best to draw it on a sheet of paper..
        var cameraBottom = mainCamera.GetCameraWorldspaceRect().Bottom;
        var distBrickToPaddle = paddle.transform.position - transform.position;
        var ratio = Mathf.Abs(distBrickToPaddle.x / distBrickToPaddle.y);
        var verticalLenPaddleToBottom = paddle.transform.position.y - cameraBottom;
        var horizontalLenPaddleToBottom = ratio * verticalLenPaddleToBottom;
        var lenPaddleToBottom = Mathf.Sqrt(Mathf.Pow(horizontalLenPaddleToBottom, 2f) + Mathf.Pow(verticalLenPaddleToBottom, 2f));
        var lenBrickToBottom = distBrickToPaddle.magnitude + lenPaddleToBottom;
        var distBrickToBottom = distBrickToPaddle.normalized * lenBrickToBottom;

        // smooth step towards target
        var startTime = Time.time;
        var startPosition = transform.position;
        var targetPosition = transform.position + distBrickToBottom;
        var totalTime = distBrickToBottom.magnitude / Speed;
        while (!stateQueued)
        {
            var t = (Time.time - startTime) / totalTime;
            transform.position = VectorHelper.SmoothStep(startPosition, targetPosition, t);
            var distance = Vector2.Distance(transform.position, targetPosition);

            if (Mathf.Approximately(distance, 0f))
            {
                QueueState(State.Dying);
            }

            yield return null;
        }
    }

    private IEnumerator Exploding()
    {
        // kill all the balls
        var balls = GameObject.FindGameObjectsWithTag("Ball").Select(x => x.GetComponent<Ball>()).ToList();
        balls.ForEach(x => x.QueueState(Ball.State.Dying));

        // play the epxlosion effect
        yield return brickEffects.TryStartCoroutine(this, x => x.PlayExplosion());
        gameObject.SetActive(false);
    }

    private IEnumerator Dying()
    {
        // play the epxlosion effect
        yield return brickEffects.TryStartCoroutine(this, x => x.PlayExplosion());
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.tag == "Paddle")
        {
            QueueState(State.Exploding);
        }
    }
}
