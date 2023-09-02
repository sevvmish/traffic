using DG.Tweening;
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

    public bool HasRegion;

    public void SetData(RegionController r, Vehicles type, float timeForRide)
    {
        regionController = r;
        vehicleType = type;
        TimeForRide = timeForRide;
        driveVFX.SetActive(true);
        IsCanMove = true;
    }

    // Update is called once per frame
    void Update()
    {        
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

    /*
    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.TryGetComponent(out Vehicle v))
        {
            if (v.vehicleType != vehicleType)
            {                
                currentRegion.SetInActive_Accident
                    (Globals.TIME_FOR_ACCIDENT, Vector3.Lerp(transform.position, v.transform.position, 0.5f) + Vector3.up * 0.5f, this, v);
            }
        }
    }
    */

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

        AssetManager assetManager = GameManager.Instance.GetAssets();
        //Destroy(gameObject);
        switch (vehicleType)
        {
            case Vehicles.taxi:
                assetManager.TaxiPool.ReturnObject(this.gameObject);
                break;

            case Vehicles.van:
                assetManager.VanPool.ReturnObject(this.gameObject);
                break;

            case Vehicles.ambulance:
                assetManager.AmbulancePool.ReturnObject(this.gameObject);
                break;
        }

    }
}


