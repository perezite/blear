using UnityEngine;

// This class is used for providing and managing world game objects
public class WorldManager : MonoBehaviour
{
    // Singleton instance
    private static WorldManager instance = null;

    // World Boundaries
    private GameObject leftBoundary = null;
    private GameObject rightBoundary = null;
    private GameObject topBoundary = null;
    private GameObject bottomBoundary = null;

    // world ground
    private GameObject ground = null;

    // Private c'tor
    private WorldManager()
    {
    }

    // Boundaries of the current world
    public Rect Boundaries
    {
        get
        {
            float leftPos = leftBoundary.transform.position.x + (leftBoundary.GetComponent<Collider2D>().bounds.size.x / 2f);
            float rightPos = rightBoundary.transform.position.x - (rightBoundary.GetComponent<Collider2D>().bounds.size.x / 2f);
            float topPos = topBoundary.transform.position.y - (topBoundary.GetComponent<Collider2D>().bounds.size.y / 2f);
            float bottomPos = bottomBoundary.transform.position.y + (bottomBoundary.GetComponent<Collider2D>().bounds.size.y / 2f);
            float width = rightPos - leftPos;
            float height = topPos - bottomPos;

            return new Rect(leftPos, topPos, width, height);
        }
    }

    public GameObject Ground
    {
        get
        {
            return ground;
        }
    }

    // Singleton instance
    public static WorldManager GetInstance()
    {
        if (instance == null)
        {
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<WorldManager>();
            gameObject.name = "WorldManagerInstance";
            gameObject.hideFlags = HideFlags.HideInHierarchy;
            instance = gameObject.GetComponent<WorldManager>();
        }

        return instance;
    }

    // Singleton Awake
    private void Awake()
    {
        // collect world boundaries
        GameObject boundaryParent = GameObject.FindGameObjectWithTag("WorldBoundaries");
        if (boundaryParent != null)
        {
            leftBoundary = boundaryParent.transform.Find("Left").gameObject;
            rightBoundary = boundaryParent.transform.Find("Right").gameObject;
            topBoundary = boundaryParent.transform.Find("Top").gameObject;
            bottomBoundary = boundaryParent.transform.Find("Bottom").gameObject;
        }

        // collect world ground
        ground = GameObject.FindGameObjectWithTag("Ground");
    }
}