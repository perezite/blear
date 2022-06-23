using UnityEngine;

public class BallSound : MonoBehaviour
{
    [Tooltip("the ball wall collision sound effect")]
    public SoundSource2D BallWallCollisionSound;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.parent && coll.transform.parent.tag == "WorldBoundaries")
        {
            BallWallCollisionSound.Play();
        }

        if (coll.transform.tag == "BlackBrick")
        {
            BallWallCollisionSound.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Wheel")
        {
            BallWallCollisionSound.Play();
        }
    }
}
