using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager
{
    public static List<string> PlayerPrefsKeys;
    public static void SetInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
        PlayerPrefsKeys.Add(name);
    }
    public static void SetString(string name, string value)
    {
        PlayerPrefs.SetString(name, value);
        PlayerPrefsKeys.Add(name);
    }
    public static void SetFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
        PlayerPrefsKeys.Add(name);
    }
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefsKeys.Clear();
    }
    public static void DeleteKey(string name)
    {
        PlayerPrefs.DeleteKey(name);
        PlayerPrefsKeys.Remove(name);
    }
}
