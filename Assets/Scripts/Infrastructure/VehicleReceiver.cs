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
        whatToShake.DOPunchScale(Vector3.one * 0.3f, 0.3f).SetEase(Ease.InOutBounce);
        GameManager.Instance.GetSoundUI().PlayUISound(SoundsUI.positive);
        if (effect != null)
        {
            effect.GetComponent<ParticleSystem>().Play();            
        }
            
        vehicle.MakeSelfDestruction();
    }
}
