using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleReceiver : MonoBehaviour, CityInfrastructure
{
    public RotateInfrastructure Rotator;

    [SerializeField] private Transform EntryPoint;

    [SerializeField] private GameObject effect;
    [SerializeField] private Transform whatToShake;
    
    public Vehicles VehicleType;
    
    //private Region currentRegion;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
        gameManager = GameManager.Instance;
    }

    public void GetVehicle(Vehicle vehicle)
    {        
        if (VehicleType == vehicle.vehicleType)
        {
            switch(VehicleType)
            {
                case Vehicles.taxi:                    
                    gameManager.AddTaxi();
                    if (vehicle.IsWithObjectInside) gameManager.AddTaxiMan();
                    break;

                case Vehicles.van:
                    gameManager.AddVan();
                    if (vehicle.IsWithObjectInside) gameManager.AddVanCargo();
                    break;

                case Vehicles.ambulance:
                    gameManager.AddAmbulance();
                    if (vehicle.IsWithObjectInside) gameManager.AddAmbulanceMan();
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
                    if (vehicle.IsWithObjectInside) gameManager.RemoveTaxiMan();
                    gameManager.AddMistake();
                    break;

                case Vehicles.van:
                    gameManager.RemoveVan();
                    if (vehicle.IsWithObjectInside) gameManager.RemoveVanCargo();
                    gameManager.AddMistake();
                    break;

                case Vehicles.ambulance:
                    gameManager.RemoveAmbulance();
                    if (vehicle.IsWithObjectInside) gameManager.RemoveAmbulanceMan();
                    gameManager.AddMistake();
                    break;
            }

            GameManager.Instance.GetSoundUI().PlayUISound(SoundsUI.error_big);
        }

        whatToShake.DOPunchScale(Vector3.one * 0.3f, 0.3f).SetEase(Ease.InOutBounce);

        StartCoroutine(playCarDestroEffect(vehicle.transform.position));    
        vehicle.MakeSelfDestruction();
    }

    private IEnumerator playCarDestroEffect(Vector3 pos)
    {
        GameObject g = gameManager.GetAssets().CarDestroEffectPool.GetObject();
        g.transform.position = pos;
        g.SetActive(true);

        yield return new WaitForSeconds(1);

        gameManager.GetAssets().CarDestroEffectPool.ReturnObject(g);
    }

    public CityInfrastructureTypes GetInfrastructureTypes()
    {
        return CityInfrastructureTypes.receiver;
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
