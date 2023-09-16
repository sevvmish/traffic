using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightsController : MonoBehaviour
{
    [SerializeField] private GameObject[] greens;
    [SerializeField] private GameObject[] reds;
    [SerializeField] private GameObject[] yellows;

    private const float YELLOW_KOEFF = 0.3f;
    
    void Start()
    {
        AllOff();
    }

    public void UpdateLighter(float currentTimer, float maxTimer)
    {
        if (currentTimer < maxTimer * (1-YELLOW_KOEFF))
        {
            SetRed();
        }
        else if (currentTimer > (maxTimer * (1 - YELLOW_KOEFF)) && currentTimer < maxTimer*0.95f)
        {
            SetYellow();
        }
        else if (currentTimer >= maxTimer*0.95f)
        {
            SetGreen();
        }
    }

    private void SetRed()
    {
        AllOff(reds);

        if (reds.Length > 0)
        {
            
            for (int i = 0; i < reds.Length; i++)
            {
                if (!reds[i].activeSelf) reds[i].SetActive(true);
            }
        }
    }

    private void SetGreen()
    {
        AllOff(greens);

        if (greens.Length > 0)
        {
            for (int i = 0; i < greens.Length; i++)
            {
                if (!greens[i].activeSelf) greens[i].SetActive(true);
            }
        }
    }

    private void SetYellow()
    {
        AllOff(yellows);

        if (yellows.Length > 0)
        {
            for (int i = 0; i < yellows.Length; i++)
            {
                if (!yellows[i].activeSelf) yellows[i].SetActive(true);
            }
        }
    }

    private void AllOff(GameObject[] except)
    {
        if (greens.Length > 0 && !greens.Equals(except))
        {
            for (int i = 0; i < greens.Length; i++)
            {
                if (greens[i].activeSelf) greens[i].SetActive(false);
            }
        }

        if (reds.Length > 0 && !reds.Equals(except))
        {
            for (int i = 0; i < reds.Length; i++)
            {
                if (reds[i].activeSelf) reds[i].SetActive(false);
            }
        }

        if (yellows.Length > 0 && !yellows.Equals(except))
        {
            for (int i = 0; i < yellows.Length; i++)
            {
                if (yellows[i].activeSelf) yellows[i].SetActive(false);
            }
        }
    }

    private void AllOff()
    {
        if (greens.Length > 0)
        {
            for (int i = 0; i < greens.Length; i++)
            {
                if (greens[i].activeSelf) greens[i].SetActive(false);
            }
        }

        if (reds.Length > 0)
        {
            for (int i = 0; i < reds.Length; i++)
            {
                if (reds[i].activeSelf) reds[i].SetActive(false);
            }
        }

        if (yellows.Length > 0)
        {
            for (int i = 0; i < yellows.Length; i++)
            {
                if (yellows[i].activeSelf) yellows[i].SetActive(false);
            }
        }
    }
}
