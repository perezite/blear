namespace Menu.Title
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class ShopButton : MenuElement
    {
        protected override IEnumerator OnClickHandler()
        {
            GameManager.GetInstance().GoToLevel(Levels.ShopMenu);
            yield return null;
        }

        private void Awake()
        {
            if (BuildSettings.PremiumBuildModes.Contains(BuildSettings.CurrentBuildMode))
            {
                SetInteractable(false);
                var image = GetComponent<Image>();
                var newColor = new Color(image.color.r, image.color.g, image.color.b, 61f / 255f);
                image.color = newColor;
            }
        }
    }
}