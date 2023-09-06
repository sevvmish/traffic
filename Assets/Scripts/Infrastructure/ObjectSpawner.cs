using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour, CityInfrastructure
{
    public Vehicles MainType;
    [SerializeField] private Transform[] places;
    [SerializeField] private GameObject[] appearance;

    private Transform currentPlace;
    private Transform previousPlace;

    private GameObject [] objects;

    private GameObject currentAppearance;
    private GameObject sign;

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
                //sign.transform.parent = objects[i].transform;
                //sign.transform.position = transform.position + Vector3.up * 2.5f;
                sign.gameObject.SetActive(true);
                break;

            case Vehicles.van:
                sign = Instantiate(GameManager.Instance.GetAssets().VanSign);
                //sign.transform.parent = objects[i].transform;
                //sign.transform.position = transform.position + Vector3.up * 2.5f;
                sign.gameObject.SetActive(true);
                break;

            case Vehicles.ambulance:
                sign = Instantiate(GameManager.Instance.GetAssets().AmbulanceSign);
                //sign.transform.parent = objects[i].transform;
                //sign.transform.position = transform.position + Vector3.up * 2.5f;
                sign.gameObject.SetActive(true);
                break;
        }

        SpawnNewObject();
    }

    private void Update()
    {
        sign.transform.position = currentPlace.position + Vector3.up * 2.5f;
    }

    public Vector3 GetPosition() => currentPlace.position;

    public void SpawnNewObject()
    {
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
}
