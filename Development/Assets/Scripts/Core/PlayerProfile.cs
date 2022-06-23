using System;
using UnityEngine;

// Typesafe wrapper for PlayerPrefs
public static class PlayerProfile
{
    // is music enabled changed action
    public static Action IsMusicEnabledChanged;

    // is sound enabled changed action
    public static Action IsSoundEnabledChanged;

    // is music enabled default value
    private const bool IsMusicEnabledDefaultValue = true;

    // is sound enabled default value
    private const bool IsSoundEnabledDefaultValue = true;

    public static bool GetIsMusicEnabled()
    {
        return GetBoolean("IsMusicEnabled", IsMusicEnabledDefaultValue);
    }

    public static bool GetIsSoundEnabled()
    {
        return GetBoolean("IsSoundEnabled", IsSoundEnabledDefaultValue);
    }

    public static void SetIsMusicEnabled(bool value)
    {
        SetBoolean("IsMusicEnabled", value);

        IsMusicEnabledChanged.TryInvoke();
    }

    public static void SetIsSoundEnabled(bool value)
    {
        SetBoolean("IsSoundEnabled", value);

        IsSoundEnabledChanged.TryInvoke();
    }

    private static bool GetBoolean(string key, bool defaultValue = false)
    {
        int defaultIntValue = System.Convert.ToInt32(defaultValue);
        int value = PlayerPrefs.GetInt(key, defaultIntValue);
        return System.Convert.ToBoolean(value);
    }

    private static void SetBoolean(string key, bool value)
    {
        int intValue = System.Convert.ToInt32(value);
        PlayerPrefs.SetInt(key, intValue);
    }
}