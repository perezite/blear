namespace Menu.Settings
{
    using System.Collections;

    public class AboutButton : MenuElement
    {
        protected override IEnumerator OnClickHandler()
        {
            GameManager.GetInstance().GoToLevel(Levels.AboutMenu);
            yield return null;
        }
    }
}