using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private GameObject driveVFX;

    public Region currentRegion { get; private set; }
    public Transform from { get; private set; }
    public Transform to { get; private set; }
    public Transform center { get; private set; }
    public bool IsCanMove { get; private set; }

    public RegionController regionController { get; private set; }
    public Vehicles vehicleType { get; private set; }
    public float TimeForRide { get; private set; } = 3f;

    private bool isStart;
    private float _timer;
    private Vector3 prevPosition;
    private bool isCenterReached;
    private GameManager gameManager;

    public bool HasRegion;

    private ObjectSpawner objectSpawner;
    private Transform sign;
    public bool IsWithObjectInside { get; private set; }

    public void SetData(RegionController r, Vehicles type, float timeForRide)
    {
        gameManager = GameManager.Instance;
        regionController = r;
        vehicleType = type;
        TimeForRide = timeForRide;
        driveVFX.SetActive(true);
        IsCanMove = true;

        objectSpawner = regionController.GetObjectSpawner(type);

        switch(vehicleType)
        {
            case Vehicles.taxi:
                sign = gameManager.GetAssets().TaxiSignPool.GetObject().transform;
                sign.gameObject.SetActive(false);
                break;

            case Vehicles.van:
                sign = gameManager.GetAssets().VanSignPool.GetObject().transform;
                sign.gameObject.SetActive(false);
                break;

            case Vehicles.ambulance:
                sign = gameManager.GetAssets().AmbulanceSignPool.GetObject().transform;
                sign.gameObject.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsGameStarted) return;

        if (isStart && IsCanMove)
        {
            _timer += Time.deltaTime;

            if (!isCenterReached)
            {                
                transform.position = Vector3.Lerp(from.position, center.position, _timer / (TimeForRide/2f));
                transform.LookAt(center);
                if (_timer / (TimeForRide / 2f) >= 1f )
                {
                    isCenterReached = true;
                    _timer = 0;
                }
            }
            else
            {
                transform.position = Vector3.Lerp(center.position, to.position, _timer / (TimeForRide / 2f));
                transform.LookAt(to);
            }


            float dist = (transform.position - prevPosition).magnitude;

            if (dist <= 0) MakeSelfDestruction();
            prevPosition = transform.position;
        }

        if (objectSpawner != null && !IsWithObjectInside && (transform.position - objectSpawner.GetPosition()).magnitude <= Globals.DISTANCE_FOR_OBJECTS)
        {
            IsWithObjectInside = true;
            sign.gameObject.SetActive(true);

            transform.DOPunchScale(Vector3.one * 0.4f, 0.3f).SetEase(Ease.InOutFlash);
            GameManager.Instance.GetSoundUI().PlayUISound(SoundsUI.swallow);
            objectSpawner.SpawnNewObject();
        }

        if (sign.gameObject.activeSelf)
        {
            sign.position = transform.position + Vector3.up * 2.5f;
        }
               
        if (isStart && _timer > 0.1f && (transform.position - to.position).magnitude <= 0.1f)
        {
            isStart = false;
            regionController.GetNewRoot(this, to);            
        }


    }

    public void SetMove(bool isMove)
    {
        IsCanMove = isMove;
        driveVFX.SetActive(isMove);
    }


    public void SetNewRoot(Region currentRegion, Transform from, Transform to, Transform center)
    {
        if (currentRegion != null && !currentRegion.IsActive) return;

        if (this.currentRegion != null)
        {
            this.currentRegion.RemoveVehicle(this);
        }

        this.currentRegion = currentRegion;
        this.currentRegion.AddVehicle(this);

        this.from = from;
        this.to = to;
        this.center = center;

        transform.position = from.position;
        transform.LookAt(center);
        _timer = 0;

        isStart = true;
        isCenterReached = false;

        SetMove(true);       
    }

    public void MakeSelfDestructionWithVFX()
    {
        if (currentRegion.endBarrier.Length > 0)
        {
            for (int i = 0; i < currentRegion.endBarrier.Length; i++)
            {
                if ((transform.position - currentRegion.endBarrier[i].transform.position).magnitude < 1)
                {                    
                    currentRegion.PlayDestroWithVFX(i);
                    break;
                }
            }
        }
        MakeSelfDestruction();
    }
    


    public void MakeSelfDestruction()
    {
        if (this.currentRegion != null)
        {
            this.currentRegion.RemoveVehicle(this);
            this.currentRegion = null;
        }

        IsWithObjectInside = false;
        AssetManager assetManager = GameManager.Instance.GetAssets();
        //Destroy(gameObject);
        switch (vehicleType)
        {
            case Vehicles.taxi:
                assetManager.TaxiPool.ReturnObject(this.gameObject);
                assetManager.TaxiSignPool.ReturnObject(sign.gameObject);
                break;

            case Vehicles.van:
                assetManager.VanPool.ReturnObject(this.gameObject);
                assetManager.VanSignPool.ReturnObject(sign.gameObject);
                break;

            case Vehicles.ambulance:
                assetManager.AmbulancePool.ReturnObject(this.gameObject);
                assetManager.AmbulanceSignPool.ReturnObject(sign.gameObject);
                break;
        }

    }
}


