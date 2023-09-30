using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("level1")]
    [SerializeField] private GameObject level1_tutorial;
    [SerializeField] private TextMeshProUGUI level1_tutorial_text;
    [SerializeField] private GameObject new_region;
    [SerializeField] private Transform parts;
    [SerializeField] private GameObject level1_tutorial_2;
    [SerializeField] private TextMeshProUGUI level1_tutorial_text_2;
    [SerializeField] private GameObject level1_tutorial_3;
    [SerializeField] private TextMeshProUGUI level1_tutorial_text_3;
    [SerializeField] private GameObject level1_tutorial_4;
    [SerializeField] private TextMeshProUGUI level1_tutorial_text_4;
    [SerializeField] private GameObject level1_tutorial_5;
    [SerializeField] private TextMeshProUGUI level1_tutorial_text_5;

    [SerializeField] private GameObject tutorial1AdditionalTutorial;
    [SerializeField] private Region region;
    [SerializeField] private Transform cameraBody;
    private Vector3 camerabodyOld;
    [SerializeField] private GameObject leftHighlight;
    [SerializeField] private GameObject rightHighlight;
    [SerializeField] private GameObject leftUnclick;
    [SerializeField] private GameObject rightUnclick;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;

    [SerializeField] private GameObject preClickMessage;
    [SerializeField] private TextMeshProUGUI preClickMessageText;

    [SerializeField] private GameObject LeftClickMessage;
    [SerializeField] private TextMeshProUGUI LeftClickMessageText;
    [SerializeField] private GameObject RightClickMessage;
    [SerializeField] private TextMeshProUGUI RightClickMessageText;

    private bool isTutorialRotationDone;
    private float secOfDone;


    [Header("level2")]
    [SerializeField] private GameObject level2_tutorial;
    [SerializeField] private TextMeshProUGUI level2_tutorial_text;

    [Header("level5")]
    [SerializeField] private GameObject level5_tutorial;
    [SerializeField] private TextMeshProUGUI level5_tutorial_text;
    [SerializeField] private GameObject level5_tutorial_1;
    [SerializeField] private TextMeshProUGUI level5_tutorial_text_1;

    [Header("level11")]
    [SerializeField] private GameObject level11_tutorial;
    [SerializeField] private TextMeshProUGUI level11_tutorial_text;
    [SerializeField] private GameObject level11_tutorial_1;
    [SerializeField] private TextMeshProUGUI level11_tutorial_text_1;

    [Header("level12")]
    [SerializeField] private GameObject level12_tutorial;
    [SerializeField] private TextMeshProUGUI level12_tutorial_text;

    private bool isLevel1_1, isLevel1_2, isLevel1_4, isEndLevel1;
    private bool isLevel11_1;
    private float _timer;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        Translation lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        level1_tutorial.SetActive(false);
        level1_tutorial_2.SetActive(false);
        level1_tutorial_3.SetActive(false);
        level1_tutorial_4.SetActive(false);
        level1_tutorial_5.SetActive(false);
        level5_tutorial.SetActive(false);
        level11_tutorial.SetActive(false);
        level11_tutorial_1.SetActive(false);

        if (Globals.CurrentLevel == 1)
        {
            tutorial1AdditionalTutorial.SetActive(true);
            region.SetData(gm.regionController);
            region.gameObject.SetActive(false);

            preClickMessageText.text = lang.Level1_Tutorial_preclick;
            LeftClickMessageText.text = lang.Level1_Tutorial_leftClick;
            RightClickMessageText.text = lang.Level1_Tutorial_rightClick;

            leftHighlight.SetActive(false);
            rightHighlight.SetActive(false);
            leftUnclick.SetActive(false);
            rightUnclick.SetActive(false);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);

            level1_tutorial_text.text = lang.Level1_Tutorial;
            level1_tutorial_text_2.text = lang.Level1_Tutorial_2;
            level1_tutorial_text_3.text = lang.Level1_Tutorial_3;
            level1_tutorial_text_4.text = lang.Level1_Tutorial_4;
            level1_tutorial_text_5.text = lang.Level1_Tutorial_5;
            new_region = GameObject.Find("tutorial_try2");
            new_region.SetActive(false);
            StartCoroutine(activateAfterSecs(1, level1_tutorial));            
        }

        /*
        if (Globals.CurrentLevel == 2 && MainMenu.GetLastLevel() == 2)
        {
            level2_tutorial_text.text = lang.Level2_Tutorial;            
            StartCoroutine(activateDeactivateAfterSecs(1, level2_tutorial, 6));
        }*/

        if (Globals.CurrentLevel == 5 && MainMenu.GetLastLevel() == 5)
        {
            level5_tutorial_text.text = lang.Level5_Tutorial;
            level5_tutorial_text_1.text = lang.Level5_Tutorial_1;
            StartCoroutine(activateAfterSecs(1, level5_tutorial));
            StartCoroutine(activateDeactivateAfterSecs(10f, level5_tutorial_1, 6f));
        }

        if (Globals.CurrentLevel == 11)
        {
            level11_tutorial_text.text = lang.Level11_Tutorial;
            level11_tutorial_text_1.text = lang.Level11_Tutorial_1;

            StartCoroutine(activateAfterSecs(1, level11_tutorial));

            new_region = GameObject.Find("tutorial_try2");
            new_region.SetActive(false);
        }

        if (Globals.CurrentLevel == 12)
        {
            level12_tutorial_text.text = lang.Level12_Tutorial;
            StartCoroutine(activateDeactivateAfterSecs(1f, level12_tutorial, 6));
        }
    }

    private void Update()
    {
        if (!gm.IsGameStarted) return;
        _timer += Time.deltaTime;

        if (Globals.CurrentLevel == 1)
        {
            if (gm.VanCurrent == 1 && !isLevel1_1)
            {
                level1_tutorial.SetActive(false);
                isLevel1_1 = true;
                GameObject g = GameObject.Find("tutorial_try1");
                gm.regionController.RemoveInfrastructure(g.GetComponent<CityInfrastructure>());
                Destroy(g);
                                
                StartCoroutine(activateAfterSecs(2f, new_region));
                StartCoroutine(activateDeactivateAfterSecs(0.3f, level1_tutorial_2, 6));
                StartCoroutine(playLevel1Rotation());
            }

            if (gm.VanCurrent == 2 && !isLevel1_2)
            {
                isLevel1_2 = true;
                level1_tutorial_4.SetActive(false);
                StartCoroutine(activateDeactivateAfterSecs(0f, level1_tutorial_3, 5f));
                GameManager.Instance.GetUI().SetDataPanelScale(2);
                StartCoroutine(playIncreaseTimer());
                StartCoroutine(activateDeactivateAfterSecs(4f, level1_tutorial_5, 5f));
            }

            
            if (gm.VanCurrent == 3 && !isEndLevel1)
            {
                isEndLevel1 = true;
                level1_tutorial_3.SetActive(false);
                level1_tutorial_4.SetActive(false);
                level1_tutorial_5.SetActive(false);
            }
        }

        if (Globals.CurrentLevel == 11)
        {
            if (gm.AccidentsCurrent == 1 && !isLevel11_1)
            {
                level11_tutorial.SetActive(false);
                isLevel11_1 = true;

                StartCoroutine(playAccidentTutorial11());
                StartCoroutine(activateDeactivateAfterSecs(6f, level11_tutorial_1, 6));
            }
        }



    }

    private IEnumerator playAccidentTutorial11()
    {
        yield return new WaitForSeconds(3.1f);
        GameObject g = GameObject.Find("tutorial_try1");
        gm.regionController.RemoveInfrastructure(g.GetComponent<CityInfrastructure>());
        Destroy(g);
        new_region.SetActive(true);
    }

    private IEnumerator activateAfterSecs(float delay, GameObject obj)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
    }

    private IEnumerator playIncreaseTimer()
    {
        yield return new WaitForSeconds(4);
        GameManager.Instance.GetUI().SetTimePanelScale(2);
    }

    private IEnumerator activateDeactivateAfterSecs(float delay, GameObject obj, float continuity)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);

        yield return new WaitForSeconds(continuity);
        obj.SetActive(false);
    }

    private IEnumerator playLevel1Rotation()
    {
        yield return new WaitForSeconds(0.5f);
        
        while (level1_tutorial_2.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
        }

        secOfDone = _timer;

        camerabodyOld = cameraBody.position;
        cameraBody.DOMove(new Vector3(cameraBody.position.x, cameraBody.position.y - 50, cameraBody.position.z), 0.3f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.3f);
        region.gameObject.SetActive(true);
        region.transform.localEulerAngles = Vector3.zero;
        yield return new WaitForSeconds(0.2f);

        preClickMessage.SetActive(true);
        preClickMessage.transform.localScale = Vector3.zero;
        preClickMessage.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutElastic);
        SoundController.Instance.PlayUISound(SoundsUI.positive);
        yield return new WaitForSeconds(3f);
        preClickMessage.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutElastic);
        //preClickMessage.SetActive(false);

        //============================

        leftArrow.SetActive(true);
        rightUnclick.SetActive(true);
        StartCoroutine(blink(leftHighlight));
        LeftClickMessage.SetActive(true);
        LeftClickMessage.transform.localScale = Vector3.zero;
        LeftClickMessage.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutElastic);

        while ((int)region.transform.localEulerAngles.y != 240)
        {            
            yield return new WaitForSeconds(0.1f);
        }

        SoundController.Instance.PlayUISound(SoundsUI.win);
        StopCoroutine(blink(leftHighlight));
        yield return new WaitForSeconds(0.2f);

        //==========================

        leftArrow.SetActive(false);
        rightUnclick.SetActive(false);
        LeftClickMessage.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutElastic);
        region.transform.DOLocalRotate(Vector3.zero, 0.3f);

        StartCoroutine(blink(rightHighlight));
        rightArrow.SetActive(true);
        leftUnclick.SetActive(true);
        RightClickMessage.SetActive(true);
        RightClickMessage.transform.localScale = Vector3.zero;
        RightClickMessage.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutElastic);

        while ((int)region.transform.localEulerAngles.y != 120)
        {
            yield return new WaitForSeconds(0.1f);
        }

        SoundController.Instance.PlayUISound(SoundsUI.win);
        StopCoroutine(blink(rightHighlight));
        yield return new WaitForSeconds(0.3f);

        //========================================

        RightClickMessage.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutElastic);
        cameraBody.DOMove(camerabodyOld, 0.3f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.3f);
        tutorial1AdditionalTutorial.SetActive(false);
        preClickMessage.SetActive(false);
        RightClickMessage.SetActive(false);
        LeftClickMessage.SetActive(false);

        isTutorialRotationDone = true;
        secOfDone = _timer - secOfDone;

        yield return new WaitForSeconds(0.3f);
        level1_tutorial_2.SetActive(false);
        StartCoroutine(activateDeactivateAfterSecs(0f, level1_tutorial_4, 6f));
    }


    private IEnumerator blink(GameObject g)
    {
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                g.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                g.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

}
