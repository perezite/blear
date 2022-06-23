using System.Collections;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private void Update()
    { 
        if (InputHelper.GetTapDown())
        {
            GameManager.GetInstance().GoToLevel(Levels.TitleMenu);
        }
    }
}
