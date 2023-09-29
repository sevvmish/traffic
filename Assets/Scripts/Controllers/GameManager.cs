using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Win conditions")]
    public float GameTime;

    public float StarsLimitTimer;
    public int StarsLimitMistakes;
    public int StarsLimitAccidents;

    //===============================================
    public int MistakesCurrent;
    public int AccidentsCurrent;

    public bool IsGameStarted { get; private set; } = false;
    //===============================
    public int RiddleCount;
    public int RiddleCurrent;

    public int TaxiCount;
    public int TaxiCurrent { get; private set; }
    public int TaxiManCount;
    public int TaxiManCurrent { get; private set; }
    //===============================
    public int VanCount;
    public int VanCurrent { get; private set; }
    public int VanCargoCount;
    public int VanCargoCurrent { get; private set; }
    //===============================
    public int AmbulanceCount;
    public int AmbulanceCurrent { get; private set; }
    public int AmbulanceManCount;
    public int AmbulanceManCurrent { get; private set; }
    
    public Action OnChangedConditionsAmount;
    public bool IsSecondsAddedReward { get; private set; }

    public void AddTaxi() 
    {
        print("add taxi");
        TaxiCurrent++;
        OnChangedConditionsAmount?.Invoke();        
    }

    public void AddMistake()
    {
        MistakesCurrent++;
    }

    public void AddAccident()
    {
        AccidentsCurrent++;
    }

    public void AddTaxiMan()
    {
        print("add taxi man");
        TaxiManCurrent++;
        OnChangedConditionsAmount?.Invoke();
    }

    public void RemoveTaxi()
    {
        print("rem taxi");
        if (TaxiCurrent > 0) TaxiCurrent--;
        OnChangedConditionsAmount?.Invoke();        
    }

    public void RemoveTaxiMan()
    {
        print("rem taxi man");
        if (TaxiManCurrent > 0) TaxiManCurrent--;
        OnChangedConditionsAmount?.Invoke();
    }

    public void AddVan()
    {
        VanCurrent++;
        OnChangedConditionsAmount?.Invoke();        
    }
    public void AddVanCargo()
    {
        VanCargoCurrent++;
        OnChangedConditionsAmount?.Invoke();
    }
    public void RemoveVan()
    {
        if (VanCurrent > 0) VanCurrent--;
        OnChangedConditionsAmount?.Invoke();        
    }
    public void RemoveVanCargo()
    {
        if (VanCargoCurrent > 0) VanCargoCurrent--;
        OnChangedConditionsAmount?.Invoke();
    }
    public void AddAmbulance()
    {
        AmbulanceCurrent++;
        OnChangedConditionsAmount?.Invoke();        
    }
    public void AddAmbulanceMan()
    {
        AmbulanceManCurrent++;
        OnChangedConditionsAmount?.Invoke();
    }
    public void RemoveAmbulance()
    {
        if (AmbulanceCurrent > 0) AmbulanceCurrent--;
        OnChangedConditionsAmount?.Invoke();        
    }
    public void RemoveAmbulanceMan()
    {
        if (AmbulanceManCurrent > 0) AmbulanceManCurrent--;
        OnChangedConditionsAmount?.Invoke();
    }


    public RegionController regionController { get; private set; }

    [Header("options")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform mainCameraBody;
    [SerializeField] private Joystick joystick;
    [SerializeField] private SoundController sound;
    [SerializeField] private Ambient ambient;
    [SerializeField] private AssetManager assets;

    [Header("end menues and adv")]
    [SerializeField] private WinGameMenu winGameMenu;
    [SerializeField] private LoseGameMenu loseGameMenu;
    [SerializeField] private Rewarded rewarded;
    [SerializeField] private Interstitial interstitial;
    [SerializeField] private PlusStarMenu plusStarMenu;

    private LevelScale levelScale;

    public float CameraZShift;

    public SoundController GetSoundUI() => sound;
    public AssetManager GetAssets() => assets;
    public Camera GetCamera() => mainCamera;
    public Ambient GetAmbient() => ambient;
    public Transform GetCameraBody() => mainCameraBody;
    public UIManager GetUI() => GetComponent<UIManager>();

    private Transform mainCar;
    private UIManager uiManager;
    private InputController inputController;
    private bool isAfterAdv;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Screen.SetResolution(1200, 600, true);
        Globals.IsInfoActive = false;

        //=====TO DELETE======
        //Globals.CurrentLevel = 16;
        //Globals.MainPlayerData = new PlayerData();
        //====================

        if (YandexGame.EnvironmentData.isDesktop)
        {
            YandexGame.StickyAdActivity(true);            
        }

        GetComponent<LevelGenerator>().InitLevel(Globals.CurrentLevel, this);

        CameraZShift = CameraZShift != 0 ? CameraZShift : -9;
        mainCameraBody.position = new Vector3(0, 15, CameraZShift);

        UIManager.BackImageBlack(true, 0);
        UIManager.BackImageBlack(false, 1f);

        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1f;
        }
        else
        {
            AudioListener.volume = 0;
        }

        if (TaxiCount == 0 && VanCount == 0 && AmbulanceCount == 0 && TaxiManCount == 0 
            && VanCargoCount == 0 && AmbulanceManCount == 0 && RiddleCount == 0)
        {
            UnityEngine.Debug.LogError("Error in match conditions!");
        }

        MainMenu.SetStarsUI();

        regionController = GetComponent<RegionController>();
        inputController = GetComponent<InputController>();
        uiManager = GetComponent<UIManager>();

        regionController.SetData(mainCameraBody);
        //InitPools();

        

        mainCameraBody.position = new Vector3(
            (regionController.xBorder.x + regionController.xBorder.y) / 2,
            mainCameraBody.position.y,
            (regionController.zBorder.x + regionController.zBorder.y) / 2);

        inputController.SetData(mainCamera, mainCameraBody, joystick, mainCar, regionController.Location(), regionController);
                
        uiManager.SetData(GameTime);

        InitPools();

        winGameMenu.gameObject.SetActive(false);
        loseGameMenu.gameObject.SetActive(false);

        regionController.UpdateAll();

        //start game
        StartCoroutine(gameStartDelay());
    }

    
    private void Update()
    {
        if (IsGameStarted && uiManager.GetTimeLeft() <= 0)
        {
            Debug.LogError("game lost!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            StartCoroutine(handleLoseCondition());
        }

        if (IsGameStarted && TaxiCount <= TaxiCurrent && VanCount <= VanCurrent && AmbulanceCount <= AmbulanceCurrent && TaxiManCount <= TaxiManCurrent
            && VanCargoCount <= VanCargoCurrent && AmbulanceManCount <= AmbulanceManCurrent && RiddleCount <= RiddleCurrent)
        {
            Debug.LogError("game win!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            StartCoroutine(handleWinCondition());
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            plusStarMenu.StartPlusStar();
            loseGameMenu.gameObject.SetActive(false);
        }
    }

    public void AddSecondsAndContinue(float seconds)
    {
        //if (IsSecondsAddedReward) return;
                
        IsSecondsAddedReward = true;
        uiManager.AddSeconds(seconds);
        SetGameStatus(true);
        uiManager.TurnOnOptions();
    }

    public void AddSecondsAndContinue()
    {
        //if (IsSecondsAddedReward) throw new System.NotImplementedException();

        float seconds = HowManySecondsToAddForRewarded(Globals.CurrentLevel);

        IsSecondsAddedReward = true;
        uiManager.AddSeconds(seconds);
        SetGameStatus(true);
        uiManager.TurnOnOptions();
    }

    public void PLayRewardedPlusStar()
    {
        uiManager.TurnOffOptions();
        winGameMenu.gameObject.SetActive(false);
        rewarded.OnRewardedEndedOK = plusStarMenu.StartPlusStar;
        rewarded.OnError = backToLevels;
        rewarded.ShowRewardedVideo();
    }

    public void PLayRewardedAddSeconds()
    {
        uiManager.TurnOffOptions();
        loseGameMenu.gameObject.SetActive(false);
        rewarded.OnRewardedEndedOK = AddSecondsAndContinue;
        rewarded.OnError = RestartLevel;
        rewarded.ShowRewardedVideo();
    }


    private IEnumerator handleWinCondition()
    {
        SoundController.Instance.PlayUISound(SoundsUI.win);
        SetGameStatus(false);
        uiManager.SetDataPanel(false);
        yield return new WaitForSeconds(1);

        
        regionController.Location().gameObject.SetActive(false);
        winGameMenu.StartWinGameMenu();
        uiManager.TurnOffOptions();
    }

    private IEnumerator handleLoseCondition()
    {
        SoundController.Instance.PlayUISound(SoundsUI.lose);
        SetGameStatus(false);
        yield return new WaitForSeconds(1);


        //regionController.Location().gameObject.SetActive(false);
        loseGameMenu.StartLoseGameMenu();
        uiManager.TurnOffOptions();
    }

    private IEnumerator gameStartDelay()
    {
        yield return new WaitForSeconds(2f);
        regionController.UpdateAll();
        SetGameStatus(true);
    }

    public void SetGameStatus(bool isStarted)
    {
        print("game made to: " + isStarted);
        uiManager.GameTimerStatus(isStarted);
        IsGameStarted = isStarted;
    }

    public void RestartLevel()
    {
        uiManager.TurnOffOptions();
        uiManager.SetDataPanel(false);

        if (Globals.CurrentLevel > 1
            && (DateTime.Now - Globals.TimeWhenLastInterstitialWas).TotalSeconds > Globals.INTERSTITIAL_COOLDOWN
            && (DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds > (Globals.REWARDED_COOLDOWN / 2))
        {
            isAfterAdv = true;
            UIManager.BackImageBlack(true, 0f);
            interstitial.OnEnded = RestartLevel;
            interstitial.ShowInterstitialVideo();
        }
        else
        {
            StartCoroutine(restartLevel());
        }        
    }
    private IEnumerator restartLevel()
    {
        if (!isAfterAdv) SoundController.Instance.PlayUISound(SoundsUI.positive);

        UIManager.BackImageBlack(true, 1f);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(MainMenu.GetLevelName(Globals.CurrentLevel));
    }

    private void backToLevels()
    {
        if (Globals.CurrentLevel > 1 
            && (DateTime.Now - Globals.TimeWhenLastInterstitialWas).TotalSeconds > Globals.INTERSTITIAL_COOLDOWN 
            && (DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds > (Globals.REWARDED_COOLDOWN/2))
        {
            isAfterAdv = true;
            UIManager.BackImageBlack(true, 0f);
            interstitial.OnEnded = backToLevels;
            interstitial.ShowInterstitialVideo();
        }
        else
        {
            BackToMainMenu(false);
        }        
    }
        



    public void BackToMainMenu(bool isMainScreenOn)
    {
        uiManager.TurnOffOptions();
        uiManager.SetDataPanel(false);

        if (Globals.CurrentLevel > 1 && !isMainScreenOn
            && (DateTime.Now - Globals.TimeWhenLastInterstitialWas).TotalSeconds > Globals.INTERSTITIAL_COOLDOWN
            && (DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds > (Globals.REWARDED_COOLDOWN/2))
        {
            isAfterAdv = true;
            UIManager.BackImageBlack(true, 0f);
            interstitial.OnEnded = backToLevels;
            interstitial.ShowInterstitialVideo();
            return;
        }
        
        StartCoroutine(loadMenu(isMainScreenOn));
    }
    private IEnumerator loadMenu(bool isMainScreenOn)
    {
        if (!isAfterAdv) SoundController.Instance.PlayUISound(SoundsUI.positive);
        Globals.IsMainScreen = isMainScreenOn;

        UIManager.BackImageBlack(true, 1f);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("menu");
    }

    
    public void SetScreenFOV(LevelScale scale)
    {
        levelScale = scale; 

        switch(scale)
        {
            case LevelScale.small:
                if (Globals.IsMobilePlatform)
                {
                    mainCamera.fieldOfView = 55;
                }
                else
                {
                    mainCamera.fieldOfView = 65;
                }
                
                break;

            case LevelScale.medium:
                if (Globals.IsMobilePlatform)
                {
                    mainCamera.fieldOfView = 65;
                }
                else
                {
                    mainCamera.fieldOfView = 75;
                }
                break;

            case LevelScale.large:
                if (Globals.IsMobilePlatform)
                {
                    mainCamera.fieldOfView = 75;
                }
                else
                {
                    mainCamera.fieldOfView = 85;
                }
                break;
        }
    }

    private void InitPools()
    {
        int objAmount = 10;

        switch(levelScale)
        {
            case LevelScale.small:
                objAmount = 4;
                break;

            case LevelScale.medium:
                objAmount = 8;
                break;

            case LevelScale.large:
                objAmount = 16;
                break;
        }

        assets.CarDestroEffectPool = new ObjectPool(objAmount*2, GetAssets().CarDestroEffect);


        if (TaxiCount > 0)
        {
            assets.TaxiPool = new ObjectPool(objAmount, assets.GetVehicle(Vehicles.taxi), regionController.Location());
            assets.TaxiSignPool = new ObjectPool(objAmount, assets.TaxiSign);
        }
        else
        {
            assets.TaxiPool = new ObjectPool(objAmount/2, assets.GetVehicle(Vehicles.taxi), regionController.Location());
            assets.TaxiSignPool = new ObjectPool(objAmount/2, assets.TaxiSign);
        }

        if (VanCount > 0)
        {
            assets.VanPool = new ObjectPool(objAmount, assets.GetVehicle(Vehicles.van), regionController.Location());
            assets.VanSignPool = new ObjectPool(objAmount, assets.VanSign);
        }
        else
        {
            assets.VanPool = new ObjectPool(objAmount/2, assets.GetVehicle(Vehicles.van), regionController.Location());
            assets.VanSignPool = new ObjectPool(objAmount/2, assets.VanSign);
        }

        if (AmbulanceCount > 0)
        {
            assets.AmbulancePool = new ObjectPool(objAmount, assets.GetVehicle(Vehicles.ambulance), regionController.Location());
            assets.AmbulanceSignPool = new ObjectPool(objAmount, assets.AmbulanceSign);
        }
        else
        {
            assets.AmbulancePool = new ObjectPool(objAmount/2, assets.GetVehicle(Vehicles.ambulance), regionController.Location());
            assets.AmbulanceSignPool = new ObjectPool(objAmount/2, assets.AmbulanceSign);
        }

    }

    public static float HowManySecondsToAddForRewarded(int level)
    {
        float result = (int)(Instance.GameTime * Globals.HOW_MANY_SEC_ADD_FOR_REWARD_KOEFF);

        return result > 8 ? result : 8;
    }

    
}

public enum LevelScale
{
    small,
    medium,
    large
}
