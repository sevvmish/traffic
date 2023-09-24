using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject restartPanel;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button rewardButton;
    [SerializeField] private GameObject rewardIcon;

    [SerializeField] private TextMeshProUGUI GetMoreText;
    [SerializeField] private TextMeshProUGUI SecondsText;

    void Start()
    {
        
        restartButton.onClick.AddListener(() => 
        {
            StartCoroutine(restartLevel());
        });
    }

    public void StartLoseGameMenu()
    {
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
        SoundController _sound = SoundController.Instance;
        GameManager gm = GameManager.Instance;

        gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(0.1f);

        if ((DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds > 2/*Globals.REW_COOLDOWN*/)
        {
            float secondsToAdd = 30;
            rewardPanel.SetActive(true);
            rewardPanel.transform.localScale = Vector3.zero;
            rewardPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
            SecondsText.text =  $"+{secondsToAdd} {lang.SecondsText}";
        }

        restartPanel.SetActive(true);
        restartPanel.transform.localScale = Vector3.zero;
        restartPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);

        if (rewardPanel.activeSelf) StartCoroutine(playShake(rewardIcon.transform));

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

    private IEnumerator restartLevel()
    {
        SoundController.Instance.PlayUISound(SoundsUI.positive);

        UIManager.BackImageBlack(true, 1f);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(MainMenu.GetLevelName(Globals.CurrentLevel));
    }
}
