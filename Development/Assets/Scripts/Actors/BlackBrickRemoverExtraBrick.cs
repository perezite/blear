using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// an extra brick which removes a list of black bricks upon collision
public class BlackBrickRemoverExtraBrick : MonoBehaviour
{
    [Tooltip("bricks which are removed upon collision")]
    public List<GameObject> BlackBricks = new List<GameObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BlackBricks.ForEach(x => StartCoroutine(x.GetComponent<BlackBrick>().Destroy()));
    }
}
