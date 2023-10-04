using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class LoseGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject restartPanel;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button rewardButton;
    [SerializeField] private GameObject rewardIcon;

    [SerializeField] private TextMeshProUGUI GetMoreText;
    [SerializeField] private TextMeshProUGUI SecondsText;

    [SerializeField] private TextMeshProUGUI loseText;

    void Start()
    {
        
        restartButton.onClick.AddListener(() => 
        {
            GameManager.Instance.RestartLevel();
        });

        rewardButton.onClick.AddListener(() =>
        {
            GameManager.Instance.PLayRewardedAddSeconds();
        });
    }

    public void StartLoseGameMenu()
    {
        //================        
        YandexMetrica.Send("lost-" + Globals.CurrentLevel);
        //================

        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.zero;
        StartCoroutine(play());
    }

    private IEnumerator play()
    {
        restartPanel.SetActive(false);
        rewardPanel.SetActive(false);


        //===========================================
        Translation lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        GetMoreText.text = lang.GetMoreTextReward;
        loseText.text = lang.LoseText;
        SoundController _sound = SoundController.Instance;
        GameManager gm = GameManager.Instance;

        gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(0.1f);

        if (Globals.CurrentLevel > 1 && (DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds > Globals.REWARDED_COOLDOWN && !gm.IsSecondsAddedReward)
        {
            float secondsToAdd = GameManager.HowManySecondsToAddForRewarded(Globals.CurrentLevel);

            rewardPanel.SetActive(true);
            rewardPanel.transform.localScale = Vector3.zero;
            rewardPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
            SecondsText.text =  $"+{secondsToAdd} {lang.SecondsText}";
        }

        restartPanel.SetActive(true);
        restartPanel.transform.localScale = Vector3.zero;
        restartPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);

        if (rewardPanel.activeSelf)
        {
            StartCoroutine(playShake(rewardIcon.transform));
        }
        else
        {
            gm.regionController.Location().gameObject.SetActive(false);
        }
        

        //======================================
    }

    private IEnumerator playShake(Transform _transform)
    {
        while (true)
        {
            _transform.DOShakeScale(0.5f, 0.5f, 30).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(1f);
        }
    }

    
}
