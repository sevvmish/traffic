using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private float _timer = 1f;
    public float GetTimeLeft() => currentTime;

    [Header("ui data gameobjects")]
    [SerializeField] private GameObject dataPanel;
    [SerializeField] private GameObject taxiDataPanel;
    [SerializeField] private GameObject vanDataPanel;
    [SerializeField] private GameObject ambulanceDataPanel;
    [SerializeField] private GameObject taxiManDataPanel;
    [SerializeField] private GameObject vanCargoDataPanel;
    [SerializeField] private GameObject ambulanceManDataPanel;

    [Header("ui data texts")]
    [SerializeField] private TextMeshProUGUI taxiDataText;
    [SerializeField] private TextMeshProUGUI vanDataText;
    [SerializeField] private TextMeshProUGUI ambulanceDataText;
    [SerializeField] private TextMeshProUGUI taxiManDataText;
    [SerializeField] private TextMeshProUGUI vanCargoDataText;
    [SerializeField] private TextMeshProUGUI ambulanceManDataText;

    [Header("options menu")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    
    private GameManager gm;
    private int currentTaxi;
    private int currentVan;
    private int currentAmbu;
    private int currentTaxiMan;
    private int currentVanCargo;
    private int currentAmbuMan;

    private RectTransform taxiRect;
    private RectTransform vanRect;
    private RectTransform ambuRect;
    private RectTransform taxiManRect;
    private RectTransform vanCargoRect;
    private RectTransform ambuManRect;

    private Vector3 dataShakeAmount = new Vector3(0.4f, 0.6f, 0);
    private Vector2 bigSizeUIFrame = new Vector2(127, 35);
    private Vector2 smallSizeUIFrame = new Vector2(95, 35);


    private SoundController _sound;
    
    public void SetData(float timeForGame)
    {
        _sound = SoundController.Instance;
        dataPanel.SetActive(false);
        optionsPanel.SetActive(false);
        optionsButton.gameObject.SetActive(true);
        if (Globals.IsSoundOn)
        {
            soundButton.GetComponent<Image>().sprite = soundOnSprite;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = soundOffSprite;
        }

        timerTextRect = timerText.GetComponent<RectTransform>();
        timerPanel.SetActive(false);
        allTime = timeForGame;
        currentTime = timeForGame + 0.1f;
        ShowTimeOnTimer(currentTime);

        gm = GameManager.Instance;

        taxiRect = taxiDataText.GetComponent<RectTransform>();
        vanRect = vanDataText.GetComponent<RectTransform>();
        ambuRect = ambulanceDataText.GetComponent<RectTransform>();
        taxiManRect = taxiManDataText.GetComponent<RectTransform>();
        vanCargoRect = vanCargoDataText.GetComponent<RectTransform>();
        ambuManRect = ambulanceManDataText.GetComponent<RectTransform>();

        taxiDataPanel.SetActive(gm.TaxiCount > 0);
        vanDataPanel.SetActive(gm.VanCount > 0);
        ambulanceDataPanel.SetActive(gm.AmbulanceCount > 0);
        taxiManDataPanel.SetActive(gm.TaxiManCount > 0);
        vanCargoDataPanel.SetActive(gm.VanCargoCount > 0);
        ambulanceManDataPanel.SetActive(gm.AmbulanceManCount > 0);

        if (gm.TaxiCount > 9 || gm.VanCount > 9 || gm.AmbulanceCount > 9 || gm.TaxiManCount > 9 || gm.VanCargoCount > 9 || gm.AmbulanceManCount > 9)
        {
            taxiDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = bigSizeUIFrame;
            vanDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = bigSizeUIFrame;
            ambulanceDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = bigSizeUIFrame;
            taxiManDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = bigSizeUIFrame;
            vanCargoDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = bigSizeUIFrame;
            ambulanceManDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = bigSizeUIFrame;
        }
        else if(gm.TaxiCount < 10 && gm.VanCount < 10 && gm.AmbulanceCount < 10 && gm.TaxiManCount < 10 && gm.VanCargoCount < 10 && gm.AmbulanceManCount < 10)
        {
            taxiDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = smallSizeUIFrame;
            vanDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = smallSizeUIFrame;
            ambulanceDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = smallSizeUIFrame;
            taxiManDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = smallSizeUIFrame;
            vanCargoDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = smallSizeUIFrame;
            ambulanceManDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = smallSizeUIFrame;
        }

        winConditionUpdate();
        gm.OnChangedConditionsAmount = winConditionUpdate;

        currentTaxi = gm.TaxiCurrent;
        currentVan = gm.VanCurrent;
        currentAmbu = gm.AmbulanceCurrent;
        currentTaxiMan = gm.TaxiManCurrent;
        currentVanCargo = gm.VanCargoCurrent;
        currentAmbuMan = gm.AmbulanceManCurrent;

        //options
        optionsButton.onClick.AddListener(() => 
        {
            _sound.PlayUISound(SoundsUI.click);
            optionsButton.gameObject.SetActive(false);
            optionsPanel.SetActive(true);
            gm.SetGameStatus(false);

            continueButton.transform.localScale = Vector3.zero;
            soundButton.transform.localScale = Vector3.zero;
            homeButton.transform.localScale = Vector3.zero;

            continueButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            soundButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            homeButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        });

        continueButton.onClick.AddListener(() =>
        {
            _sound.PlayUISound(SoundsUI.click);
            optionsButton.gameObject.SetActive(true);
            optionsPanel.SetActive(false);
            gm.SetGameStatus(true);            
        });

        soundButton.onClick.AddListener(() =>
        {            
            if (Globals.IsSoundOn)
            {
                Globals.IsSoundOn = false;
                soundButton.GetComponent<Image>().sprite = soundOffSprite;
                AudioListener.volume = 0;
            }
            else
            {
                _sound.PlayUISound(SoundsUI.click);
                Globals.IsSoundOn = true;
                soundButton.GetComponent<Image>().sprite = soundOnSprite;
                AudioListener.volume = 1f;
            }

            SaveLoadManager.Save();
        });

        homeButton.onClick.AddListener(() =>
        {
            gm.BackToMainMenu(true);
        });
    }

    public void TurnOffOptions()
    {
        optionsPanel.SetActive(false);
        optionsButton.gameObject.SetActive(false);
    }

    public void TurnOnOptions()
    {
        optionsPanel.SetActive(false);
        optionsButton.gameObject.SetActive(true);
    }

    public void AddSeconds(float seconds)
    {
        allTime += seconds;
        currentTime += seconds + 0.1f;
    }

    

    private void winConditionUpdate()
    {        
        taxiDataText.text = gm.TaxiCurrent + "/" + gm.TaxiCount;
        vanDataText.text = gm.VanCurrent + "/" + gm.VanCount;
        ambulanceDataText.text = gm.AmbulanceCurrent + "/" + gm.AmbulanceCount;
        taxiManDataText.text = gm.TaxiManCurrent + "/" + gm.TaxiManCount;
        vanCargoDataText.text = gm.VanCargoCurrent + "/" + gm.VanCargoCount;
        ambulanceManDataText.text = gm.AmbulanceManCurrent + "/" + gm.AmbulanceManCount;

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

        if (currentTaxiMan != gm.TaxiManCurrent)
        {
            if (currentTaxiMan < gm.TaxiManCurrent)
            {
                StartCoroutine(showColorTMP(Color.white, Color.green, 0.3f, taxiManDataText));
            }
            else
            {
                StartCoroutine(showColorTMP(Color.white, Color.red, 0.3f, taxiManDataText));
            }
            taxiManRect.DOPunchScale(dataShakeAmount, 0.2f).SetEase(Ease.InOutFlash);
            currentTaxiMan = gm.TaxiManCurrent;
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

        if (currentVanCargo != gm.VanCargoCurrent)
        {
            if (currentVanCargo < gm.VanCargoCurrent)
            {
                StartCoroutine(showColorTMP(Color.white, Color.green, 0.3f, vanCargoDataText));
            }
            else
            {
                StartCoroutine(showColorTMP(Color.white, Color.red, 0.3f, vanCargoDataText));
            }
            vanCargoRect.DOPunchScale(dataShakeAmount, 0.2f).SetEase(Ease.InOutFlash);
            currentVanCargo = gm.VanCargoCurrent;
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

        if (currentAmbuMan != gm.AmbulanceManCurrent)
        {
            if (currentAmbuMan < gm.AmbulanceManCurrent)
            {
                StartCoroutine(showColorTMP(Color.white, Color.green, 0.3f, ambulanceManDataText));
            }
            else
            {
                StartCoroutine(showColorTMP(Color.white, Color.red, 0.3f, ambulanceManDataText));
            }
            ambuManRect.DOPunchScale(dataShakeAmount, 0.2f).SetEase(Ease.InOutFlash);
            currentAmbuMan = gm.AmbulanceManCurrent;
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

    public void GameTimerStatus(bool isStart)
    {        
        dataPanel.SetActive(isStart);
        timerPanel.SetActive(isStart);
        isPauseTimer = !isStart;
    }

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

            float sec = _time - mins * 60;
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
