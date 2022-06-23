namespace Menu.Title
{
    using System.Collections;

    public class SettingsButton : MenuElement
    {
        protected override IEnumerator OnClickHandler()
        {
            GameManager.GetInstance().GoToLevel(Levels.SettingsMenu);
            yield return null;
        }
    }
}