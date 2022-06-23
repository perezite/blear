using UnityEngine;

public class PaddleSound : MonoBehaviour
{
    [Tooltip("the paddle wall collision sound")]
    public SoundSource2D PaddleWallCollisionSound;

    [Tooltip("the paddle ball collision sound")]
    public SoundSource2D PaddleBallCollisionSound;

    // the intial volume of the sound source
    private float paddleWallCollisionInitVolume;

    // the physics
    private PaddlePhysics paddlePhysics;

    // the velocimeter
    private Velocimeter velocimeter;

    private void Start()
    {
        paddleWallCollisionInitVolume = PaddleWallCollisionSound.Volume;
        paddlePhysics = GetComponent<PaddlePhysics>();
        velocimeter = GetComponent<Velocimeter>();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.parent.tag == "WorldBoundaries")
        {
            var velocityRatio = Mathf.Clamp(velocimeter.CurrentVelocity.magnitude / paddlePhysics.MaximalSlideSpeed, 0, 1);
            PaddleWallCollisionSound.Volume = paddleWallCollisionInitVolume * velocityRatio;
            PaddleWallCollisionSound.Play();
        }

        if (coll.transform.tag == "Ball")
        {
            PaddleBallCollisionSound.Play();
        }
    }
}
