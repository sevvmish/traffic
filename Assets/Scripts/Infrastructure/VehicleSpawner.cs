using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour, CityInfrastructure
{
    public Vehicles _vehicle;
    public float SpawnFrequency = 5f;
        
    private RegionController regionController;
    private AssetManager assetManager;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        assetManager = GameManager.Instance.GetAssets();
        regionController = GameManager.Instance.regionController;
        _timer = SpawnFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > SpawnFrequency)
        {
            _timer = 0;
            SpawnVehicle(_vehicle, this.transform.localPosition);
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public void SpawnVehicle(Vehicles vehicle, Vector3 pos)
    {
        if (regionController.IsDeadEndForRoute(null, transform)) { return; }

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
        regionController.GetNewRoot(g.GetComponent<Vehicle>(), null);

    }

    public CityInfrastructureTypes GetInfrastructureTypes()
    {
        return CityInfrastructureTypes.spawner;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}


