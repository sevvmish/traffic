using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "Languages", order = 1)]
public class Translation : ScriptableObject
{
    public string PlayText;
    public string PressFirstLevelText_MainMenu;
    public string Level1_Tutorial;
    public string Level1_Tutorial_2;
    public string Level1_Tutorial_3;
    public string Level1_Tutorial_4;
    public string Level1_Tutorial_5;

    public string Level5_Tutorial;
    public string Level5_Tutorial_1;

    public string Level11_Tutorial;
    public string Level11_Tutorial_1;

    public string Level12_Tutorial;

    public string SpeedOfGameWinMenu;
    public string MistakesOfGameWinMenu;
    public string AccidentsOfGameWinMenu;
    public string NextOfGameWinMenu;

    public string WinText;
    public string LoseText;

    public string GetMoreTextReward;
    public string SecondsText;

    public string GetMoreStars;

    public string AllProgressWillBeReset;

    public Translation() { }
}
