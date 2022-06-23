using UnityEngine;

// physics (Rigidbody2d and Collider2D) wrapper for the paddle
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PaddlePhysics : MonoBehaviour
{
    [Tooltip("Maximal allowed horizontal slide velocity")]
    public float MaximalSlideSpeed = 9f;

    [Tooltip("Minimal allowed horizontal slide velocity")]
    public float MinimalSlideSpeed = 0.2f;

    // rigidbody
    private Rigidbody2D paddleRigidbody;

    // collider
    private Collider2D paddleCollider;

    // physics velocity
    public Vector2 Velocity
    {
        get
        {
            return paddleRigidbody.velocity;
        }

        set
        {
            Vector2 restrictedVelocity = GetRestrictedVelocity(value);
            paddleRigidbody.velocity = restrictedVelocity;
        }
    }

    // physics position
    public void SetPosition(Vector2 position)
    {
        Vector2 restrictedPosition = GetRestrictedPosition(position);
        GetComponent<Rigidbody2D>().position = restrictedPosition;
    }

    private void Awake()
    {
        paddleRigidbody = GetComponent<Rigidbody2D>();
        paddleCollider = GetComponent<Collider2D>();
    }

    // get restricted position
    private Vector2 GetRestrictedPosition(Vector2 position)
    {
        Rect worldBoundaries = WorldManager.GetInstance().Boundaries;
        float horizontalMin = worldBoundaries.xMin + (paddleCollider.bounds.size.x / 2f);
        float horizontalMax = worldBoundaries.xMax - (paddleCollider.bounds.size.x / 2f);
        float positionHorizontal = Mathf.Clamp(position.x, horizontalMin, horizontalMax);

        return new Vector2(positionHorizontal, transform.position.y);
    }

    // get restricted velocity
    private Vector2 GetRestrictedVelocity(Vector2 velocity)
    {
        if (Mathf.Abs(velocity.x) < MinimalSlideSpeed)
        {
            velocity.x = 0f;
        }

        if (Mathf.Abs(velocity.x) > MaximalSlideSpeed)
        {
            velocity.x = MaximalSlideSpeed * Mathf.Sign(velocity.x);
        }

        return velocity;
    }
}