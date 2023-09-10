using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour, CityInfrastructure
{
    public Vehicles MainType;    
    [SerializeField] private GameObject[] appearance;

    private Transform currentPlace;
    private Transform previousPlace;

    private GameObject [] objects;

    private GameObject currentAppearance;
    private GameObject sign;

    private bool isFirstStarted;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;

        objects = new GameObject[appearance.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = Instantiate(appearance[i]);
            objects[i].GetComponent<ObjectBehaviour>().MainType = MainType;
            objects[i].SetActive(false);
        }

        switch (MainType)
        {
            case Vehicles.taxi:
                sign = Instantiate(gameManager.GetAssets().TaxiSign);
                
                break;

            case Vehicles.van:
                sign = Instantiate(gameManager.GetAssets().VanSign);
                
                break;

            case Vehicles.ambulance:
                sign = Instantiate(gameManager.GetAssets().AmbulanceSign);
                
                break;
        }

        sign.SetActive(false);


    }

    private void Update()
    {
        if (currentPlace != null)
        {
            if (sign != null && !sign.activeSelf) sign.SetActive(true);
            sign.transform.position = currentPlace.position + Vector3.up * 2.5f;
        }
            

        if (!isFirstStarted && gameManager.IsGameStarted)
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
        if (previousPlace == null && currentPlace == null)
        {
            currentPlace = gameManager.regionController.GetObjectPlace();
        }
        else if (currentPlace != null)
        {
            previousPlace = currentPlace;
            gameManager.regionController.ReturnObjectPlace(currentPlace);

            for (int i = 0; i < 100; i++)
            {
                currentPlace = gameManager.regionController.GetObjectPlace();
                if (currentPlace == previousPlace)
                {
                    gameManager.regionController.ReturnObjectPlace(currentPlace);
                }
                else
                {
                    break;
                }
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
