using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressPointController : MonoBehaviour
{
    public int CurrentLevel = 0;
    public int CurrentLimit = 0;
    public bool IsActivated { get; private set; }

    [SerializeField] private GameObject[] fullStars;
    [SerializeField] private GameObject[] emptyStars;
    [SerializeField] private TextMeshPro levelText;
    [SerializeField] private TextMeshPro limitText;
    [SerializeField] private GameObject locker;
    [SerializeField] private GameObject baseCapsule;
    [SerializeField] private Material active;
    [SerializeField] private Material nonActive;



    // Start is called before the first frame update
    void OnEnable()
    {
        levelText.text = CurrentLevel.ToString();
        int levelPassed = MainMenu.GetLastLevel();                
        CurrentLimit = StarsLimit(CurrentLevel);

        Activate(CurrentLevel <= levelPassed);
        if (CurrentLevel == levelPassed) StartCoroutine(playShake());
    }

    private IEnumerator playShake()
    {
        while (true)
        {
            transform.DOShakeScale(0.5f, 0.5f, 30).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(1f);

            //transform.DOPunchScale(Vector3.one*0.2f, 0.3f).SetEase(Ease.OutQuad);
            //yield return new WaitForSeconds(0.7f);

            //transform.DOPunchPosition(Vector3.one * 0.2f, 0.3f).SetEase(Ease.OutQuad);
            //yield return new WaitForSeconds(0.7f);
        }

        
    }

    private void Activate(bool isActive)
    {
        locker.SetActive(!isActive);
        levelText.gameObject.SetActive(isActive);
        limitText.gameObject.SetActive(isActive);

        IsActivated = isActive;

        if (isActive)
        {
            if (CurrentLimit > 0)
            {
                limitText.text = CurrentLimit.ToString();
            }
            else
            {
                limitText.text = "";
            }

            if (CurrentLimit > MainMenu.GetStarsAmount())
            {
                limitText.color = Color.red;
            }
            else if (CurrentLimit == MainMenu.GetStarsAmount())
            {
                limitText.color = Color.grey;
            }
            else
            {
                limitText.color = Color.green;
            }

            baseCapsule.GetComponent<MeshRenderer>().material = active;

            int stars = Globals.MainPlayerData.Progress1[CurrentLevel];

            for (int i = 0; i < 3; i++)
            {
                fullStars[i].SetActive(false);
                emptyStars[i].SetActive(false);

                if (stars > i)
                {
                    fullStars[i].SetActive(true);
                    emptyStars[i].SetActive(false);
                }
                else
                {
                    fullStars[i].SetActive(false);
                    emptyStars[i].SetActive(true);
                }                
            }
        }
        else
        {
            baseCapsule.GetComponent<MeshRenderer>().material = nonActive;

            for (int i = 0; i < fullStars.Length; i++)
            {
                fullStars[i].SetActive(false);
            }

            for (int i = 0; i < fullStars.Length; i++)
            {
                emptyStars[i].SetActive(true);
            }
        }
        
    }


    public static int StarsLimit(int level)
    {
        int[] data = new int[] { 0, 
            0, 0, 0, 7, 10, 13, 16, 19, 22, 25,   //1 - 10 lvls
            26, 29, 32, 35, 38, 41, 44, 47, 50, 53,   //11 - 20 lvls
            55, 58, 61, 64, 66, 69, 72, 75, 78, 83, 86   //21 - 30 lvls
        }; 

        /*
        -    3  6    9  12  15  18  21  24  27
        30  33  36  39  42  45  48  51  54  57
        60  63  66  69  72  75  78  81  84  87
         */

        return data[level];
       
    }
}
