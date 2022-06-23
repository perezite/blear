// List of level indices
public static class Levels
{
    public static int Splash = 0;
    public static int TitleMenu = 1;
    public static int SettingsMenu = 2;
    public static int AboutMenu = 3;
    public static int LevelSelectionMenu = 4;
    public static int ShopMenu = 5;
    public static int Level1 = GetAbsoluteLevelIndex(1);
    public static int Level2 = GetAbsoluteLevelIndex(2);
    public static int Level3 = GetAbsoluteLevelIndex(3);
    public static int Level4 = GetAbsoluteLevelIndex(4); 
    public static int Level5 = GetAbsoluteLevelIndex(5);
    public static int Level6 = GetAbsoluteLevelIndex(6);
    public static int Level7 = GetAbsoluteLevelIndex(7);
    public static int Level8 = GetAbsoluteLevelIndex(8);
    public static int Level9 = GetAbsoluteLevelIndex(9);
    public static int Level10 = GetAbsoluteLevelIndex(10);
    public static int Level11 = GetAbsoluteLevelIndex(11);
    public static int Level12 = GetAbsoluteLevelIndex(12);
    public static int Level13 = GetAbsoluteLevelIndex(13);
    public static int Level14 = GetAbsoluteLevelIndex(14);
    public static int Level15 = GetAbsoluteLevelIndex(15);
    public static int Level16 = GetAbsoluteLevelIndex(16);
    public static int Level17 = GetAbsoluteLevelIndex(17);
    public static int Level18 = GetAbsoluteLevelIndex(18);
    public static int Level19 = GetAbsoluteLevelIndex(19);
    public static int Level20 = GetAbsoluteLevelIndex(20);
    public static int Level21 = GetAbsoluteLevelIndex(21);
    public static int EndLevel = GetAbsoluteLevelIndex(22);

    public static int LastMenuLevel
    {
        get
        {
            return ShopMenu;
        }
    }

    public static int FirstGameLevel
    {
        get
        {
            return Level1;
        }
    }

    public static int GetAbsoluteLevelIndex(int relativeLevelIndex)
    {
        return LastMenuLevel + relativeLevelIndex;
    }
}