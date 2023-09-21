using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Win conditions")]
    public float GameTime;
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

    public void AddTaxi() 
    {
        print("add taxi");
        TaxiCurrent++;
        OnChangedConditionsAmount?.Invoke();        
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
    [SerializeField] private WinGameMenu winGameMenu;

    public SoundController GetSoundUI() => sound;
    public AssetManager GetAssets() => assets;
    public Camera GetCamera() => mainCamera;
    public Ambient GetAmbient() => ambient;
    public Transform GetCameraBody() => mainCameraBody;

    private Transform mainCar;
    private UIManager uiManager;
    private InputController inputController;

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

        GetComponent<LevelGenerator>().InitLevel(Globals.CurrentLevel, this);

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

        //start game
        StartCoroutine(gameStartDelay());
    }

    
    private void Update()
    {
        if (IsGameStarted && uiManager.GetTimeLeft() <= 0)
        {
            Debug.LogError("game lost!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            SetGameStatus(false);
        }

        if (IsGameStarted && TaxiCount == TaxiCurrent && VanCount == VanCurrent && AmbulanceCount == AmbulanceCurrent && TaxiManCount == TaxiManCurrent
            && VanCargoCount == VanCargoCurrent && AmbulanceManCount == AmbulanceManCurrent && RiddleCount == RiddleCurrent)
        {
            Debug.LogError("game win!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            SetGameStatus(false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetGameStatus(false);
            winGameMenu.StartWinGameMenu(0.2f, 2, 3);
        }
    }

    private IEnumerator gameStartDelay()
    {
        yield return new WaitForSeconds(2f);
        regionController.UpdateAll();
        SetGameStatus(true);
    }

    public void SetGameStatus(bool isStarted)
    {
        uiManager.GameTimerStatus(isStarted);
        IsGameStarted = isStarted;
    }

    public void SetScreenFOV(LevelScale scale)
    {
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
        assets.CarDestroEffectPool = new ObjectPool(10, GetAssets().CarDestroEffect);

        if (TaxiCount > 0)
        {
            assets.TaxiPool = new ObjectPool(10, assets.GetVehicle(Vehicles.taxi), regionController.Location());
            assets.TaxiSignPool = new ObjectPool(10, assets.TaxiSign);
        }
        else
        {
            assets.TaxiPool = new ObjectPool(1, assets.GetVehicle(Vehicles.taxi), regionController.Location());
            assets.TaxiSignPool = new ObjectPool(1, assets.TaxiSign);
        }

        if (VanCount > 0)
        {
            assets.VanPool = new ObjectPool(10, assets.GetVehicle(Vehicles.van), regionController.Location());
            assets.VanSignPool = new ObjectPool(10, assets.VanSign);
        }
        else
        {
            assets.VanPool = new ObjectPool(1, assets.GetVehicle(Vehicles.van), regionController.Location());
            assets.VanSignPool = new ObjectPool(1, assets.VanSign);
        }

        if (AmbulanceCount > 0)
        {
            assets.AmbulancePool = new ObjectPool(10, assets.GetVehicle(Vehicles.ambulance), regionController.Location());
            assets.AmbulanceSignPool = new ObjectPool(10, assets.AmbulanceSign);
        }
        else
        {
            assets.AmbulancePool = new ObjectPool(1, assets.GetVehicle(Vehicles.ambulance), regionController.Location());
            assets.AmbulanceSignPool = new ObjectPool(1, assets.AmbulanceSign);
        }

    }

}

public enum LevelScale
{
    small,
    medium,
    large
}
