using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MenuToggle
{
    public override void OnClickAnimationComplete()
    {
        bool isOn = transform.GetComponent<Toggle>().isOn;
        PlayerProfile.SetIsSoundEnabled(isOn);
    }

    public void OnSelect()
    {
        // Debug.Log("OnSelect()");
    }

    public override void Start()
    {
        base.Start();
        transform.GetComponent<Toggle>().isOn = PlayerProfile.GetIsSoundEnabled();
    }
}