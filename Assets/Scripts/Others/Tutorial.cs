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

    private bool isLevel1_1, isLevel1_2, isLevel1_4, isEndLevel1;
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

        if (Globals.CurrentLevel == 1 && MainMenu.GetLastLevel() == 1)
        {
            level1_tutorial_text.text = lang.Level1_Tutorial;
            level1_tutorial_text_2.text = lang.Level1_Tutorial_2;
            level1_tutorial_text_3.text = lang.Level1_Tutorial_3;
            level1_tutorial_text_4.text = lang.Level1_Tutorial_4;
            level1_tutorial_text_5.text = lang.Level1_Tutorial_5;
            new_region = GameObject.Find("tutorial_try2");
            new_region.SetActive(false);
            StartCoroutine(activateAfterSecs(1, level1_tutorial));
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (Globals.CurrentLevel == 1 && MainMenu.GetLastLevel() == 1)
        {
            if (gm.VanCurrent == 1 && !isLevel1_1)
            {
                level1_tutorial.SetActive(false);
                isLevel1_1 = true;
                GameObject g = GameObject.Find("tutorial_try1");
                gm.regionController.RemoveInfrastructure(g.GetComponent<CityInfrastructure>());
                Destroy(g);
                                
                StartCoroutine(activateAfterSecs(2f, new_region));
                StartCoroutine(activateAfterSecs(0.3f, level1_tutorial_2));
            }

            if (gm.VanCurrent == 2 && !isLevel1_2)
            {
                isLevel1_2 = true;
                level1_tutorial_4.SetActive(false);
                StartCoroutine(activateDeactivateAfterSecs(0f, level1_tutorial_3, 5f));
                StartCoroutine(activateDeactivateAfterSecs(4f, level1_tutorial_5, 5f));
            }

            if (_timer > 23 && gm.VanCurrent != 2 && !isLevel1_4)
            {
                isLevel1_4 = true;
                level1_tutorial_2.SetActive(false);
                StartCoroutine(activateDeactivateAfterSecs(0f, level1_tutorial_4, 6f));
            }

            if (gm.VanCurrent == 3 && !isEndLevel1)
            {
                isEndLevel1 = true;
                level1_tutorial_3.SetActive(false);
                level1_tutorial_4.SetActive(false);
                level1_tutorial_5.SetActive(false);
            }
        }
    }

    private IEnumerator activateAfterSecs(float delay, GameObject obj)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
    }

    private IEnumerator activateDeactivateAfterSecs(float delay, GameObject obj, float continuity)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);

        yield return new WaitForSeconds(continuity);
        obj.SetActive(false);
    }
}
