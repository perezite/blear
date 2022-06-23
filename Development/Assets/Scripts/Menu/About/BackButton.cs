namespace Menu.About
{
    using System.Collections;

    public class BackButton : MenuElement
    {
        protected override IEnumerator OnClickHandler()
        {
            GameManager.GetInstance().GoToLevel(Levels.SettingsMenu);
            yield return null;
        }
    }
}