using UnityEngine;

// physics (Rigidbody2d and Collider2D) wrapper for the ball
// reference: http://forum.unity3d.com/threads/2d-physics-bounciness-issue.211844/
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallPhysics : MonoBehaviour
{
    [Tooltip("Strength of slope correction")]
    public float SlopeCorrectionStrength = 1f;

    // Constant speed of the ball
    private float speed = 0f;

    // Velocity
    private Vector2 velocity;

    // rigidbody
    private Rigidbody2D ballRigidbody;

    // collider
    private Collider2D ballCollider;

    // velocity
    public Vector2 Velocity
    {
        get
        {
            return velocity;
        }

        set
        {
            ballRigidbody.velocity = value;
            speed = ballRigidbody.velocity.magnitude;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, ballRigidbody.velocity);
            velocity = value;
        }
    }

    // Use this for initialization
    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<Collider2D>();
        Velocity = ballRigidbody.velocity;
    }

    private void FixedUpdate()
    {
        // apply fixed velocity
        Velocity = ballRigidbody.velocity.normalized * speed;

        // rotate the vector slowly towards a 45 degrees slope
        Velocity = Vector3.RotateTowards(Velocity, GetTargetDirection(Velocity), SlopeCorrectionStrength * Time.fixedDeltaTime, 0f);                
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        // Normal
        Vector3 n = coll.contacts[0].normal;

        // Direction
        Vector3 v = Velocity.normalized;

        // Reflection
        Vector2 r = VectorHelper.Reflect(v, n).normalized;

        // Deflection
        Vector2 d = r;
        if (coll.gameObject.tag == "Paddle")
        {
            // deflect the ball reflection
            r = Vector2.up; 
            d = DeflectVelocity(r, coll.gameObject);

            // move ball right to the top of the paddle. This prevents the ball from being squeezed between paddle and boundaries
            var paddleCollider = coll.transform.GetComponent<Collider2D>();
            var vertPaddlePosition = coll.transform.position.y;
            var vertPaddleExtent = paddleCollider.bounds.extents.y;
            var vertBallExtent = ballCollider.bounds.extents.y;
            transform.position = new Vector2(transform.position.x, vertPaddlePosition + vertPaddleExtent + vertBallExtent);
        }

        // Assign normalized reflection with the constant speed
        Velocity = d * speed;
    }

    // reference: https://github.com/perezite/sfml-breakout/blob/master/src/Ball.cpp
    private Vector2 DeflectVelocity(Vector2 v, GameObject paddle)
    {
        float verticalPaddleCenter = paddle.transform.position.x;
        float verticalBallCenter = transform.position.x;
        float verticalBallExtent = ballCollider.bounds.extents.x;

        float ratio = (verticalBallCenter - verticalPaddleCenter) / verticalBallExtent;
        float originalLen = v.magnitude;
        v = v.normalized;
        v.x += ratio * 0.5f;
        v = v.normalized * originalLen;

        return v;
    }

    // get angle nearest to a multiple of 45° relative to Vector2.right
    private Vector2 GetTargetDirection(Vector2 v)
    {
        float maximalSlopeAngle = 45f;      // relative to Vector2.up or Vector2.down
        var targetDirection = Vector2.zero;

        if (v.y >= 0)
        {
            float alpha = Vector2.Angle(v, Vector2.up);
            alpha = Mathf.Min(alpha, maximalSlopeAngle);

            targetDirection = Quaternion.Euler(0, 0, -Mathf.Sign(v.x) * alpha) * Vector2.up;
        }
        else
        {
            float alpha = Vector2.Angle(v, Vector2.down);
            alpha = Mathf.Min(alpha, maximalSlopeAngle);

            targetDirection = Quaternion.Euler(0, 0, Mathf.Sign(v.x) * alpha) * Vector2.down;
        }

        return targetDirection;
    }
}