using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AssetManager : MonoBehaviour
{
    [Header("Vehicles")]
    public GameObject Taxi;
    public GameObject Van;
    public GameObject Ambulance;

    [Header("Vehicles pools")]
    public ObjectPool TaxiPool;
    public ObjectPool VanPool;
    public ObjectPool AmbulancePool;

    [Header("Sounds")]
    public AudioClip ErrorClip;
    public AudioClip SmallTownAmbient;
    public AudioClip hornClip;
    public AudioClip hornDoubleClip;
    public AudioClip ambuSirenClip;
    public AudioClip sirenClip;
    public AudioClip specSignalClip;
    public AudioClip motorSoundLightClip;
    public AudioClip motorSoundHeavyClip;
    public AudioClip positiveSoundClip;


    public GameObject GetVehicle(Vehicles vehicle)
    {
        switch(vehicle)
        {
            case Vehicles.taxi:
                return Taxi;

            case Vehicles.van:
                return Van;

            case Vehicles.ambulance:
                return Ambulance;

        }

        throw new System.NotImplementedException();
    }

}

public enum Vehicles
{
    none,
    taxi,
    van,
    ambulance
}
