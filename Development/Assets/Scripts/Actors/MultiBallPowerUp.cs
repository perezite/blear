using System.Collections;
using UnityEngine;

public class MultiBallPowerUp : Extra
{
    [Tooltip("The ball prefab")]
    public GameObject BallPrefab;

    private const string ParentGameObjectName = "Actors";

    protected override IEnumerator Affect()
    {   
        var ballClonePosition = new Vector2(transform.position.x, -4.592f);
        var ballClone = (GameObject)Instantiate(BallPrefab, ballClonePosition, Quaternion.identity);
        var newParentGameObject = GameObject.FindGameObjectWithTag(ParentGameObjectName);
        ballClone.transform.parent = newParentGameObject.transform;

        var ball = ballClone.GetComponent<Ball>();
        if (ball)
        {
            ball.QueueState(Ball.State.Flying);
        }

        yield return null;
    }
}
