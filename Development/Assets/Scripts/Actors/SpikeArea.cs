using UnityEngine;

public class SpikeArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Ball")
        {
            var ball = other.transform.GetComponent<Ball>();
            ball.QueueState(Ball.State.Dying);
        }
    }
}
