using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleReceiver : MonoBehaviour
{
    public Vehicles VehicleType;
    private Region currentRegion;


    // Start is called before the first frame update
    void Start()
    {
        currentRegion = GetComponent<Region>();
        currentRegion.OnVehicleAdded = getVehicle;
    }

    private void getVehicle(Vehicle vehicle)
    {
        if (VehicleType == vehicle.vehicleType)
        {
            print("Match!!!!!!!!!!!!!!!!");
        }

        vehicle.MakeSelfDestruction();
    }
}
