using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("ui timer")]
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private RectTransform timerTextRect;
    private float allTime;
    private float currentTime;
    private bool isPauseTimer = true;
    private float _timer;
    public float GetTimeLeft() => currentTime;

    [Header("ui data gameobjects")]
    [SerializeField] private GameObject taxiDataPanel;
    [SerializeField] private GameObject vanDataPanel;
    [SerializeField] private GameObject ambulanceDataPanel;

    [Header("ui data texts")]
    [SerializeField] private TextMeshProUGUI taxiDataText;
    [SerializeField] private TextMeshProUGUI vanDataText;
    [SerializeField] private TextMeshProUGUI ambulanceDataText;


    private GameManager gm;
    private int currentTaxi;
    private int currentVan;
    private int currentAmbu;

    private RectTransform taxiRect;
    private RectTransform vanRect;
    private RectTransform ambuRect;

    private Vector3 dataShakeAmount = new Vector3(0.4f, 0.6f, 0);
    private Vector2 bigSizeUIFrame = new Vector2(150, 50);
    private Vector2 smallSizeUIFrame = new Vector2(107, 50);

    
    public void SetData(float timeForGame)
    {
        timerTextRect = timerText.GetComponent<RectTransform>();
        timerPanel.SetActive(true);
        allTime = timeForGame;
        currentTime = timeForGame;
        ShowTimeOnTimer(currentTime);

        gm = GameManager.Instance;

        taxiRect = taxiDataText.GetComponent<RectTransform>();
        vanRect = vanDataText.GetComponent<RectTransform>();
        ambuRect = ambulanceDataText.GetComponent<RectTransform>();

        taxiDataPanel.SetActive(gm.TaxiCount > 0);
        vanDataPanel.SetActive(gm.VanCount > 0);
        ambulanceDataPanel.SetActive(gm.AmbulanceCount > 0);

        if (gm.TaxiCount > 9 || gm.VanCount > 9 || gm.AmbulanceCount > 9)
        {
            taxiDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = bigSizeUIFrame;
            vanDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = bigSizeUIFrame;
            ambulanceDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = bigSizeUIFrame;
        }
        else if(gm.TaxiCount < 10 && gm.VanCount < 10 && gm.AmbulanceCount < 10)
        {
            taxiDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = smallSizeUIFrame;
            vanDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = smallSizeUIFrame;
            ambulanceDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = smallSizeUIFrame;
        }

        winConditionUpdate();
        gm.OnChangedConditionsAmount = winConditionUpdate;

        currentTaxi = gm.TaxiCurrent;
        currentVan = gm.VanCurrent;
        currentAmbu = gm.AmbulanceCurrent;
               
    }

    private void winConditionUpdate()
    {        
        taxiDataText.text = gm.TaxiCurrent + "/" + gm.TaxiCount;
        vanDataText.text = gm.VanCurrent + "/" + gm.VanCount;
        ambulanceDataText.text = gm.AmbulanceCurrent + "/" + gm.AmbulanceCount;

        if (currentTaxi != gm.TaxiCurrent)
        {
            if (currentTaxi < gm.TaxiCurrent)
            {
                StartCoroutine(showColorTMP(Color.white, Color.green, 0.3f, taxiDataText));
            }
            else
            {
                StartCoroutine(showColorTMP(Color.white, Color.red, 0.3f, taxiDataText));
            }
            taxiRect.DOPunchScale(dataShakeAmount, 0.2f).SetEase(Ease.InOutFlash);
            currentTaxi = gm.TaxiCurrent;
        }

        if (currentVan != gm.VanCurrent)
        {
            if (currentVan < gm.VanCurrent)
            {
                StartCoroutine(showColorTMP(Color.white, Color.green, 0.3f, vanDataText));
            }
            else
            {
                StartCoroutine(showColorTMP(Color.white, Color.red, 0.3f, vanDataText));
            }
            vanRect.DOPunchScale(dataShakeAmount, 0.2f).SetEase(Ease.InOutFlash);
            currentVan = gm.VanCurrent;
        }

        if (currentAmbu != gm.AmbulanceCurrent)
        {
            if (currentAmbu < gm.AmbulanceCurrent)
            {
                StartCoroutine(showColorTMP(Color.white, Color.green, 0.3f, ambulanceDataText));
            }
            else
            {
                StartCoroutine(showColorTMP(Color.white, Color.red, 0.3f, ambulanceDataText));
            }
            ambuRect.DOPunchScale(dataShakeAmount, 0.2f).SetEase(Ease.InOutFlash);
            currentAmbu = gm.AmbulanceCurrent;
        }
    }
    private IEnumerator showColorTMP(Color standartColor, Color newColor, float _timer, TextMeshProUGUI tmp)
    {
        tmp.color = newColor;
        yield return new WaitForSeconds(_timer);
        tmp.color = standartColor;
    }

    public static void BackImageBlack(bool isBlack, float _time)
    {
        try
        {
            Image image = GameObject.Find("Back image").GetComponent<Image>();

            if (isBlack)
            {
                image.DOFade(1, _time);
            }
            else
            {
                image.DOFade(0, _time);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        
    }

    public void GameTimerStatus(bool isStart) => isPauseTimer = !isStart;

    private void Update()
    {
        if (!isPauseTimer)
        {
            currentTime -= Time.deltaTime;

            if (_timer >= 1f)
            {
                _timer = 0f;
                ShowTimeOnTimer(currentTime);
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }

    }

    /*
    private void LateUpdate()
    {
        Transform mainCamBody = GameManager.Instance.GetCameraBody();
        if (textOverHeadList.Count > 0)
        {            
            for (int i = 0; i < textOverHeadList.Count; i++)
            {
                textOverHeadList[i].transform.LookAt(textOverHeadList[i].transform.position + mainCamBody.rotation * Vector3.back,
                                                     mainCamBody.rotation * Vector3.up);
            }
        }

    }*/

    private void ShowTimeOnTimer(float _time)
    {
        if (_time <= 0) 
        {
            timerText.color = Color.red;
            timerText.text = "00:00";
            return;
        }

        if (_time < 10)
        {
            timerText.text = $"00:0{(int)_time}";
        }
        else if (_time >= 10 && _time < 60)
        {
            timerText.text = $"00:{(int)_time}";
        }
        else if (_time >= 60)
        {
            int mins = (int)_time / 60;
            string minsText = mins < 10 ? $"0{mins}" : $"{mins}";

            float sec = _time - 60;
            string secText = sec < 10 ? $"0{(int)sec}" : $"{(int)sec}";
            
            timerText.text = $"{minsText}:{secText}";
        }


        if (_time > (allTime * 0.15f))
        {
            timerText.color = Color.yellow;            
        }
        else
        {
            timerText.color = Color.red;
            timerTextRect.DOPunchScale(dataShakeAmount, 0.2f).SetEase(Ease.InOutFlash);
            gm.GetSoundUI().PlayUISound(SoundsUI.tick);
        }
    }

}
