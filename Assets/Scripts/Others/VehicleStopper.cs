using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleStopper : MonoBehaviour
{
    public float Radius;
    public bool IsActive;
    [SerializeField] private Region currentRegion;
    [SerializeField] private GameObject destroEffect;

    private AssetManager assetManager;

    private void Start()
    {
        if (GameManager.Instance != null) assetManager = GameManager.Instance.GetAssets();
        currentRegion = transform.parent.GetComponent<Region>();
    }

    private void Update()
    {
        if (IsActive && currentRegion != null && currentRegion.IsHasVehicles())
        {
            Vehicle[] vehicles = currentRegion.GetVehicles();

            for (int i = 0; i < vehicles.Length; i++) 
            {
                if ((vehicles[i].transform.position - transform.position).magnitude < Radius)
                {
                    StartCoroutine(playCarDestroEffect(vehicles[i].transform.position));
                    vehicles[i].MakeSelfDestruction();
                }
            }
        }

        
    }

    private IEnumerator playCarDestroEffect(Vector3 pos)
    {
        GameObject g = assetManager.CarDestroEffectPool.GetObject();
        g.transform.position = pos;
        g.SetActive(true);

        yield return new WaitForSeconds(1);

        assetManager.CarDestroEffectPool.ReturnObject(g);
    }

    
}
