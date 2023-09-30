using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static PlayerData MainPlayerData;
    public static int CurrentStars = 0;
    public static int CurrentLevel = 2;
    public static string CurrentLanguage = "ru";
    public static bool IsMobilePlatform = false;
    public static bool IsSoundOn = true;
    public static bool IsInitiated = false;
    public static DateTime TimeWhenStartedPlaying;
    public static DateTime TimeWhenLastInterstitialWas;
    public static DateTime TimeWhenLastRewardedWas;

    public static bool IsMainScreen = true;
    public static bool IsInfoActive;

    public const float REWARDED_COOLDOWN = 65f;
    public const float INTERSTITIAL_COOLDOWN = 65f;

    public const float HOW_MANY_SEC_ADD_FOR_REWARD_KOEFF = 0.2f;

    public const float TIME_FOR_ACCIDENT = 3f;
    public const float DISTANCE_FOR_ACCIDENT = 1.25f;
    public const float DISTANCE_FOR_OBJECTS = 1.5f;
    public static bool IsShowBorder = true;

    public const float TIME_FOR_RIDE_TAXI = 4f;
    public const float TIME_FOR_RIDE_AMBULANCE = 4.5f;
    public const float TIME_FOR_RIDE_VAN = 5f;

    public const float SWIPE_SPEED = 0.25f;

    public static int ScreenWidth = 0;
    public static int ScreenHeight = 0;

    public static int[] WINS_LOSES = new int[200];
    public static bool IsSpectatorMode = false;
    public static HashSet<int> LevelsWithEducation = new HashSet<int>(new[] { 6,9,10,12, 15, 16 });

    //public static int CurrentLevel = 1;
    //public static int CurrentStars = 1;
}
