using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAmbientSounds : MonoBehaviour
{
    public Vehicles _type;
        
    private AudioSource audioSource;
    private AssetManager assets;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        assets = GameManager.Instance.GetAssets();

        switch(_type)
        {
            case Vehicles.taxi:
                StartCoroutine(playSimpleMotor());
                break;

            case Vehicles.van:
                StartCoroutine(playHeavyMotor());
                break;

            case Vehicles.ambulance:
                StartCoroutine(playAmbulance());
                break;
        }
    }

    private IEnumerator playSimpleMotor()
    {        
        while (true)
        {
            //yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f,1f));
            //audioSource.clip = assets.motorSoundLightClip;
            //audioSource.Play();
            //yield return new WaitForSeconds(2f);
            yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 6f));
            int chanse = UnityEngine.Random.Range(0, 100);
            if (chanse < 40)
            {
                audioSource.clip = assets.hornClip;
                audioSource.Play();
                yield return new WaitForSeconds(UnityEngine.Random.Range(0, 2f));
            }
        }
    }

    private IEnumerator playHeavyMotor()
    {
        while (true)
        {
            //yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1f));
            //audioSource.clip = assets.motorSoundHeavyClip;
            //audioSource.Play();
            //yield return new WaitForSeconds(2f);
            yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 6f));
            int chanse = UnityEngine.Random.Range(0, 100);
            if (chanse < 40)
            {
                audioSource.clip = assets.hornDoubleClip;
                audioSource.Play();
                yield return new WaitForSeconds(UnityEngine.Random.Range(0, 2f));
            }
        }
    }

    private IEnumerator playAmbulance()
    {
        int chanse = 0;

        while (true)
        {
            int h = 0;
            yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 4f));
            chanse = UnityEngine.Random.Range(0, 100);
            if (chanse < 10)
            {
                audioSource.clip = assets.ambuSirenClip;
                audioSource.Play();
                yield return new WaitForSeconds(2f);
            }
            else
            {
                h = 20;
            }
                        
            yield return new WaitForSeconds(UnityEngine.Random.Range(2.5f, 4f));
            chanse = UnityEngine.Random.Range(0, 100);
            if (chanse < (10 + h))
            {
                audioSource.clip = assets.specSignalClip;
                audioSource.Play();
                yield return new WaitForSeconds(1f);
            }

            
            //audioSource.clip = assets.motorSoundHeavyClip;
            //audioSource.Play();
            //yield return new WaitForSeconds(2f);
        }
    }
}
