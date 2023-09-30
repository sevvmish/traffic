using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EducationMode : MonoBehaviour
{
    public bool IsDone;

    [SerializeField] private GameObject sign;
    [SerializeField] private TextMeshProUGUI signText;

    [SerializeField] private Button endButton;
    [SerializeField] private TextMeshProUGUI endButtonText;

    private float coolDown;
    private GameManager gm;
    private Region[] regionToOperate = new Region[10];

    private Conditions[] level6Plan = new Conditions[] {
        new Conditions(27.69f,-1,0),
        new Conditions(26.6f,1,0),
        new Conditions(24.29f,1,0),
        new Conditions(23.53f,-1,0),
        new Conditions(20.97f,-1,0),
        new Conditions(20.19f,1,0),
        new Conditions(17.77f,1,0),
        new Conditions(16.99f,-1,0),
        new Conditions(15.09f,-1,0),
        new Conditions(14.3f,1,0),
        new Conditions(11.97f,1,0),
        new Conditions(11.25f,-1,0)
    };

    private Conditions[] level9Plan = new Conditions[] {
        new Conditions(37.68f,-1,0),
        new Conditions(36.93f,1,1),
        new Conditions(36.52f,1,1),
        new Conditions(35.59f,1,1),
        new Conditions(35.13f,1,1),
        new Conditions(34.61f,1,1),
        new Conditions(29.31f,1,1),
        new Conditions(28.84f,1,1),
        new Conditions(28.36f,1,1),
        new Conditions(27.57f,1,1),
        new Conditions(27.1f,1,1),
        new Conditions(26.57f,1,1),
        new Conditions(20.74f,-11,1),
        new Conditions(18.95f,-1,1)
    };

    private Conditions[] level10Plan = new Conditions[] {
        new Conditions(44.06f,1,1),
        new Conditions(43.19f,1,0),
        new Conditions(42.19f,1,0),
        new Conditions(41.77f,1,0),
        new Conditions(41.14f,1,0),
        new Conditions(38.26f,-1,1),
        new Conditions(37.89f,-1,1),
        new Conditions(37.09f,1,0),
        new Conditions(36.46f,1,0),
        new Conditions(36.07f,1,0),
        new Conditions(35.32f,1,0),
        new Conditions(34.84f,1,0),
        new Conditions(34.39f,1,0),
        new Conditions(31.12f,-1,1),
        new Conditions(30.59f,-1,1),
        new Conditions(29.24f,1,0),
        new Conditions(28.69f,1,0),
        new Conditions(28.35f,1,0),
        new Conditions(27.80f,1,0),
        new Conditions(27.34f,1,0),
        new Conditions(26.89f,1,0),
        new Conditions(26.37f,1,1),
        new Conditions(23.25f,-1,1),
        new Conditions(22.87f,-1,1)

    };

    private Conditions[] level12Plan = new Conditions[] {
        new Conditions(36.10f, 1, 0),
        new Conditions(34.25f, 1, 0),
        new Conditions(33.60f, -1, 0),
        new Conditions(33.22f, -1, 0),
        new Conditions(32.75f, -1, 0),
        new Conditions(31.25f, -1, 0),
        new Conditions(28.56f, 1, 0),
        new Conditions(28.21f, 1, 0),
        new Conditions(27.73f, 1, 0),
        new Conditions(26.10f, 1, 0),
        new Conditions(25.46f, -1, 0),
        new Conditions(25.03f, -1, 0),
        new Conditions(24.63f, -1, 0),
        new Conditions(23.01f, -1, 0),
        new Conditions(20.34f, 1, 0),
        new Conditions(19.99f, 1, 0),
        new Conditions(19.56f, 1, 0),
        new Conditions(17.74f, 1, 0),
        new Conditions(17.01f, -1, 0),
        new Conditions(16.61f, -1, 0),
        new Conditions(16.24f, -1, 0),
        new Conditions(14.52f, -1, 0)
    };

    private Conditions[] level15Plan = new Conditions[] {
        new Conditions(35.10f, 1, 0)
    };

    private Conditions[] level16Plan = new Conditions[] {
        new Conditions(49.68f, -1, 2),
        new Conditions(49.24f, -1, 2),
        new Conditions(48.42f, 1, 0),
        new Conditions(48.02f, 1, 0),
        new Conditions(45.75f, 1, 1),
        new Conditions(44.92f, -1, 1),
        new Conditions(39.06f, 1, 1),
        new Conditions(38.23f, -1, 1),
        new Conditions(32.06f, 1, 1),
        new Conditions(30.33f, -1, 1),
        new Conditions(25.89f, 1, 1)
    };


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        Translation lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();

        signText.text = lang.ForEducationSpectator;
        endButtonText.text = lang.ForEducationEndButton;

        endButton.gameObject.SetActive(false);
        sign.SetActive(true);

        endButton.onClick.AddListener(() => 
        {
            Globals.IsSpectatorMode = false;
            gm.RestartLevel();
        });

        switch (Globals.CurrentLevel)
        {
            case 6:
                regionToOperate[0] = GameObject.Find("main1").GetComponent<Region>();
                break;

            case 9:
                regionToOperate[0] = GameObject.Find("main1").GetComponent<Region>();
                regionToOperate[1] = GameObject.Find("main2").GetComponent<Region>();
                break;

            case 10:
                regionToOperate[0] = GameObject.Find("main0").GetComponent<Region>();
                regionToOperate[1] = GameObject.Find("main1").GetComponent<Region>();
                break;

            case 12:
                regionToOperate[0] = GameObject.Find("main0").GetComponent<Region>();                
                break;

            case 15:
                regionToOperate[0] = GameObject.Find("main0").GetComponent<Region>();
                regionToOperate[1] = GameObject.Find("main1").GetComponent<Region>();
                break;

            case 16:
                regionToOperate[0] = GameObject.Find("main0").GetComponent<Region>();
                regionToOperate[1] = GameObject.Find("main1").GetComponent<Region>();
                regionToOperate[2] = GameObject.Find("main2").GetComponent<Region>();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDone && !endButton.gameObject.activeSelf)
        {
            endButton.gameObject.SetActive(true);
            SoundController.Instance.PlayUISound(SoundsUI.win);
            //TO DELETE
            //gm.RestartLevel();
        }

        if (coolDown>0)
        {
            coolDown -= Time.deltaTime;
            return;
        }

        switch(Globals.CurrentLevel)
        {
            case 6:
                levelPlay(level6Plan);
                break;

            case 9:
                levelPlay(level9Plan);
                break;

            case 10:
                levelPlay(level10Plan);
                break;

            case 12:
                levelPlay(level12Plan);
                break;

            case 15:
                levelPlay(level15Plan);
                break;

            case 16:
                levelPlay(level16Plan);
                break;
        }
    }

    private void levelPlay(Conditions[] levelPlan)
    {
        float curTime = gm.GetUI().GetTimeLeft();

        for (int i = 0; i < levelPlan.Length; i++)
        {
            /*
            if (levelPlan[i]._timer < curTime && levelPlan[i]._timer > (curTime - 0.2f))
            {
                regionToOperate[levelPlan[i].region].RotateRegion(levelPlan[i].sign);
                //coolDown = 0.1f;
                break;
            }*/

            if ( Mathf.Abs(levelPlan[i]._timer - curTime) < 0.1f)
            {
                regionToOperate[levelPlan[i].region].RotateRegion(levelPlan[i].sign);
                //coolDown = 0.1f;
                break;
            }
        }

    }


    public struct Conditions
    {
        public float _timer;
        public int sign;
        public int region;

        public Conditions(float timer, int sign, int reg)
        {
            _timer = timer;
            this.sign = sign;
            region = reg;
        }
    }
}
