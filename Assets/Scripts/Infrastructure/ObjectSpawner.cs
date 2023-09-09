using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour, CityInfrastructure
{
    public Vehicles MainType;
    private Transform[] places = new Transform[0];
    [SerializeField] private GameObject[] appearance;

    private Transform currentPlace;
    private Transform previousPlace;

    private GameObject [] objects;

    private GameObject currentAppearance;
    private GameObject sign;

    private bool isFirstStarted;

    private void Start()
    {
       
        objects = new GameObject[appearance.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = Instantiate(appearance[i]);
            objects[i].GetComponent<ObjectBehaviour>().MainType = Vehicles.ambulance;
            objects[i].SetActive(false);
        }

        switch (MainType)
        {
            case Vehicles.taxi:
                sign = Instantiate(GameManager.Instance.GetAssets().TaxiSign);
                
                break;

            case Vehicles.van:
                sign = Instantiate(GameManager.Instance.GetAssets().VanSign);
                
                break;

            case Vehicles.ambulance:
                sign = Instantiate(GameManager.Instance.GetAssets().AmbulanceSign);
                
                break;
        }

        
    }

    private void Update()
    {
        if (currentPlace != null)
        {
            if (sign != null && !sign.activeSelf) sign.SetActive(true);
            sign.transform.position = currentPlace.position + Vector3.up * 2.5f;
        }
            

        if (!isFirstStarted && GameManager.Instance.IsGameStarted)
        {
            isFirstStarted = true;
            SpawnNewObject();
        }
                
    }

    public Vector3 GetPosition()
    {
        if (currentPlace != null)
        {
            return currentPlace.position;
        }
        else
        {
            return new Vector3 (1000, 1000, 1000);
        }
        
    }

    public void SpawnNewObject()
    {
        if (places.Length == 0)
        {
            places = GameManager.Instance.regionController.GetObjectPlaces();
            if (places.Length == 0) return;
        }

        if (previousPlace == null && currentPlace == null)
        {
            currentPlace = places[UnityEngine.Random.Range(0, places.Length)];
        }
        else if (currentPlace != null)
        {
            previousPlace = currentPlace;

            for (int i = 0; i < 100; i++)
            {
                currentPlace = places[UnityEngine.Random.Range(0, places.Length)];
                if (currentPlace != previousPlace) break;
            }

            currentAppearance.SetActive(false);
        }

        currentAppearance = objects[UnityEngine.Random.Range(0, objects.Length)];
        currentAppearance.transform.parent = currentPlace.parent;
        currentAppearance.SetActive(true);
        currentAppearance.transform.position = currentPlace.transform.position;
    }

    public CityInfrastructureTypes GetInfrastructureTypes()
    {
        return CityInfrastructureTypes.object_spawner;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Transform GetEntryPoint()
    {
        return gameObject.transform;
    }
}
