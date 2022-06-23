using System.Collections;
using UnityEngine;

// fixes a Unity issue causing the scene to stutter at the first few frames after loading 
public class SceneLoadFramerateOptimizer : MonoBehaviour
{
    // cheesy singleton instance
    private static SceneLoadFramerateOptimizer instance;

    // is done
    private bool isDone = false;

    // get cheesy singleton
    public static SceneLoadFramerateOptimizer GetInstance()
    {
        return instance;
    }

    public IEnumerator Await()
    {
        while (!isDone)
        {
            yield return null;
        }
    }

    private void Awake()
    {
        Debug.Assert(FindObjectsOfType(typeof(SceneLoadFramerateOptimizer)).Length == 1, "Only one component of type " + GetType().Name + " is allowed per scene");

        instance = this;
    }

    // Use this for initialization
    private IEnumerator Start()
    {
        var originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }

        Time.timeScale = originalTimeScale;
        isDone = true;
    }
}
