using System.Collections;
using UnityEngine;

public abstract class Extra : MonoBehaviour
{
    [Tooltip("Drop speed")]
    public float DropSpeed = 1f;

    [Tooltip("Shrink duration")]
    public float ShrinkDuration = 0.5f;

    // offset from start to target
    private readonly Vector2 targetPositionOffset = new Vector2(0f, -16f);

    // start time
    private float startTime;

    // start position
    private Vector2 startPosition;

    // is busy
    private bool isDropping = true;

    protected abstract IEnumerator Affect();

    private void Start()
    {
        startTime = Time.time;
        startPosition = transform.position;
    }

    private void Update()
    {
        if (isDropping)
        {
            Drop();
        }
    }

    private void Drop()
    {
        var totalTime = targetPositionOffset.y / DropSpeed;
        var t = (startTime - Time.time) / totalTime;

        // move to target
        var targetPosition = startPosition + targetPositionOffset;
        transform.position = VectorHelper.SmoothStep(startPosition, targetPosition, t);

        // check if target reached
        var distance = Vector2.Distance(transform.position, targetPosition);
        if (Mathf.Approximately(distance, 0))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator OnTriggerEnter2D(Collider2D coll)
    {
        if (isDropping && coll.transform.tag == "Paddle")
        {
            isDropping = false;

            yield return StartCoroutine(TransformHelper.Scale(transform, ShrinkDuration, Vector3.zero));

            yield return StartCoroutine(Affect());

            Destroy(gameObject);
        }
    }
}
