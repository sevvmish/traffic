using DG.Tweening;
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


    // Start is called before the first frame update
    void Start()
    {
        Translation lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        /*
        allStar1.SetActive(false);
        allStar2.SetActive(false);
        allStar3.SetActive(false);
        fullStar1.SetActive(false); 
        fullStar2.SetActive(false);
        fullStar3.SetActive(false);

        stat1.SetActive(false);
        stat2.SetActive(false);
        stat3.SetActive(false);

        rewardPanel.SetActive(false);
        nextPanel.SetActive(false);
        */
        stat1Text.text = lang.SpeedOfGameWinMenu;
        stat2Text.text = lang.MistakesOfGameWinMenu;
        stat3Text.text = lang.AccidentsOfGameWinMenu;
        nextText.text = lang.NextOfGameWinMenu;
        stat1ValueText.text = "";
        stat2ValueText.text = "";
        stat3ValueText.text = "";
    }

    public void StartWinGameMenu(float _speed, int mistakes, int accidents)
    {
        gameObject.SetActive(true);
        StartCoroutine(play(_speed, mistakes, accidents));
    }
    private IEnumerator play(float _speed, int mistakes, int accidents)
    {
        allStar1.SetActive(false);
        allStar2.SetActive(false);
        allStar3.SetActive(false);
        fullStar1.SetActive(false);
        fullStar2.SetActive(false);
        fullStar3.SetActive(false);

        stat1.SetActive(false);
        stat2.SetActive(false);
        stat3.SetActive(false);

        rewardPanel.SetActive(false);
        nextPanel.SetActive(false);

        //===========================================

        SoundController _sound = SoundController.Instance;

        
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(0.2f);

        allStar1.SetActive(true);
        allStar2.SetActive(true);
        allStar3.SetActive(true);

        allStar1.transform.localScale = Vector3.zero;
        allStar2.transform.localScale = Vector3.zero;
        allStar3.transform.localScale = Vector3.zero;

        allStar1.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.pop);
        yield return new WaitForSeconds(0.2f);
        
        allStar2.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.pop);
        yield return new WaitForSeconds(0.2f);
        
        allStar3.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        _sound.PlayUISound(SoundsUI.pop);
        yield return new WaitForSeconds(0.2f);


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

        stat1ValueText.text = _speed.ToString("f0") + "%";
        stat2ValueText.text = mistakes.ToString();
        stat3ValueText.text = accidents.ToString();

        //======================================


    }
}
