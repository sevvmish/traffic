using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleReceiver : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    [SerializeField] private Transform whatToShake;
    public Vehicles VehicleType;
    
    private Region currentRegion;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        currentRegion = GetComponent<Region>();
        currentRegion.OnVehicleAdded = getVehicle;
        gameManager = GameManager.Instance;
    }

    private void getVehicle(Vehicle vehicle)
    {
        if (VehicleType == vehicle.vehicleType)
        {
            switch(VehicleType)
            {
                case Vehicles.taxi:                    
                    gameManager.AddTaxi();
                    break;

                case Vehicles.van:
                    gameManager.AddVan();
                    break;

                case Vehicles.ambulance:
                    gameManager.AddAmbulance();
                    break;
            }

            GameManager.Instance.GetSoundUI().PlayUISound(SoundsUI.positive);
            if (effect != null)
            {
                effect.GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            switch (VehicleType)
            {
                case Vehicles.taxi:
                    gameManager.RemoveTaxi();
                    break;

                case Vehicles.van:
                    gameManager.RemoveVan();
                    break;

                case Vehicles.ambulance:
                    gameManager.RemoveAmbulance();
                    break;
            }

            GameManager.Instance.GetSoundUI().PlayUISound(SoundsUI.error_big);
        }

        whatToShake.DOPunchScale(Vector3.one * 0.3f, 0.3f).SetEase(Ease.InOutBounce);
        
            
        vehicle.MakeSelfDestruction();
    }
}
