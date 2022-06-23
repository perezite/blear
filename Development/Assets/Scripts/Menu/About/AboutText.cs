namespace Menu.About
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class AboutText : MonoBehaviour
    {
        // the ui text
        private Text text;

        // Use this for initialization
        private void Start()
        {
            text = GetComponent<Text>();
        }

        // Update is called once per frame
        private void Update()
        {
            text.text = text.text.Replace("[VersionNumber]", BuildSettings.CurrentProductVersion.ToString());
        }
    }
}
