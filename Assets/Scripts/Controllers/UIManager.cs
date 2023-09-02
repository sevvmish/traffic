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
    
    public void SetData()
    {
        gm = GameManager.Instance;

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
    }

    private void winConditionUpdate()
    {        
        taxiDataText.text = gm.TaxiCurrent + "/" + gm.TaxiCount;
        vanDataText.text = gm.VanCurrent + "/" + gm.VanCount;
        ambulanceDataText.text = gm.AmbulanceCurrent + "/" + gm.AmbulanceCount;
    }
}
