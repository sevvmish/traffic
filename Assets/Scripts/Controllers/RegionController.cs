using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionController : MonoBehaviour
{
    [SerializeField] private Transform location;
    [SerializeField] private GameObject reg;
    [SerializeField] private List<CityInfrastructure> infrastructures = new List<CityInfrastructure>();

    public Transform Location() => location;
    public Vector2 xBorder { get; private set; }
    public Vector2 zBorder { get; private set; }
    public List<CityInfrastructure> GetInfrastructures() => infrastructures;

    private readonly float minDistnaceForConnectionOK = 2f;

    public void SetData(Transform cameraTransform)
    {
        //addRegion(Vector3.zero, Vector3.zero);
        //addRegion(new Vector3(10, 0, 0), new Vector3(0,90,0));
        //addRegion(new Vector3(-10, 0, 0), new Vector3(0, 180, 0));
        //addRegion(new Vector3(0, 0, 10), new Vector3(0, 0, 0));
        //addRegion(new Vector3(0, 0, -10), new Vector3(0, 270, 0));

        for (int i = 0; i < location.childCount; i++)
        {
            if (location.GetChild(i).gameObject.TryGetComponent(out CityInfrastructure r) && location.GetChild(i).gameObject.activeSelf)
            {
                if (r.GetInfrastructureTypes() == CityInfrastructureTypes.region) r.GetGameObject().GetComponent<Region>().SetData(this);
                infrastructures.Add(r);
            }            
        }

        float minX = 1000;
        float minZ = 1000;
        float maxX = -1000;
        float maxZ = -1000;

        for (int i = 0; i < infrastructures.Count; i++)
        {
            if (infrastructures[i].GetGameObject().TryGetComponent(out Region r))
            {
                r.UpdateEnds();

                if (r.transform.position.x < minX)
                {
                    minX = r.transform.position.x;
                }
                if (r.transform.position.x > maxX)
                {
                    maxX = r.transform.position.x;
                }

                if (r.transform.position.z < minZ)
                {
                    minZ = r.transform.position.z;
                }
                if (r.transform.position.z > maxZ)
                {
                    maxZ = r.transform.position.z;
                }
            }
            
        }

        xBorder = new Vector2(minX + cameraTransform.position.x + 8, maxX + cameraTransform.position.x - 8);
        zBorder = new Vector2(minZ + cameraTransform.position.z, maxZ + cameraTransform.position.z);
        print(xBorder + " = " + zBorder);
    }

    public void UpdateAll()
    {
        for (int i = 0; i < infrastructures.Count; i++)
        {
            if (infrastructures[i].GetGameObject().TryGetComponent(out Region r))
            {
                r.UpdateEnds();
            }

        }
    }

    private void addRegion(Vector3 pos, Vector3 rot)
    {
        GameObject r = Instantiate(reg, location);
        r.transform.localPosition = pos;
        r.transform.localEulerAngles = rot;
        r.transform.localScale = Vector3.one * 0.95f;
        r.SetActive(true);
        Region region = r.GetComponent<Region>();
        region.SetData(this);
        infrastructures.Add(region);
    }

    public void GetNewRoot(Vehicle vehicle, Transform currentEnd)
    {
        if (currentEnd != null && IsDeadEndForRoute(vehicle.currentRegion, currentEnd))
        {
            //if (vehicle.currentRegion != null) vehicle.currentRegion.RemoveVehicle(vehicle);
            //Destroy(vehicle.gameObject);
            //vehicle.MakeSelfDestruction();
            vehicle.MakeSelfDestructionWithVFX();
            return;
        }

        float distance = 1000;
        Transform from = default;
        Transform to = default;
        Transform center = default;
        Region region = default;

        for (int i = 0; i < infrastructures.Count; i++)
        {
            if (infrastructures[i].GetGameObject().TryGetComponent(out Region r)) {

                if (!r.gameObject.activeSelf || !r.IsActive) continue;
                if (vehicle.currentRegion != null && vehicle.currentRegion == r) continue;

                for (int j = 0; j < r.entrances.Length; j++)
                {
                    float dist = (vehicle.transform.position - r.entrances[j].position).magnitude;
                    if (dist < distance)
                    {
                        distance = dist;
                        from = r.entrances[j];
                        to = r.entrancesEnd[j];
                        center = r.centers[j];
                        region = r;
                        
                    }
                }
            }
                      
        }
        
        if ((vehicle.transform.position - from.position).magnitude < minDistnaceForConnectionOK)
        {
            vehicle.SetNewRoot(region, from, to, center);
        }
        
    }

    public bool IsDeadEndForRoute(Region region, Transform end)
    {
        for (int j = 0; j < infrastructures.Count; j++)
        {
            if (infrastructures[j].GetGameObject().TryGetComponent(out Region r) && r.IsActive)
            {
                if (region != null && r == region) continue;

                for (int i = 0; i < r.entrances.Length; i++)
                {
                    if ((r.entrances[i].position - end.position).magnitude < minDistnaceForConnectionOK)
                    {
                        return false;
                    }
                }
            }
                
        }

        return true;
    }

    public bool IsAnyInfrastructureInRadius(float radius, Transform end)
    {
        for (int j = 0; j < infrastructures.Count; j++)
        {
            if ((infrastructures[j].GetGameObject().transform.position - end.position).magnitude < radius)
            {
                return true;
            }

        }

        return false;
    }

}

public interface CityInfrastructure
{
    CityInfrastructureTypes GetInfrastructureTypes();
    GameObject GetGameObject();
}

public enum CityInfrastructureTypes
{
    region,
    spawner,
    receiver
}
