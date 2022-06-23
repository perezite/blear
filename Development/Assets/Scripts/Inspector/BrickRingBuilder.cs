#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// creates a ring of bricks around a wheel in editor mode
[RequireComponent(typeof(Wheel))]
public class BrickRingBuilder : MonoBehaviour
{
    [Tooltip("The brick prefab")]
    public Brick BrickPrefab;

    [Tooltip("The radius of the wheel")]
    public float Radius;

    [Tooltip("Number of bricks to be placed on the ring")]
    public int NumberOfBricks;

    // last placed set of bricks
    private List<Brick> lastBrickRing = new List<Brick>();

    public void Build()
    {
        var deltaAngle = 360f / NumberOfBricks;
        var brickParent = GameObject.FindGameObjectWithTag("Actors").transform.GetAllChildren().ToList().First(x => x.name == "Bricks");
        lastBrickRing.Clear();

        for (int i = 0; i < NumberOfBricks; i++)
        {
            var angle = deltaAngle * i;
            var displacement = (Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right) * Radius;
            var brickPosition = transform.position + displacement;

            var brickClone = (GameObject)PrefabUtility.InstantiatePrefab(BrickPrefab.gameObject);
            brickClone.transform.position = brickPosition;
            brickClone.transform.rotation = Quaternion.identity;
            brickClone.gameObject.name = BrickPrefab.gameObject.name;
            var brick = brickClone.GetComponent<Brick>();
            brick.transform.parent = brickParent;

            lastBrickRing.Add(brick);
        }
    }

    public void Undo()
    {
        foreach (var brick in lastBrickRing)
        {
            if (brick.gameObject)
            {
                DestroyImmediate(brick.gameObject);
            }
        }
    }
}
#endif