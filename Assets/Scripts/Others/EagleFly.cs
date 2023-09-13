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

    // Start is called before the first frame update
    void Start()
    {
        eagle.gameObject.SetActive(false);  
        StartCoroutine(play());        
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
                if (i == (routes.Length - 1))
                {
                    distance = (routes[i].localPosition - routes[0].localPosition).magnitude;
                    pos = routes[0].localPosition;
                    eagle.DOLookAt(routes[0].position, 1);
                }
                else
                {
                    distance = (routes[i].localPosition - routes[i + 1].localPosition).magnitude;
                    pos = routes[i+1].localPosition;
                    eagle.DOLookAt(routes[i+1].position, 1);
                }

                if (!isFirst)
                {
                    isFirst = true;
                    distance = 0;
                }

                eagle.DOLocalMove(pos, distance * speed).SetEase(Ease.Linear);                
                yield return new WaitForSeconds(distance * speed);
            }
        }
    }


}
