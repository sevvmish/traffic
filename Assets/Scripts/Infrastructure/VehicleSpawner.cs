using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour, CityInfrastructure
{
    public Vehicles _vehicle;
    public float Frequency = 0;
        
    private RegionController regionController;
    private AssetManager assetManager;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        assetManager = GameManager.Instance.GetAssets();
        regionController = GameManager.Instance.regionController;
        _timer = Frequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > Frequency)
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
        GameObject transport = default;
        
        switch(vehicle)
        {
            case Vehicles.taxi:
                transport = assetManager.TaxiPool.GetObject();
                break;

            case Vehicles.van:
                transport = assetManager.VanPool.GetObject();
                break;

            case Vehicles.ambulance:
                transport = assetManager.AmbulancePool.GetObject();
                break;
        }

        GameObject g = transport;//Instantiate(transport, regionController.Location());
        g.SetActive(true);
        g.transform.localPosition = pos;
        g.GetComponent<Vehicle>().SetData(regionController, vehicle, 3f);
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


