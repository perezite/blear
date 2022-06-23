namespace Menu.Title
{
    using System.Collections;

    using UnityEngine;

    public class StartButton : MenuElement
    {
        protected override IEnumerator OnClickHandler()
        {
            GameManager.GetInstance().GoToLevel(Levels.LevelSelectionMenu);
            yield return null;
        }
    }
}