using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MenuToggle
{
    [Tooltip("The menu music controller")]
    public MenuMusic MenuMusic;

    public override void OnClickAnimationComplete()
    {
        bool isOn = transform.GetComponent<Toggle>().isOn;
        PlayerProfile.SetIsMusicEnabled(isOn);
    }

    public override void Start()
    {
        base.Start();
        transform.GetComponent<Toggle>().isOn = PlayerProfile.GetIsMusicEnabled();
    }
}