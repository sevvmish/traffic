using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressPointController : MonoBehaviour
{
    public int CurrentLevel = 0;

    [SerializeField] private GameObject[] fullStars;
    [SerializeField] private GameObject[] emptyStars;
    [SerializeField] private TextMeshPro levelText;
    [SerializeField] private TextMeshPro limitText;

    // Start is called before the first frame update
    void Start()
    {
        levelText.text = CurrentLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
