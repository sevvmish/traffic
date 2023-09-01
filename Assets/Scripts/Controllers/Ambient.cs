using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambient : MonoBehaviour
{    
    private AudioSource audioSource;

    public void SetData(AmbientType _type)
    {
        audioSource = GetComponent<AudioSource>();

        switch (_type)
        {
            case AmbientType.small_town:
                audioSource.clip = GameManager.Instance.GetAssets().SmallTownAmbient;
                audioSource.Play();
                break;
        }
    }
}

public enum AmbientType
{
    none,
    small_town
}
