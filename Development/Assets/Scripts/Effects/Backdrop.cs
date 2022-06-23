using System.Linq;
using UnityEngine;

public class Backdrop : MonoBehaviour
{
    [Tooltip("Activate backdrop at startup")]
    public bool ActivateOnStartup = true;

    private void Awake()
    {
        if (ActivateOnStartup)
        {
            transform.GetAllChildren().ToList().ForEach(x => x.gameObject.SetActive(true));
        }
    }
}
