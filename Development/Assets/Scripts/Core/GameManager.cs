using System.Collections;
using UnityEngine;

// Class for managing scene changes. Stores and loads the data from and to a a savegame file
public class GameManager
{
    // Singleton instance
    private static GameManager instance = null;

    // The savegame
    private Savegame savegame;

    // Private c'tor
    private GameManager()
    {
        savegame = new Savegame();
    }

    // Singleton Imstance
    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();           
        }

        return instance;
    }

    // get the savegame
    public Savegame GetSavegame()
    {
        return savegame;
    }

    // Go to level
    public void GoToLevel(int sceneIndex)
    {
        var savegameData = savegame.Load();

        // got back to main menu if we exceed the number of levels
        if (sceneIndex >= Application.levelCount)
        {
            Application.LoadLevel(Levels.TitleMenu);
            return;
        }

        // save progress, if we have never seen this level before
        if (sceneIndex > savegameData.LastVisitedGameLevel && sceneIndex > Levels.FirstGameLevel)
        {
            savegameData.LastVisitedGameLevel = sceneIndex;
            savegame.Save(savegameData);
        }

        Application.LoadLevel(sceneIndex);
    }

    // Go to level with loading indicator
    public IEnumerator GoToLevelWithLoadingIndicator(int sceneIndex)
    {
        #if UNITY_IPHONE
            Handheld.SetActivityIndicatorStyle(iOS.ActivityIndicatorStyle.Gray);
        #elif UNITY_ANDROID
            Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Large);
        #endif

		#if UNITY_IPHONE || UNITY_ANDROID
			Handheld.StartActivityIndicator();
		#endif 
		
        yield return new WaitForSeconds(0);
        GoToLevel(sceneIndex);
    }

    // Go to main menu
    public void GoToMainMenu()
    {
        Application.LoadLevel(Levels.TitleMenu);
    }
}