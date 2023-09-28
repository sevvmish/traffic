using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInfrastructure : MonoBehaviour, IRotate
{
    public float RotationAngle = 0;
    [SerializeField] private Transform mainBody;
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject edgeEffect;
    [SerializeField] private AudioSource _audioSource;
        
    public bool IsBusyRotate { get; set; }
    public void SetRotationAngle(float angle)
    {        
        RotationAngle = angle;
        border.SetActive(RotationAngle > 0);
    }
        
    private bool IsActive = true;

    private void Start()
    {
        IsBusyRotate = false;
        IsActive = true;
        border.SetActive(RotationAngle > 0);
    }

    public void RotateRegion(int sign)
    {
        if (IsBusyRotate || !IsActive || RotationAngle == 0)
        {
            /*if (RotationAngle > 0) */playError();
            return;
        }

        StartCoroutine(rotatePart(sign));
    }

    private void playError()
    {
        GameManager.Instance.GetSoundUI().PlayUISound(SoundsUI.error);
    }

    private IEnumerator rotatePart(int sign)
    {
        Vector3 pos = mainBody.position;
        IsBusyRotate = true;
        IsActive = false;
                
        _audioSource.Play();
        edgeEffect?.GetComponent<ParticleSystem>().Play();

        mainBody.DOPunchPosition(new Vector3(0, 1, 0), 0.05f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.025f);
        mainBody.DOPunchPosition(new Vector3(0, 0.5f, 0), 0.05f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.025f);


        mainBody.DORotate(new Vector3(mainBody.eulerAngles.x, mainBody.eulerAngles.y + RotationAngle * sign, mainBody.eulerAngles.z), Globals.SWIPE_SPEED);
        yield return new WaitForSeconds(Globals.SWIPE_SPEED);
        mainBody.position = pos;

        IsBusyRotate = false;
        IsActive = true;
        GameManager.Instance.regionController.UpdateAll();
    }
}
