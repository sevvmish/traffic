using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinGameMenu : MonoBehaviour
{
    [Header("STARS")]
    [SerializeField] private GameObject allStar1;
    [SerializeField] private GameObject allStar2;
    [SerializeField] private GameObject allStar3;

    [SerializeField] private GameObject fullStar1;
    [SerializeField] private GameObject fullStar2;
    [SerializeField] private GameObject fullStar3;

    [SerializeField] private GameObject doneStar1;
    [SerializeField] private GameObject doneStar2;
    [SerializeField] private GameObject doneStar3;

    [Header("STATISTICS")]
    [SerializeField] private GameObject stat1;
    [SerializeField] private TextMeshProUGUI stat1Text;
    [SerializeField] private TextMeshProUGUI stat1ValueText;

    [SerializeField] private GameObject stat2;
    [SerializeField] private TextMeshProUGUI stat2Text;
    [SerializeField] private TextMeshProUGUI stat2ValueText;

    [SerializeField] private GameObject stat3;
    [SerializeField] private TextMeshProUGUI stat3Text;
    [SerializeField] private TextMeshProUGUI stat3ValueText;

    [Header("Buttons")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private Button rewardButton; 
    [SerializeField] private GameObject starIcon;

    [SerializeField] private GameObject nextPanel;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI nextText;

    [SerializeField] private TextMeshProUGUI winText;


    // Start is called before the first frame update
    void Start()
    {
        Translation lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        
        stat1Text.text = lang.SpeedOfGameWinMenu;
        stat2Text.text = lang.MistakesOfGameWinMenu;
        stat3Text.text = lang.AccidentsOfGameWinMenu;
        nextText.text = lang.NextOfGameWinMenu;
        stat1ValueText.text = "";
        stat2ValueText.text = "";
        stat3ValueText.text = "";

        winText.text = lang.WinText;

        nextButton.onClick.AddListener(() => 
        {
            GameManager.Instance.BackToMainMenu(false);
        });

        rewardButton.onClick.AddListener(() =>
        {
            GameManager.Instance.PLayRewardedPlusStar();
        });
    }

    public void StartWinGameMenu()
    {
        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.zero;
        StartCoroutine(play());
    }
    private IEnumerator play()
    {
        allStar1.SetActive(false);
        allStar2.SetActive(false);
        allStar3.SetActive(false);
        fullStar1.SetActive(false);
        fullStar2.SetActive(false);
        fullStar3.SetActive(false);
        doneStar1.SetActive(false);
        doneStar2.SetActive(false);
        doneStar3.SetActive(false);

        stat1.SetActive(false);
        stat2.SetActive(false);
        stat3.SetActive(false);

        rewardPanel.SetActive(false);
        nextPanel.SetActive(false);

        stat1ValueText.gameObject.SetActive(false);
        stat2ValueText.gameObject.SetActive(false);
        stat3ValueText.gameObject.SetActive(false);

        //===========================================

        SoundController _sound = SoundController.Instance;
        GameManager gm = GameManager.Instance;

        gm.regionController.Location().gameObject.SetActive(false);
                
        gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(0.1f);

        allStar1.SetActive(true);
        allStar2.SetActive(true);
        allStar3.SetActive(true);

        allStar1.transform.localScale = Vector3.zero;
        allStar2.transform.localScale = Vector3.zero;
        allStar3.transform.localScale = Vector3.zero;

        allStar1.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.pop);
        yield return new WaitForSeconds(0.1f);
        
        allStar2.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.pop);
        yield return new WaitForSeconds(0.1f);
        
        allStar3.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.pop);
        yield return new WaitForSeconds(0.1f);


        //======================================

        stat1.SetActive(true);
        stat2.SetActive(true);
        stat3.SetActive(true);

        stat1.transform.localScale = Vector3.zero;
        stat2.transform.localScale = Vector3.zero;
        stat3.transform.localScale = Vector3.zero;

        stat1.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.tick);
        yield return new WaitForSeconds(0.2f);

        stat2.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.tick);
        yield return new WaitForSeconds(0.2f);

        stat3.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.tick);
        yield return new WaitForSeconds(0.2f);

        int starAmount = 3;
                
        if (gm.StarsLimitMistakes > 0 && gm.StarsLimitAccidents > 0)
        {
            starAmount = 1;
        }
        else if (gm.StarsLimitMistakes > 0 && gm.StarsLimitAccidents <= 0)
        {
            starAmount = 2;
        }
        else if (gm.StarsLimitAccidents > 0 && gm.StarsLimitMistakes <= 0)
        {
            starAmount = 2;
        }
        

        Color speedColor = Color.white;
        Color mistakesColor = Color.white;
        Color accidentsColor = Color.white;

        if ((gm.GetUI().GetTimeLeft() / gm.GameTime) >= gm.StarsLimitTimer)
        {
            starAmount++;
            speedColor = Color.green;
        }
        else
        {
            starAmount--;
            speedColor = Color.red;
        }

        if (gm.MistakesCurrent > gm.StarsLimitMistakes && gm.StarsLimitMistakes > 0)
        {
            starAmount--;
            mistakesColor = Color.red;
        }
        else if(gm.MistakesCurrent <= gm.StarsLimitMistakes && gm.StarsLimitMistakes > 0)
        {
            starAmount++;
            mistakesColor = Color.green;
        }

        if (gm.AccidentsCurrent > gm.StarsLimitAccidents && gm.StarsLimitAccidents > 0)
        {
            starAmount--;
            accidentsColor = Color.red;
        }
        else if(gm.AccidentsCurrent <= gm.StarsLimitAccidents && gm.StarsLimitAccidents > 0)
        {
            starAmount++;
            accidentsColor = Color.green;
        }

        if (starAmount < 1)
        {
            starAmount = 1;
        }
        else if (starAmount > 3)
        {
            starAmount = 3;
        }

        stat1ValueText.color = speedColor;
        stat2ValueText.color = mistakesColor;
        stat3ValueText.color = accidentsColor;

        stat1ValueText.text = (gm.GetUI().GetTimeLeft() / gm.GameTime * 100).ToString("f0") + "%";
        stat2ValueText.text = gm.MistakesCurrent == 0 ? "-" : gm.MistakesCurrent.ToString();
        stat3ValueText.text = gm.AccidentsCurrent == 0 ? "-" : gm.AccidentsCurrent.ToString();

        stat1ValueText.gameObject.SetActive(true);
        stat2ValueText.gameObject.SetActive(true);
        stat3ValueText.gameObject.SetActive(true);

        stat1ValueText.transform.localScale = Vector3.zero;
        stat2ValueText.transform.localScale = Vector3.zero;
        stat3ValueText.transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(0.5f);

        stat1ValueText.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.tick);
        yield return new WaitForSeconds(0.5f);

        stat2ValueText.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.tick);
        yield return new WaitForSeconds(0.5f);

        stat3ValueText.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.tick);
        yield return new WaitForSeconds(0.5f);

        int allreadyReceivedStars = Globals.MainPlayerData.Progress1[Globals.CurrentLevel];

        if (starAmount >= 1)
        {
            fullStar1.SetActive(true);
            fullStar1.transform.localScale = Vector3.zero;
            fullStar1.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            _sound.PlayUISound(SoundsUI.pop);
            yield return new WaitForSeconds(0.2f);
            if (allreadyReceivedStars>=1) doneStar1.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        if (starAmount >= 2)
        {
            fullStar2.SetActive(true);
            fullStar2.transform.localScale = Vector3.zero;
            fullStar2.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            _sound.PlayUISound(SoundsUI.pop);
            yield return new WaitForSeconds(0.2f);
            if (allreadyReceivedStars >= 2) doneStar2.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        if (starAmount >= 3)
        {
            fullStar3.SetActive(true);
            fullStar3.transform.localScale = Vector3.zero;
            fullStar3.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            _sound.PlayUISound(SoundsUI.pop);
            yield return new WaitForSeconds(0.2f);
            if (allreadyReceivedStars >= 3) doneStar3.SetActive(true);
        }

        int currentStars = MainMenu.GetStarsAmount();
        TextMeshProUGUI starsText = GameObject.Find("StarsAmount").GetComponent<TextMeshProUGUI>();

        if (starAmount >= 1 && allreadyReceivedStars < 1)
        {
            RectTransform rect1 = fullStar1.GetComponent<RectTransform>();
            rect1.DOAnchorPos(new Vector2(120 * 5, 200 * 5), 0.5f).SetEase(Ease.InOutSine);
            allStar1.transform.DOScale(Vector3.one * 0.5f, 0.5f);
            _sound.PlayUISound(SoundsUI.swallow);
            yield return new WaitForSeconds(0.3f);
            currentStars++;
            MainMenu.AddStarsUI(1);
        }

        if (starAmount >= 2 && allreadyReceivedStars < 2)
        {
            RectTransform rect2 = fullStar2.GetComponent<RectTransform>();
            rect2.DOAnchorPos(new Vector2(0, 200 * 5), 0.5f).SetEase(Ease.InOutSine);
            allStar2.transform.DOScale(Vector3.one * 0.5f, 0.5f);
            _sound.PlayUISound(SoundsUI.swallow);
            yield return new WaitForSeconds(0.3f);
            currentStars++;
            MainMenu.AddStarsUI(1);
        }

        if (starAmount >= 3 && allreadyReceivedStars < 3)
        {
            RectTransform rect3 = fullStar3.GetComponent<RectTransform>();
            rect3.DOAnchorPos(new Vector2(-120 * 5, 200 * 5), 0.5f).SetEase(Ease.InOutSine);
            allStar3.transform.DOScale(Vector3.one * 0.5f, 0.5f);
            _sound.PlayUISound(SoundsUI.swallow);
            yield return new WaitForSeconds(0.3f);
            currentStars++;
            MainMenu.AddStarsUI(1);
        }

        yield return new WaitForSeconds(0.5f);

        if (Globals.CurrentLevel > 1 && (DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds > Globals.REWARDED_COOLDOWN && starAmount < 3)
        {
            rewardPanel.SetActive(true);
            rewardPanel.transform.localScale = Vector3.zero;
            rewardPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
        }
                
        nextPanel.SetActive(true);
        nextPanel.transform.localScale = Vector3.zero;
        nextPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);

        if (rewardPanel.activeSelf) StartCoroutine(playShake(starIcon.transform));

        //======================================


    }

    private IEnumerator playShake(Transform _transform)
    {
        while (true)
        {
            _transform.DOShakeScale(0.5f, 0.5f, 30).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(1f);

            //transform.DOPunchScale(Vector3.one*0.2f, 0.3f).SetEase(Ease.OutQuad);
            //yield return new WaitForSeconds(0.7f);

            //transform.DOPunchPosition(Vector3.one * 0.2f, 0.3f).SetEase(Ease.OutQuad);
            //yield return new WaitForSeconds(0.7f);
        }


    }
}
