using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActOnClick : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject[] toActivate;
    [SerializeField] private GameObject[] toDeactivate;

    // Start is called before the first frame update
    void Start()
    {
        Globals.IsInfoActive = true;

        button.onClick.AddListener(() => 
        {
            SoundController.Instance.PlayUISound(SoundsUI.tick);
            Globals.IsInfoActive = false;

            for (int i = 0; i < toActivate.Length; i++)
            {
                toActivate[i].SetActive(true);
            }

            for (int i = 0; i < toDeactivate.Length; i++)
            {
                toDeactivate[i].SetActive(false);
            }

        });
    }

    private void OnEnable()
    {
        Globals.IsInfoActive = true;
    }


}
