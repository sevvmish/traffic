using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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

    public void SetData()
    {
        gm = GameManager.Instance;

        taxiRect = taxiDataText.GetComponent<RectTransform>();
        vanRect = vanDataText.GetComponent<RectTransform>();
        ambuRect = ambulanceDataText.GetComponent<RectTransform>();

        taxiDataPanel.SetActive(gm.TaxiCount > 0);
        vanDataPanel.SetActive(gm.VanCount > 0);
        ambulanceDataPanel.SetActive(gm.AmbulanceCount > 0);

        if (gm.TaxiCount > 9 || gm.VanCount > 9 || gm.AmbulanceCount > 9)
        {
            taxiDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2 (175, 55);
            vanDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(175, 55);
            ambulanceDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(175, 55);
        }
        else if(gm.TaxiCount < 10 && gm.VanCount < 10 && gm.AmbulanceCount < 10)
        {
            taxiDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(120, 55);
            vanDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(120, 55);
            ambulanceDataPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(120, 55);
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
            taxiRect.DOPunchScale(dataShakeAmount, 0.2f).SetEase(Ease.InOutFlash);
        }

        if (currentVan != gm.VanCurrent)
        {
            vanRect.DOPunchScale(dataShakeAmount, 0.2f).SetEase(Ease.InOutFlash);
        }

        if (currentAmbu != gm.AmbulanceCurrent)
        {
            ambuRect.DOPunchScale(dataShakeAmount, 0.2f).SetEase(Ease.InOutFlash);
        }
    }
}
