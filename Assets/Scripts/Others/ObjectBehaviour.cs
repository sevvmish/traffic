using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    public Vehicles MainType;

    private float _timer;

    private void Update()
    {
        if (MainType == Vehicles.taxi || MainType == Vehicles.ambulance)
        {
            if (_timer > 2)
            {
                _timer = 0;
                transform.DOLocalRotate(new Vector3(0, UnityEngine.Random.Range(0, 360), 0), 0.3f).SetEase(Ease.Linear);
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }
        
    }
}
