using System.Collections;

public class BackButton : MenuElement
{
    protected override IEnumerator OnClickHandler()
    {
        GameManager.GetInstance().GoToLevel(Levels.TitleMenu);
        yield return null;
    }
}