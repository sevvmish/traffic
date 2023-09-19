using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleFly : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform eagle;
    [SerializeField] private Transform[] routes;
    private bool isFirst;

    private AudioSource _audio;
    private float cooldown;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        eagle.gameObject.SetActive(false);  
        StartCoroutine(play());
        cooldown = UnityEngine.Random.Range(7f, 15f);
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_timer > cooldown)
        {
            _timer = 0;

            int chance = UnityEngine.Random.Range(0, 100);

            if (chance > 50) 
            {
                _audio.Play();
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private IEnumerator play()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0,3f));
        eagle.gameObject.SetActive(true);

        eagle.localPosition = routes[0].localPosition;
        eagle.DOLookAt(routes[1].position, 0);

        while (true)
        {
            for (int i = 0; i < routes.Length; i++)
            {
                float distance = 0;
                Vector3 pos = Vector3.zero;
                Vector3 lookAtPos = Vector3.zero;
                if (i == (routes.Length - 1))
                {
                    distance = (routes[i].localPosition - routes[0].localPosition).magnitude;
                    pos = routes[0].localPosition;
                    eagle.DOLookAt(routes[0].position, 1);
                    lookAtPos = routes[0].position;
                }
                else
                {
                    distance = (routes[i].localPosition - routes[i + 1].localPosition).magnitude;
                    pos = routes[i+1].localPosition;
                    eagle.DOLookAt(routes[i+1].position, 1);
                    lookAtPos = routes[i + 1].position;
                }

                if (!isFirst)
                {
                    isFirst = true;
                    distance = 0;
                }

                eagle.DOLocalMove(pos, distance * speed).SetEase(Ease.Linear);
                float koeff = distance * speed/5;

                for (int j = 0; j < 5; j++)
                {
                    eagle.DOLookAt(lookAtPos, 1f);
                    yield return new WaitForSeconds(koeff);
                }
                
            }
        }
    }


}
