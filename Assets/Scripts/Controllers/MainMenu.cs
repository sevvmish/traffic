using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AssetManager assetManager;
    [SerializeField] private SoundController sound;

    [SerializeField] private Ambient ambient;

    [SerializeField] private GameObject Visual01;
    [SerializeField] private GameObject Map01;

    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform cameraPosition1;
    [SerializeField] private Transform cameraPosition2;

    [SerializeField] private Button playButton;


    private void Awake()
    {
        Screen.SetResolution(1200, 600, true);
        UIManager.BackImageBlack(true, 0);
        UIManager.BackImageBlack(false, 1f);
                
        Visual01.SetActive(false);
        Map01.SetActive(false);
        mainCamera.position = Vector3.zero;
        mainCamera.rotation = Quaternion.identity;

        ambient.SetData(AmbientType.forest);

        playButton.onClick.AddListener(() => 
        {
            sound.PlayUISound(SoundsUI.positive);
            StartCoroutine(playMap01());
        });
    }

    private void Start()
    {
        if (!Globals.IsGameStarted)
        {
            Globals.IsGameStarted = true;
            StartCoroutine(stage1());
        }
        else
        {
            //
        }
    }

    private IEnumerator stage1()
    {
        Visual01.SetActive(true);

        playButton.gameObject.SetActive(true);
        playButton.transform.DOPunchPosition(Vector3.one * 50, 0.3f).SetEase(Ease.OutQuad);
        
        mainCamera.DOMove(cameraPosition1.position, 1f).SetEase(Ease.OutSine);
        mainCamera.DORotate(cameraPosition1.eulerAngles, 1f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(0.2f);
        sound.PlayUISound(SoundsUI.pop);
    }

    private IEnumerator playMap01()
    {
        ambient.SetData(AmbientType.desert);

        Visual01.SetActive(false);
        playButton.gameObject.SetActive(false);
        Map01.SetActive(true);

        mainCamera.DOMove(cameraPosition2.position, 0.5f).SetEase(Ease.OutSine);
        mainCamera.DORotate(cameraPosition2.eulerAngles, 0.5f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(0.2f);
        
    }
}
