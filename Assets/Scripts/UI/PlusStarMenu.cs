using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusStarMenu : MonoBehaviour
{
    public bool isStayInCurrentScene;

    [SerializeField] private GameObject allStar;
    

    public void StartPlusStar()
    {
        gameObject.SetActive(true);
        StartCoroutine(play());
    }
    private IEnumerator play()
    {
        allStar.SetActive(true);

        allStar.transform.localScale = Vector3.zero;

        allStar.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
        SoundController.Instance.PlayUISound(SoundsUI.win);
        yield return new WaitForSeconds(0.5f);

        allStar.transform.DOShakeScale(0.3f, 1f, 30).SetEase(Ease.OutQuad);        
        yield return new WaitForSeconds(1f);

        RectTransform rect = allStar.GetComponent<RectTransform>();
        rect.DOAnchorPos(new Vector2(0, 250), 0.5f).SetEase(Ease.InOutSine);
        allStar.transform.DOScale(Vector3.zero, 0.5f);
        SoundController.Instance.PlayUISound(SoundsUI.swallow);
        yield return new WaitForSeconds(0.3f);
        MainMenu.AddStarsUI(1);

        yield return new WaitForSeconds(0.3f);
        if (!isStayInCurrentScene) GameManager.Instance.BackToMainMenu(false);
    }
}
