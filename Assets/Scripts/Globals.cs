using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static PlayerData MainPlayerData;
    public static int CurrentStars;
    public static int CurrentLevel;
    public static string CurrentLanguage;
    public static bool IsMobilePlatform;
    public static bool IsSoundOn;
    public static bool IsInitiated = false;
    public static DateTime TimeWhenStartedPlaying;
    public static DateTime TimeWhenLastInterstitialWas;
    public static DateTime TimeWhenLastRewardedWas;

    public const float TIME_FOR_ACCIDENT = 3f;
    public const float DISTANCE_FOR_ACCIDENT = 1.25f;
    public const float DISTANCE_FOR_OBJECTS = 1.5f;
    public static bool IsShowBorder = true;

    public const float TIME_FOR_RIDE_TAXI = 4f;
    public const float TIME_FOR_RIDE_AMBULANCE = 4.5f;
    public const float TIME_FOR_RIDE_VAN = 5f;

    public static int ScreenWidth = 0;
    public static int ScreenHeight = 0;

    //public static int CurrentLevel = 1;
    //public static int CurrentStars = 1;
}
