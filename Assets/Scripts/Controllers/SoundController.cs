using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{    
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayUISound(SoundsUI _type)
    {
        switch(_type)
        {
            case SoundsUI.error:
                audioSource.Stop();
                audioSource.clip = GameManager.Instance.GetAssets().ErrorClip;
                audioSource.Play();
                break;

            case SoundsUI.positive:
                audioSource.Stop();
                audioSource.clip = GameManager.Instance.GetAssets().positiveSoundClip;
                audioSource.Play();
                break;

            case SoundsUI.error_big:
                audioSource.Stop();
                audioSource.clip = GameManager.Instance.GetAssets().ErrorBiggerClip;
                audioSource.Play();
                break;

            case SoundsUI.swallow:
                audioSource.Stop();
                audioSource.clip = GameManager.Instance.GetAssets().Swallow;
                audioSource.Play();
                break;

            case SoundsUI.tick:
                audioSource.Stop();
                audioSource.clip = GameManager.Instance.GetAssets().Tick;
                audioSource.Play();
                break;
        }
    }
}

public enum SoundsUI
{
    none,
    error,
    positive,
    error_big,
    swallow,
    tick
}
