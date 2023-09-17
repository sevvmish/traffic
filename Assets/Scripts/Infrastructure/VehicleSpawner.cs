using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour, CityInfrastructure
{
    [SerializeField] private Transform EntryPoint;
    [SerializeField] private TrafficLightsController trafficLights;

    public Vehicles _vehicle;
    public float SpawnFrequency = 5f;
    public int Limit = 0;
    private int currentCount;
        
    private RegionController regionController;
    private AssetManager assetManager;
    private GameManager gameManager;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        assetManager = GameManager.Instance.GetAssets();
        regionController = GameManager.Instance.regionController;
        _timer = SpawnFrequency;
        gameManager = GameManager.Instance;
        if (Limit < 1) Limit = 1000000;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameStarted)
        {
            if (_timer > SpawnFrequency && currentCount < Limit)
            {
                if (SpawnVehicle(_vehicle, this.transform.localPosition))
                {
                    _timer = 0;
                }
                else
                {
                    _timer -= 0.1f;
                }


            }
            else if(_timer <= SpawnFrequency && currentCount < Limit)
            {

                _timer += Time.deltaTime;
            }
            else
            {
                _timer = 0;
            }

            if (trafficLights != null) trafficLights.UpdateLighter(_timer, SpawnFrequency);
        }
        
    }

    public bool SpawnVehicle(Vehicles vehicle, Vector3 pos)
    {
        //if (regionController.IsDeadEndForRoute(null, transform)) { return false; }
        Transform from = null;
        Transform to = null;
        Transform center = null;
        Region region = regionController.GetClosestRoot(EntryPoint.position, 2, out from, out to, out center);

        if (region == null) return false;

        currentCount++;
        GameObject transport = default;
        float timeForRide = 0;
        
        if (vehicle == Vehicles.none)
        {
            vehicle = (Vehicles)UnityEngine.Random.Range(1, 4);
        }

        switch(vehicle)
        {
            case Vehicles.taxi:
                transport = assetManager.TaxiPool.GetObject();
                timeForRide = Globals.TIME_FOR_RIDE_TAXI;
                break;

            case Vehicles.van:
                transport = assetManager.VanPool.GetObject();
                timeForRide = Globals.TIME_FOR_RIDE_VAN;
                break;

            case Vehicles.ambulance:
                transport = assetManager.AmbulancePool.GetObject();
                timeForRide = Globals.TIME_FOR_RIDE_AMBULANCE;
                break;
        }

        GameObject g = transport;//Instantiate(transport, regionController.Location());
        g.SetActive(true);
        g.transform.localPosition = pos;
        g.GetComponent<Vehicle>().SetData(regionController, vehicle, timeForRide);
        //regionController.GetNewRoot(g.GetComponent<Vehicle>(), null);
        g.GetComponent<Vehicle>().SetNewRoot(region, from, to, center);

        return true;
    }

    public CityInfrastructureTypes GetInfrastructureTypes()
    {
        return CityInfrastructureTypes.spawner;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Transform GetEntryPoint()
    {
        return EntryPoint;
    }
}


