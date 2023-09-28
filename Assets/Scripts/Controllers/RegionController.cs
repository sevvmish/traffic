using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegionController : MonoBehaviour
{
    [SerializeField] private Transform location;
    [SerializeField] private List<CityInfrastructure> infrastructures = new List<CityInfrastructure>();

    public Transform Location() => location;
    public Vector2 xBorder { get; private set; }
    public Vector2 zBorder { get; private set; }
    public List<CityInfrastructure> GetInfrastructures() => infrastructures;
    public void RemoveInfrastructure(CityInfrastructure infr) => infrastructures.Remove(infr);
    public void AddInfrastructure(CityInfrastructure infr) => infrastructures.Add(infr);

    private readonly float minDistnaceForConnectionOK = 2f;

    private Transform[] ObjectPlaces;
    private HashSet<Transform> busyPlaces = new HashSet<Transform>();

    public ObjectSpawner GetObjectSpawner(Vehicles _type)
    {
        for (int i = 0; i < infrastructures.Count; i++)
        {
            if (infrastructures[i].GetInfrastructureTypes() == CityInfrastructureTypes.object_spawner 
                && infrastructures[i].GetGameObject().GetComponent<ObjectSpawner>().MainType == _type)
            {
                return infrastructures[i].GetGameObject().GetComponent<ObjectSpawner>();
            }
        }

        return null;
    }

    
    public Transform GetObjectPlace()
    {
        List<Transform> preResult = new List<Transform>();

        for (int i = 0; i < infrastructures.Count; i++)
        {
            if (infrastructures[i].GetInfrastructureTypes() == CityInfrastructureTypes.region && infrastructures[i].GetGameObject().TryGetComponent(out Region r))
            {
                if (r.ObjectPlaces.Length > 0)
                {
                    for (int j = 0; j < r.ObjectPlaces.Length; j++)
                    {
                        if (busyPlaces.Contains(r.ObjectPlaces[j])) continue;
                        preResult.Add(r.ObjectPlaces[j]);
                    }
                }
            }
        }

        Transform result = preResult.ToArray()[UnityEngine.Random.Range(0, preResult.Count)];
        busyPlaces.Add(result);

        return result;
    }

    public void ReturnObjectPlace(Transform objectPlace)
    {
        if (busyPlaces.Contains(objectPlace)) busyPlaces.Remove(objectPlace);
    }

    public void SetData(Transform cameraTransform)
    {
        
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
            if (infrastructures[i].GetGameObject().TryGetComponent(out Region r) && !r.IsFakeRegion)
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

    
    public void GetNewRoot(Vehicle vehicle, Transform currentEnd)
    {
        if (currentEnd != null)
        {
            for (int i = 0; i < infrastructures.Count; i++)
            {
                if (infrastructures[i].GetInfrastructureTypes() == CityInfrastructureTypes.receiver 
                    && (infrastructures[i].GetEntryPoint().position - currentEnd.position).magnitude < 2)
                {
                    infrastructures[i].GetGameObject().GetComponent<VehicleReceiver>().GetVehicle(vehicle);
                    return;
                }
            }
        }


        if (currentEnd != null && IsDeadEndForRoute(vehicle.currentRegion, currentEnd))
        {
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

    public Region GetClosestRoot(Vector3 position, float distance, out Transform from, out Transform to, out Transform center)
    {        
        Region region = null;
        from = null;
        to = null;
        center = null;
        float minDistance = 1000;

        for (int i = 0; i < infrastructures.Count; i++)
        {
            if (infrastructures[i].GetGameObject().TryGetComponent(out Region r))
            {

                if (!r.gameObject.activeSelf || !r.IsActive) continue;
                
                for (int j = 0; j < r.entrances.Length; j++)
                {
                    float dist = (position - r.entrances[j].position).magnitude;

                    if (dist < distance && dist < minDistance)
                    {
                        minDistance = dist;
                        region = r;
                        from = r.entrances[j];
                        to = r.entrancesEnd[j];
                        center = r.centers[j];
                    }
                }
            }
        }

        return region;
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
            if ((infrastructures[j].GetEntryPoint().position - end.position).magnitude <= radius)
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
    Transform GetEntryPoint();
}

public enum CityInfrastructureTypes
{
    region,
    spawner,
    receiver,
    object_spawner,
    vehicle_stopper
}

public interface IRotate
{
    bool IsBusyRotate { get; set; }
    void RotateRegion(int sign);
    void SetRotationAngle(float angle);
}
