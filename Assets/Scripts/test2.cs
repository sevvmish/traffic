using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    private Transform me;
    // Start is called before the first frame update
    void Start()
    {
        me = transform;
        StartCoroutine(play());
    }

    private IEnumerator play()
    {
        while (true)
        {
            for (float i = 0; i <= 1; i += 0.025f)
            {
                me.position = Vector3.Slerp(new Vector3(-5, 0, 0), new Vector3(5, 0, 0), i);
                yield return new WaitForSeconds(0.1f);
            }

            for (float i = 0; i <= 1; i += 0.025f)
            {
                me.position = Vector3.Slerp(new Vector3(5, 0, 0), new Vector3(-5, 0, 0), i);
                yield return new WaitForSeconds(0.1f);
            }
        }

        
    }

}
