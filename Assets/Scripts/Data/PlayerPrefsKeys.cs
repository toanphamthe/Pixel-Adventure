using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsKeys
{
    public const string MusicVolume = "MusicVolume";
    public const string SFXVolume = "SFXVolume";
    public const string Diamonds = "Diamonds";
    public const string PlayerSkinIndex = "PlayerSkinIndex";
    public static string LevelCompletedKey(int level)
    {
        return "Lv_" + (level) + "_Completed";
    }
    public static string LevelUnlockedKey(int level)
    {
        return "Lv_" + (level) + "_Unlocked";
    }
}
