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
    [SerializeField] private GameObject Map02;

    [Header("   ")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform mainCameraBody;
    [SerializeField] private Transform cameraPosition1;
    [SerializeField] private Transform cameraPosition2;
    [SerializeField] private Transform cameraPosition3map02;

    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI playButtonText;

    [Header("helper for 1st level")]
    [SerializeField] private GameObject press1stLevel;
    [SerializeField] private TextMeshProUGUI press1stLevelText;

    [Header("stars")]
    [SerializeField] private GameObject starsPanel;
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private GameObject getMoreStarsInfo;
    [SerializeField] private TextMeshProUGUI getMoreStarsInfoText;

    [Header("reset")]
    [SerializeField] private GameObject resetPanel;
    [SerializeField] private TextMeshProUGUI resetText;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button resetOK;
    [SerializeField] private Button resetNO;

    [Header("desert")]
    [SerializeField] private GameObject part1;
    [SerializeField] private GameObject part2;
    [SerializeField] private GameObject part3;
    private bool isMap01Done;
    private bool isMap02Done;

    [Header("forest")]
    [SerializeField] private GameObject part4;
    [SerializeField] private GameObject part5;
    [SerializeField] private GameObject part6;


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
        Map02.SetActive(false);
        getMoreStarsInfo.SetActive(false);



        mainCameraBody.position = Vector3.zero;
        mainCameraBody.rotation = Quaternion.identity;
        playButton.gameObject.SetActive(false);

        ambient.SetData(AmbientType.forest);

        //==================reset===========================
        resetPanel.SetActive(false);
        resetButton.gameObject.SetActive(false);

        playButton.onClick.AddListener(() => 
        {
            if (isVisual1Ended)
            {
                sound.PlayUISound(SoundsUI.positive);
                getToLevels();
            }            
        });

        resetButton.onClick.AddListener(() =>
        {
            if (resetPanel.activeSelf) return;
            SoundController.Instance.PlayUISound(SoundsUI.lose);

            resetPanel.SetActive(true);
        });

        resetOK.onClick.AddListener(() =>
        {
            StartCoroutine(playReset());
        });

        resetNO.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundsUI.error);
            resetPanel.SetActive(false);
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
            if (YandexGame.EnvironmentData.isDesktop)
            {
                YandexGame.StickyAdActivity(false);
            }

            resetButton.gameObject.SetActive(true);

            if (Globals.IsMainScreen)
            {
                StartCoroutine(stage1());
            }
            else
            {
                getToLevels();
            }

            
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
            if (Globals.MainPlayerData.Progress1[i] == 0)
            {
                lastLevel = i;
                break;
            }
        }

        print("what is last: " + lastLevel);

        if (lastLevel < 11)
        {
            StartCoroutine(playMap01());
        }
        else if (lastLevel < 21)
        {
            StartCoroutine(playMap02());
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
                if (hit.collider.TryGetComponent(out ProgressPointController progressPoint) && !Globals.IsInfoActive)
                {
                    if (progressPoint.IsActivated && GetStarsAmount() >= progressPoint.CurrentLimit)
                    {
                        StartCoroutine(startLevel(progressPoint.CurrentLevel));
                    }
                    else if (progressPoint.IsActivated && GetStarsAmount() < progressPoint.CurrentLimit)
                    {
                        if (!getMoreStarsInfo.activeSelf)
                        {
                            SoundController.Instance.PlayUISound(SoundsUI.error);
                            getMoreStarsInfo.SetActive(true);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Globals.CurrentLevel = GetLastLevel();
            AddStarsUI(3);
            SceneManager.LoadScene("menu");
        }
    }

    private IEnumerator startLevel(int level)
    {
        SoundController.Instance.PlayUISound(SoundsUI.positive);

        Globals.IsMainScreen = false;

        Globals.CurrentLevel = level;        
        UIManager.BackImageBlack(true, 1f);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(GetLevelName(level));
    }
    public static string GetLevelName(int level)
    {
        if (level < 31)
        {
            return "desert";
        }

        return "";
    }

    private IEnumerator playReset()
    {
        Globals.IsInitiated = false;

        SoundController.Instance.PlayUISound(SoundsUI.positive);

        Globals.MainPlayerData = new PlayerData();
        SaveLoadManager.Save();

        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("menu");
    }

    private void Localize()
    {
        lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();

        playButtonText.text = lang.PlayText;
        press1stLevelText.text = lang.PressFirstLevelText_MainMenu;
        resetText.text = lang.AllProgressWillBeReset;
        getMoreStarsInfoText.text = lang.GetMoreStars;
    }

    private IEnumerator stage1()
    {
        Visual01.SetActive(true);

        playButton.gameObject.SetActive(true);
        resetButton.gameObject.SetActive(true);
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

        if (!isMap01Done)
        {
            isMap01Done = true;
            showMapPart(part1, Vector3.zero, Vector3.zero, Map01.transform);
            showMapPart(part2, new Vector3(8.25f, 0, 4.75f), Vector3.zero, Map01.transform);
            showMapPart(part3, new Vector3(0, 0, 9.5f), Vector3.zero, Map01.transform);
        }        

        Visual01.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutSine);        
        playButton.gameObject.SetActive(false);
        Map01.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        Visual01.SetActive(false);
        mainCameraBody.DOMove(cameraPosition2.position, 0.7f).SetEase(Ease.OutSine);
        mainCameraBody.DORotate(cameraPosition2.eulerAngles, 0.7f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(1f);
        if (GetLastLevel() == 1) press1stLevel.SetActive(true);        
    }


    private IEnumerator playMap02()
    {
        ambient.SetData(AmbientType.desert);

        if (!isMap01Done)
        {
            isMap01Done = true;
            showMapPart(part1, Vector3.zero, Vector3.zero, Map01.transform);
            showMapPart(part2, new Vector3(8.25f, 0, 4.75f), Vector3.zero, Map01.transform);
            showMapPart(part3, new Vector3(0, 0, 9.5f), Vector3.zero, Map01.transform);
        }

        if (!isMap02Done)
        {
            isMap02Done = true;
            showMapPart(part1, new Vector3(16.5f, 0, 9.5f), Vector3.zero, Map02.transform);
            showMapPart(part2, new Vector3(0, 0, 19f), new Vector3(0,-60,0), Map02.transform);
            showMapPart(part3, new Vector3(8.25f, 0, 14.25f), new Vector3(0, 120, 0), Map02.transform);
            showMapPart(part5, new Vector3(8.25f, 0, 23.75f), new Vector3(0,180,0), Map02.transform);
            showMapPart(part4, new Vector3(16.5f, 0, 19f), new Vector3(0,0,0), Map02.transform);
            showMapPart(part2, new Vector3(24.75f, 0, 14.25f), new Vector3(0,120,0), Map02.transform);
        }

        Visual01.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutSine);
        playButton.gameObject.SetActive(false);
        Map01.SetActive(true);
        Map02.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        Visual01.SetActive(false);
        mainCameraBody.DOMove(cameraPosition3map02.position, 0.7f).SetEase(Ease.OutSine);
        mainCameraBody.DORotate(cameraPosition3map02.eulerAngles, 0.7f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(1f);
    }

    private void showMapPart(GameObject obj, Vector3 pos, Vector3 rot, Transform place)
    {
        GameObject g = Instantiate(obj, place);
        g.transform.localPosition = pos;
        g.transform.localEulerAngles = rot;
        g.SetActive(true);
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

    public static void AddStarsUI(int amount)
    {
        Vector2 size = Vector2.zero;

        int stars = Globals.MainPlayerData.Progress1[Globals.CurrentLevel] + amount;
        
        Globals.MainPlayerData.Progress1[Globals.CurrentLevel] = stars;
        SaveLoadManager.Save();

        stars = GetStarsAmount();
        TextMeshProUGUI startText = GameObject.Find("StarsAmount").GetComponent<TextMeshProUGUI>();
        startText.text = stars.ToString();

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
