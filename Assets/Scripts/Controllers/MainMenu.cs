using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AssetManager assetManager;
    [SerializeField] private SoundController sound;

    [SerializeField] private Ambient ambient;

    [Header("territories")]
    [SerializeField] private GameObject Visual01;
    [SerializeField] private GameObject Map01;

    [Header("   ")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform mainCameraBody;
    [SerializeField] private Transform cameraPosition1;
    [SerializeField] private Transform cameraPosition2;

    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI playButtonText;

    [Header("helper for 1st level")]
    [SerializeField] private GameObject press1stLevel;
    [SerializeField] private TextMeshProUGUI press1stLevelText;

    [Header("stars")]
    [SerializeField] private GameObject starsPanel;
    [SerializeField] private TextMeshProUGUI starsText;

    private Ray ray;
    private RaycastHit hit;
    
    private bool isVisual1Ended, isLevelChosing;
    private Translation lang;

    private void Awake()
    {
        Screen.SetResolution(1200, 600, true);
        UIManager.BackImageBlack(true, 0);
        UIManager.BackImageBlack(false, 1f);

        if (Globals.IsInitiated)
        {
            if (Globals.IsSoundOn)
            {
                AudioListener.volume = 1f;
            }
            else
            {
                AudioListener.volume = 0;
            }
        }

        starsPanel.SetActive(false);
        press1stLevel.SetActive(false);
        Visual01.SetActive(false);
        Map01.SetActive(false);

        
        mainCameraBody.position = Vector3.zero;
        mainCameraBody.rotation = Quaternion.identity;
        playButton.gameObject.SetActive(false);

        ambient.SetData(AmbientType.forest);

        playButton.onClick.AddListener(() => 
        {
            if (isVisual1Ended)
            {
                sound.PlayUISound(SoundsUI.positive);
                getToLevels();
            }            
        });
    }

    private void Start()
    {
        if (!Globals.IsInitiated)
        {            
            StartCoroutine(stage1());
        }
        else
        {
            

            getToLevels();
        }
    }

    private void getToLevels()
    {
        isLevelChosing = true;
        starsPanel.SetActive(true);
        SetStarsUI();

        int lastLevel = 0;

        for (int i = 1; i < Globals.MainPlayerData.Progress1.Length; i++)
        {
            if (i == 0)
            {
                lastLevel = i;
                break;
            }
        }

        if (lastLevel < 11)
        {
            StartCoroutine(playMap01());
        }
    }

    private void Update()
    {
        if (YandexGame.SDKEnabled && !Globals.IsInitiated)
        {
            Globals.IsInitiated = true;            

            SaveLoadManager.Load();

            print("SDK enabled: " + YandexGame.SDKEnabled);
            Globals.CurrentLanguage = YandexGame.EnvironmentData.language;
            print("language set to: " + Globals.CurrentLanguage);

            Globals.IsMobilePlatform = YandexGame.EnvironmentData.isMobile;
            print("platform mobile: " + Globals.IsMobilePlatform);

            if (Globals.MainPlayerData.S == 1)
            {
                Globals.IsSoundOn = true;
                AudioListener.volume = 1;
            }
            else
            {
                Globals.IsSoundOn = false;
                AudioListener.volume = 0;
            }

            print("sound is: " + Globals.IsSoundOn);

            if (Globals.TimeWhenStartedPlaying == DateTime.MinValue)
            {
                Globals.TimeWhenStartedPlaying = DateTime.Now;
                Globals.TimeWhenLastInterstitialWas = DateTime.Now;
                Globals.TimeWhenLastRewardedWas = DateTime.Now;
            }

            Localize();
            playButton.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && isLevelChosing)
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 50))
            {
                if (hit.collider.TryGetComponent(out ProgressPointController progressPoint))
                {
                    if (progressPoint.IsActivated && Globals.CurrentStars >= progressPoint.CurrentLimit)
                    {
                        StartCoroutine(startLevel(progressPoint.CurrentLevel));
                    }
                }
            }
        }
    }

    private IEnumerator startLevel(int level)
    {
        SoundController.Instance.PlayUISound(SoundsUI.positive);

        Globals.CurrentLevel = level;        
        UIManager.BackImageBlack(true, 1f);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("desert");
    }

    private void Localize()
    {
        lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();

        playButtonText.text = lang.PlayText;
        press1stLevelText.text = lang.PressFirstLevelText_MainMenu;
    }

    private IEnumerator stage1()
    {
        Visual01.SetActive(true);

        playButton.gameObject.SetActive(true);
        playButton.transform.DOPunchPosition(Vector3.one * 50, 0.3f).SetEase(Ease.OutQuad);
        
        mainCameraBody.DOMove(cameraPosition1.position, 1f).SetEase(Ease.OutSine);
        mainCameraBody.DORotate(cameraPosition1.eulerAngles, 1f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(0.2f);
        sound.PlayUISound(SoundsUI.pop);
        isVisual1Ended = true;
    }

    private IEnumerator playMap01()
    {
        ambient.SetData(AmbientType.desert);

        Visual01.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutSine);        
        playButton.gameObject.SetActive(false);
        Map01.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        Visual01.SetActive(false);
        mainCameraBody.DOMove(cameraPosition2.position, 1f).SetEase(Ease.OutSine);
        mainCameraBody.DORotate(cameraPosition2.eulerAngles, 1f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(1f);
        if (GetLastLevel() == 1) press1stLevel.SetActive(true);
        
    }

    public static int GetLastLevel()
    {
        int lastLevel = 0;

        for (int i = 1; i < Globals.MainPlayerData.Progress1.Length; i++)
        {
            if (Globals.MainPlayerData.Progress1[i] == 0)
            {
                lastLevel = i;
                break;
            }
        }

        return lastLevel;
    }

    public static int GetStarsAmount()
    {
        int amount = 0;

        if (Globals.MainPlayerData != null)
        {
            for (int i = 1; i < Globals.MainPlayerData.Progress1.Length; i++)
            {
                amount += Globals.MainPlayerData.Progress1[i];
            }
        }
        

        return amount;
    }

    public static void SetStarsUI()
    {
        Vector2 size = Vector2.zero;
        int stars = GetStarsAmount();

        GameObject.Find("StarsAmount").GetComponent<TextMeshProUGUI>().text = stars.ToString();

        if (stars > 99)
        {
            size = new Vector2(151, 61);
        }
        else if (stars > 9)
        {
            size = new Vector2(126, 61);
        }
        else
        {
            size = new Vector2(98, 61);
        }
        
        GameObject.Find("StarsBackImage").GetComponent<RectTransform>().sizeDelta = size;
    }
}
