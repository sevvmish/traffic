using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
        
    [Header("transport use")]
    public bool IsTaxi;
    public bool IsVan;
    public bool IsAmbulance;

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

        Screen.SetResolution(1400, 700, true);
        

        regionController = GetComponent<RegionController>();
        inputController = GetComponent<InputController>();
        uiManager = GetComponent<UIManager>();

        regionController.SetData(mainCameraBody);
        InitVehiclesPools();

        mainCameraBody.position = new Vector3(
            (regionController.xBorder.x + regionController.xBorder.y) / 2,
            mainCameraBody.position.y,
            (regionController.zBorder.x + regionController.zBorder.y) / 2 - 3);

        inputController.SetData(mainCamera, mainCameraBody, joystick, mainCar, regionController.Location(), regionController);
        ambient.SetData(ambientType);
        uiManager.SetData();

        
    }

    private void InitVehiclesPools()
    {
        if (IsTaxi)
        {
            assets.TaxiPool = new ObjectPool(10, assets.GetVehicle(Vehicles.taxi), regionController.Location());
        }
        else
        {
            assets.TaxiPool = new ObjectPool(1, assets.GetVehicle(Vehicles.taxi), regionController.Location());
        }

        if (IsVan)
        {
            assets.VanPool = new ObjectPool(10, assets.GetVehicle(Vehicles.van), regionController.Location());
        }
        else
        {
            assets.VanPool = new ObjectPool(1, assets.GetVehicle(Vehicles.van), regionController.Location());
        }

        if (IsAmbulance)
        {
            assets.AmbulancePool = new ObjectPool(10, assets.GetVehicle(Vehicles.ambulance), regionController.Location());
        }
        else
        {
            assets.AmbulancePool = new ObjectPool(1, assets.GetVehicle(Vehicles.ambulance), regionController.Location());
        }

    }

}
