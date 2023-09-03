using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class RandomizeRegions : MonoBehaviour
{
    public GameObject[] version1;
    public GameObject[] version2;
    public GameObject[] version3;
    public GameObject[] version4;

    private void Awake()
    {
        /*
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }*/

        List<GameObject[]> versions = new List<GameObject[]>();

        if (version1.Length > 0)
        {
            versions.Add(version1);
        }

        if (version2.Length > 0)
        {
            versions.Add(version2);
        }

        if (version3.Length > 0)
        {
            versions.Add(version3);
        }

        if (version4.Length > 0)
        {
            versions.Add(version4);
        }

        if (versions.Count == 0) return;

        int random = UnityEngine.Random.Range(0, versions.Count);

        for (int i = 0; i < versions.Count; i++)
        {
            if (i == random)
            {
                for (int j = 0; j < versions[i].Length; j++)
                {
                    versions[i][j].SetActive(true);
                }
            }
            else
            {
                for (int j = 0; j < versions[i].Length; j++)
                {
                    versions[i][j].SetActive(false);
                }
            }
        }

    }
}
