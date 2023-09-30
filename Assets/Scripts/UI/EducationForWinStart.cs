using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EducationForWinStart : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI EducationForWinButtonText;

    // Start is called before the first frame update
    void Start()
    {
        Translation lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();        
        EducationForWinButtonText.text = lang.ForEducationStartButton;

        button.onClick.AddListener(() => 
        {
            Globals.IsSpectatorMode = true;
            GameManager.Instance.RestartLevel();
        });
    }

}
