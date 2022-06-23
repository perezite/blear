using UnityEngine;

// controller for the dummy level, which is used to circumevent the stuttering at the first level startup in the editor
public class DummyLevel : MonoBehaviour
{
    // Use this for initialization
    public void Start()
    {
        Application.LoadLevel(Levels.Splash);
    }
}
