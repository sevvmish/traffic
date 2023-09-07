using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Win conditions")]
    public float GameTime;
    public int TaxiCount;
    public int TaxiCurrent { get; private set; }

    public int VanCount;
    public int VanCurrent { get; private set; }
    
    public int AmbulanceCount;
    public int AmbulanceCurrent { get; private set; }
    public Action OnChangedConditionsAmount;

    public void AddTaxi() 
    {
        print("add taxi");
        TaxiCurrent++;
        OnChangedConditionsAmount?.Invoke();        
    }
        
    public void RemoveTaxi()
    {
        print("rem taxi");
        if (TaxiCurrent > 0) TaxiCurrent--;
        OnChangedConditionsAmount?.Invoke();        
    }
    public void AddVan()
    {
        VanCurrent++;
        OnChangedConditionsAmount?.Invoke();        
    }
    public void RemoveVan()
    {
        if (VanCurrent > 0) VanCurrent--;
        OnChangedConditionsAmount?.Invoke();        
    }
    public void AddAmbulance()
    {
        AmbulanceCurrent++;
        OnChangedConditionsAmount?.Invoke();        
    }
    public void RemoveAmbulance()
    {
        if (AmbulanceCurrent > 0) AmbulanceCurrent--;
        OnChangedConditionsAmount?.Invoke();        
    }


    public RegionController regionController { get; private set; }

    [Header("options")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform mainCameraBody;
    [SerializeField] private Joystick joystick;
    [SerializeField] private SoundController sound;
    [SerializeField] private AmbientType ambientType;
    [SerializeField] private Ambient ambient;
    [SerializeField] private AssetManager assets;

    public SoundController GetSoundUI() => sound;
    public AssetManager GetAssets() => assets;

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
        UIManager.BackImageBlack(true, 0);
        UIManager.BackImageBlack(false, 1f);

        if (TaxiCount == 0 && VanCount == 0 && AmbulanceCount == 0)
        {
            UnityEngine.Debug.LogError("Error in match conditions!");
        }

        regionController = GetComponent<RegionController>();
        inputController = GetComponent<InputController>();
        uiManager = GetComponent<UIManager>();

        regionController.SetData(mainCameraBody);
        InitVehiclesPools();

        mainCameraBody.position = new Vector3(
            (regionController.xBorder.x + regionController.xBorder.y) / 2,
            mainCameraBody.position.y,
            (regionController.zBorder.x + regionController.zBorder.y) / 2);

        inputController.SetData(mainCamera, mainCameraBody, joystick, mainCar, regionController.Location(), regionController);
        ambient.SetData(ambientType);
        
        uiManager.SetData(GameTime);
        uiManager.GameTimerStatus(true);



    }

    private void InitVehiclesPools()
    {
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
