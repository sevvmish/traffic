using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Region : MonoBehaviour, CityInfrastructure
{
    public bool IsFakeRegion;
    public float RotationAngle = 90;

    public Transform[] entrances = new Transform[0];
    public Transform[] entrancesEnd = new Transform[0];
    public Transform[] centers = new Transform[0];
    public GameObject[] endBarrier = new GameObject[0];
    public Transform[] ObjectPlaces = new Transform[0];

    public GameObject[] unconnectedRoads = new GameObject[0];

    public bool IsActive { get; private set; }
    public bool IsBusyRotate { get => isBusyRotate; }
    public Action<Vehicle> OnVehicleAdded;

    [SerializeField] private GameObject edgeEffect;
    [SerializeField] private GameObject accidentEffect;
    [SerializeField] private GameObject lockEffect;
    [SerializeField] private GameObject border;
    [SerializeField] private Material whenNotBlocked;
    [SerializeField] private Material whenBlocked;

    private HashSet<Vehicle> currentVehicles = new HashSet<Vehicle>();

    private Transform _transform;
    private RegionController regionController;
    private bool isBusyRotate;
    private AudioSource _audioSource;
    private AssetManager assetManager;

    
        
    private readonly float stopRotationForTooCloseToEndDistance = 1f;

    private void Start()
    {
        assetManager = GameManager.Instance.GetAssets();
        _audioSource = GetComponent<AudioSource>();
        if (edgeEffect != null) edgeEffect.SetActive(true);
        _transform = transform;
        if (accidentEffect != null) accidentEffect.SetActive(false);
        if (lockEffect != null) lockEffect.SetActive(false);
        if (border != null) border.SetActive(Globals.IsShowBorder);
        setBorderLocked(false);

        if (RotationAngle == 0)
        {
            SetToNonRotation();
        }

        if (string.IsNullOrEmpty(gameObject.name))
        {
            gameObject.name = UnityEngine.Random.Range(1, 1000).ToString();
        }
    }

    public void SetToNonRotation()
    {
        setBorderLocked(true);
        RotationAngle = 0;
        border.transform.GetChild(0).GetComponent<MeshRenderer>().material = whenBlocked;
    }


    private void Update()
    {
        if (currentVehicles.Count>0 && IsActive)
        {
            Vehicle[] vehicles = currentVehicles.ToArray();

            for (int i = 0; i < vehicles.Length; i++)
            {
                for (int j = 0; j < vehicles.Length; j++)
                {
                    if (
                        vehicles[i] != vehicles[j]
                        && vehicles[i].vehicleType != vehicles[j].vehicleType
                        && (vehicles[i].transform.position - vehicles[j].transform.position).magnitude < Globals.DISTANCE_FOR_ACCIDENT
                        )
                    {
                        SetInActive_Accident(Globals.TIME_FOR_ACCIDENT, Vector3.Lerp(vehicles[i].transform.position, vehicles[j].transform.position, 0.5f) + Vector3.up * 0.5f, vehicles[i], vehicles[j]);
                    }
                }
            }

            /*
            foreach (var vehicle in currentVehicles)
            {
                foreach (var vehicle2 in currentVehicles)
                {
                    if (
                        vehicle != vehicle2 
                        && vehicle.vehicleType != vehicle2.vehicleType 
                        && (vehicle.transform.position - vehicle2.transform.position).magnitude < Globals.DISTANCE_FOR_ACCIDENT
                        )
                    {
                        SetInActive_Accident(Globals.TIME_FOR_ACCIDENT, Vector3.Lerp(vehicle.transform.position, vehicle2.transform.position, 0.5f) + Vector3.up * 0.5f, vehicle, vehicle2);
                    }
                }
            }*/
        }
    }

    public void SetData(RegionController r)
    {        
        regionController = r;

        for (int i = 0; i < endBarrier.Length; i++)
        {
            endBarrier[i].SetActive(false);
        }

        IsActive = true;
        
    }

    public void SetInActive_Accident(float _time, Vector3 pos, Vehicle one, Vehicle two)
    {
        if (IsActive)
        {
            StartCoroutine(playInactive(_time, pos, one, two));
            GameManager.Instance.AddAccident();
        }
            
    }
    private IEnumerator playInactive(float _time, Vector3 pos, Vehicle one, Vehicle two)
    {
        IsActive = false;
        setBorderLocked(true);

        regionController.UpdateAll();
        accidentEffect.transform.position = pos;
        accidentEffect.transform.localScale = Vector3.one;
        one.SetMove(false);
        two.SetMove(false);
        one.transform.DORotate( 
            new Vector3(one.transform.localEulerAngles.x, one.transform.localEulerAngles.y + UnityEngine.Random.Range(5f, 35f), one.transform.localEulerAngles.z), 0.2f).SetEase(Ease.InOutSine);
        two.transform.DORotate(
            new Vector3(one.transform.localEulerAngles.x, one.transform.localEulerAngles.y - UnityEngine.Random.Range(5f, 35f), one.transform.localEulerAngles.z), 0.2f).SetEase(Ease.InOutSine);

        accidentEffect.SetActive(true);

        if (currentVehicles.Count > 0)
        {
            Vehicle[] vehicles = currentVehicles.ToArray();

            for (int i = 0; i < vehicles.Length; i++)
            {
                if (vehicles[i] != one && vehicles[i] != two)
                {
                    vehicles[i].MakeSelfDestruction();
                }
            }

            /*
            foreach (Vehicle v in currentVehicles)
            {
                if (v != one && v != two)
                {
                    v.MakeSelfDestruction();
                }                
            }*/
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(playLockEffect());
        edgeEffect?.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(_time-0.4f);
        one.transform.DOScale(Vector3.zero, 0.3f);
        two.transform.DOScale(Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);

        one.transform.localScale = Vector3.one;
        two.transform.localScale = Vector3.one;

        one.MakeSelfDestruction();
        two.MakeSelfDestruction();        
        IsActive = true;
        setBorderLocked(false);

        regionController.UpdateAll();

        accidentEffect.transform.DOScale(Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);
        accidentEffect.SetActive(false);
    }
    private IEnumerator playLockEffect()
    {
        lockEffect.SetActive(true);
        lockEffect.transform.LookAt(Vector3.forward * 1000);
        lockEffect.transform.localPosition = Vector3.zero;
        lockEffect.transform.localScale = Vector3.zero;
        lockEffect.transform.DOLocalMove(new Vector3(0,4,0), 0.3f).SetEase(Ease.InOutBounce);
        lockEffect.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutBounce); 
        yield return new WaitForSeconds(0.3f);

        lockEffect.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f).SetEase(Ease.InOutBounce);
        yield return new WaitForSeconds(0.2f);

        lockEffect.transform.DOLocalRotate(new Vector3(lockEffect.transform.localEulerAngles.x, lockEffect.transform.localEulerAngles.y, -45), 0.25f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.25f);

        lockEffect.transform.DOLocalRotate(new Vector3(lockEffect.transform.localEulerAngles.x, lockEffect.transform.localEulerAngles.y, 40), 0.5f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.5f);

        lockEffect.transform.DOLocalRotate(new Vector3(lockEffect.transform.localEulerAngles.x, lockEffect.transform.localEulerAngles.y, -30), 0.4f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.4f);

        lockEffect.transform.DOLocalRotate(new Vector3(lockEffect.transform.localEulerAngles.x, lockEffect.transform.localEulerAngles.y, 20), 0.3f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.3f);

        lockEffect.transform.DOLocalRotate(new Vector3(lockEffect.transform.localEulerAngles.x, lockEffect.transform.localEulerAngles.y, -10), 0.2f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.2f);

        lockEffect.transform.DOLocalRotate(new Vector3(lockEffect.transform.localEulerAngles.x, lockEffect.transform.localEulerAngles.y, 0), 0.2f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.5f);

        lockEffect.transform.localScale = Vector3.one;
        lockEffect.SetActive(false);
    }

    private void setBorderLocked(bool isLocked)
    {
        if (border == null) return;

        if (isLocked)
        {
            if (border.transform.childCount > 0)
            {
                for (int i = 0; i < border.transform.childCount; i++)
                {
                    border.transform.GetChild(i).GetComponent<MeshRenderer>().material = whenBlocked;
                }
            }
        }
        else
        {
            if (border.transform.childCount > 0)
            {
                for (int i = 0; i < border.transform.childCount; i++)
                {
                    border.transform.GetChild(i).GetComponent<MeshRenderer>().material = whenNotBlocked;
                }
            }
        }
    }

    public void UpdateEnds()
    {
        if (endBarrier.Length == 0)
        {
            return;
        }

        for (int i = 0; i < entrancesEnd.Length; i++)
        {
            if (regionController.IsAnyInfrastructureInRadius(2, entrancesEnd[i]) 
                || !regionController.IsDeadEndForRoute(this, entrancesEnd[i]))
            {
                endBarrier[i].SetActive(false);
            }
            else
            {
                endBarrier[i].SetActive(true);
            }
                     
        }

        if (unconnectedRoads.Length > 0)
        {
            if (endBarrier[0].activeSelf && endBarrier[1].activeSelf)
            {
                unconnectedRoads[0].SetActive(true);
            }
            else
            {
                unconnectedRoads[0].SetActive(false);
            }
        }

        if (unconnectedRoads.Length > 1)
        {
            if (endBarrier[2].activeSelf && endBarrier[3].activeSelf)
            {
                unconnectedRoads[1].SetActive(true);
            }
            else
            {
                unconnectedRoads[1].SetActive(false);
            }
        }

    }

    public bool IsHasVehicles() => currentVehicles.Count > 0;
    public Vehicle[] GetVehicles() => currentVehicles.ToArray();

    public void AddVehicle(Vehicle vehicle)
    {
        currentVehicles.Add(vehicle);
        OnVehicleAdded?.Invoke(vehicle);
    }

    public void RemoveVehicle(Vehicle vehicle)
    {
        currentVehicles.Remove(vehicle);
    }

    public void RotateRegion(int sign)
    {
        if (isBusyRotate || !IsActive || RotationAngle == 0)
        {
            playError();
            return;
        }

        if (IsHasVehicles())
        {
            foreach (Vehicle item in currentVehicles)
            {
                if ((item.transform.position - item.to.position).magnitude < stopRotationForTooCloseToEndDistance)
                {
                    playError();
                    return;
                }                    
            }
        }

        //new Conditions(22.97f,-1,0),
        string s = $"new Conditions({GameManager.Instance.GetUI().GetTimeLeft().ToString("f2").Replace(',','.')}f, {sign}, {gameObject.name.Substring(gameObject.name.Length-1,1)}),";
        regionController.dataToEdu.Add(s);
        //print(GameManager.Instance.GetUI().GetTimeLeft() + ": " + (sign) + ": " + gameObject.name + " ====================");

        StartCoroutine(rotatePart(sign));
    }

    private void playError()
    {
        if (!Globals.IsSpectatorMode) GameManager.Instance.GetSoundUI().PlayUISound(SoundsUI.error);
    }

    public void PlayDestroWithVFX(int index)
    {
        StartCoroutine(playDestroWithVFX(index));
    }
    private IEnumerator playDestroWithVFX(int index)
    {
        if (!endBarrier[index].activeSelf)
        {
            StartCoroutine(playCarDestroEffect(endBarrier[index].transform.position));
            yield break;
        }

        endBarrier[index].transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(2);
        endBarrier[index].transform.GetChild(0).gameObject.SetActive(false);
    }

    private IEnumerator playCarDestroEffect(Vector3 pos)
    {
        GameObject g = assetManager.CarDestroEffectPool.GetObject();
        g.transform.position = pos;
        g.SetActive(true);

        yield return new WaitForSeconds(1);

        assetManager.CarDestroEffectPool.ReturnObject(g);
    }

    private IEnumerator rotatePart(int sign)
    {   
        Vector3 pos = _transform.position;
        Vector3 rot = _transform.eulerAngles;
        isBusyRotate = true;
        IsActive = false;

        float yy = rot.y;
        bool isOK = false;/*
        for (int j = 0; j < 6; j++)
        {
            yy += 60 * sign;
            _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, yy, _transform.eulerAngles.z);

            
            for (int i = 0; i < entrancesEnd.Length; i++)
            {
                if (regionController.IsAnyInfrastructureInRadius(2, entrancesEnd[i])
                    || !regionController.IsDeadEndForRoute(this, entrancesEnd[i]))
                {
                    isOK = true;
                    break;
                }
                
            }

            if (isOK) break;
        }*/

        if (!isOK) yy = rot.y + 60 * sign;

        _transform.eulerAngles = rot;

        for (int i = 0; i < entrancesEnd.Length; i++)
        {
            endBarrier[i].SetActive(false);
        }

        _audioSource.Play();
        //edgeEffect.SetActive(false);
        //edgeEffect.SetActive(true);
        edgeEffect?.GetComponent<ParticleSystem>().Play();

        _transform.DOPunchPosition(new Vector3(0, 1, 0), 0.05f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.025f);
        _transform.DOPunchPosition(new Vector3(0, 0.5f, 0), 0.05f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.025f);


        //_transform.DORotate(new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y + RotationAngle * sign, _transform.eulerAngles.z), Globals.SWIPE_SPEED);
        _transform.DORotate(new Vector3(_transform.eulerAngles.x, yy, _transform.eulerAngles.z), Globals.SWIPE_SPEED);
        yield return new WaitForSeconds(Globals.SWIPE_SPEED);
        _transform.position = pos;
        
        isBusyRotate = false;
        IsActive= true;
        //UpdateEnds();
        regionController.UpdateAll();
    }

    public CityInfrastructureTypes GetInfrastructureTypes()
    {
        return CityInfrastructureTypes.region;
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
