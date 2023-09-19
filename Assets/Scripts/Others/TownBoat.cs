using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TownBoat : MonoBehaviour
{
    public float mainTimerContinue = 10f;
    public float cooldown = 20f;
    public float startCooldown = 10f;
    public float stopperRadius = 2.5f;

    [SerializeField] private Transform leftBridge;
    [SerializeField] private Transform rightBridge;
    [SerializeField] private Transform boat;
    [SerializeField] private GameObject[] lights;
    [SerializeField] private VehicleStopper stopper;
    [SerializeField] private TrafficLightsController trafficLights;

    [SerializeField] private AudioSource boatSound;

    private float _startTimer, _mainTimer;
    private bool isBoatActive;

    // Start is called before the first frame update
    void Start()
    {
        stopper.Radius = stopperRadius;
        startCooldown = UnityEngine.Random.Range(0, startCooldown);
        leftBridge.localEulerAngles = Vector3.zero;
        rightBridge.localEulerAngles = Vector3.zero;
        boat.gameObject.SetActive(false);
        setLights(false);

        if (trafficLights != null) trafficLights.UpdateLighter(0.2f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_startTimer > startCooldown)
        {
            if (_mainTimer > cooldown && !isBoatActive)
            {
                isBoatActive = true;
                StartCoroutine(play());

                if (trafficLights != null) trafficLights.UpdateLighter(1f, 1f);
            }
            else
            {
                _mainTimer += Time.deltaTime;
                if (trafficLights != null) trafficLights.UpdateLighter(_mainTimer, cooldown);
            }
        }
        else
        {
            _startTimer += Time.deltaTime;
        }
    }

    private IEnumerator play()
    {
        setLights(true);
        boat.gameObject.SetActive(true);
        
        boat.DOLocalMove(new Vector3(-3.4f, 0, 0), 0);
        boat.DOLocalMove(new Vector3(3.4f, 0, 0), mainTimerContinue).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1);

        stopper.IsActive = true;
        leftBridge.DOLocalRotate(new Vector3(-60, 0, 0), 1f).SetEase(Ease.Linear);
        rightBridge.DOLocalRotate(new Vector3(60, 0, 0), 1f).SetEase(Ease.Linear);

        boatSound.Play();

        yield return new WaitForSeconds(mainTimerContinue - 1f - 1f);

        leftBridge.DOLocalRotate(Vector3.zero, 1f).SetEase(Ease.Linear);
        rightBridge.DOLocalRotate(Vector3.zero, 1f).SetEase(Ease.Linear);

        boatSound.Play();

        yield return new WaitForSeconds(1f);
        stopper.IsActive = false;
        setLights(false);
        _mainTimer = 0;
        isBoatActive = false;
        boat.gameObject.SetActive(false);
    }

    private void setLights(bool isActive)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(isActive);
        }
    }
}
