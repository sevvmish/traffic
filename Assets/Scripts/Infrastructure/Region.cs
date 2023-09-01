using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour, CityInfrastructure
{    
    public Transform[] entrances = new Transform[0];
    public Transform[] entrancesEnd = new Transform[0];
    public Transform[] centers = new Transform[0];
    public GameObject[] endBarrier = new GameObject[0];
    public bool IsBusy { get => isBusy; }
    public Action<Vehicle> OnVehicleAdded;

    [SerializeField] private GameObject edgeEffect;
        
    private HashSet<Vehicle> currentVehicles = new HashSet<Vehicle>();

    private Transform _transform;
    private RegionController regionController;
    private bool isBusy;
    private AudioSource _audioSource;
    
    private readonly float swipeSpeed = 0.25f;
    private readonly float stopRotationForTooCloseToEndDistance = 1f;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (edgeEffect != null) edgeEffect.SetActive(true);
        _transform = transform;
    }

    public void SetData(RegionController r)
    {        
        regionController = r;

        for (int i = 0; i < endBarrier.Length; i++)
        {
            endBarrier[i].SetActive(false);
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
    }

    public bool IsHasVehicles() => currentVehicles.Count > 0;

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
        if (isBusy)
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

        StartCoroutine(rotatePart(sign));
    }

    private void playError()
    {
        GameManager.Instance.GetSoundUI().PlayUISound(SoundsUI.error);
    }

    private IEnumerator rotatePart(int sign)
    {   
        Vector3 pos = _transform.position;
        isBusy = true;

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


        _transform.DORotate(new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y + 90 * sign, _transform.eulerAngles.z), swipeSpeed);
        yield return new WaitForSeconds(swipeSpeed);
        _transform.position = pos;
        
        isBusy = false;
        UpdateEnds();
    }

    public CityInfrastructureTypes GetInfrastructureTypes()
    {
        return CityInfrastructureTypes.region;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
