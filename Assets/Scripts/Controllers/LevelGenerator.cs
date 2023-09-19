using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Desert")]
    [SerializeField] private GameObject AmbSpawnDesert;
    [SerializeField] private GameObject AmbRecDesert;
    [SerializeField] private GameObject TaxiSpawnDesert;
    [SerializeField] private GameObject TaxiRecDesert;
    [SerializeField] private GameObject VanSpawnDesert;
    [SerializeField] private GameObject VanRecDesert;
    [SerializeField] private GameObject Straight1Desert;
    [SerializeField] private GameObject Straight2Desert;
    [SerializeField] private GameObject NonStraight1Desert;
    [SerializeField] private GameObject NonStraight2Desert;
    [SerializeField] private GameObject Double1Desert;

    [Header("forest")]
    [SerializeField] private GameObject AmbSpawnForest;
    [SerializeField] private GameObject AmbRecForest;
    [SerializeField] private GameObject TaxiSpawnForest;
    [SerializeField] private GameObject TaxiRecForest;
    [SerializeField] private GameObject VanSpawnForest;
    [SerializeField] private GameObject VanRecForest;
    [SerializeField] private GameObject Straight1Forest;
    [SerializeField] private GameObject Straight2Forest;
    [SerializeField] private GameObject Straight3Forest;
    [SerializeField] private GameObject Straight4Forest;
    [SerializeField] private GameObject NonStraight1Forest;
    [SerializeField] private GameObject NonStraight2Forest;
    [SerializeField] private GameObject Double1Forest;
    [SerializeField] private GameObject Double2Forest;
    [SerializeField] private GameObject Empty1Forest;
    [SerializeField] private GameObject Empty2Forest;

    [Header("town")]
    [SerializeField] private GameObject AmbSpawnTown;
    [SerializeField] private GameObject AmbRecTown;
    [SerializeField] private GameObject TaxiSpawnTown;
    [SerializeField] private GameObject TaxiRecTown;
    [SerializeField] private GameObject VanSpawnTown;
    [SerializeField] private GameObject VanRecTown;
    [SerializeField] private GameObject Straight1Town;
    [SerializeField] private GameObject Straight2Town;
    [SerializeField] private GameObject NonStraight1Town;
    [SerializeField] private GameObject NonStraight2Town;
    [SerializeField] private GameObject Double1Town;

    [Header("Object spawners")]
    [SerializeField] private GameObject taxiObjectSpawner;
    [SerializeField] private GameObject cargoObjectSpawner;
    [SerializeField] private GameObject injuredObjectSpawner;


    private GameManager gm;
    private int level;

    public void InitLevel(int level, GameManager gm)
    {
        this.gm = gm;
        this.level = level;

        switch (level)
        {
            case 1:
                level_1();
                break;
        }
    }

    private void setDesert()
    {
        gm.GetAmbient().SetData(AmbientType.desert);
    }

    private void setForest()
    {
        gm.GetAmbient().SetData(AmbientType.forest);
    }

    private void setTown()
    {
        gm.GetAmbient().SetData(AmbientType.small_town);
    }

    private void level_1()
    {
        setDesert();
        int chance = UnityEngine.Random.Range(0, 2);

        switch(chance)
        {
            case 0:

                break;
            case 1:

                break;
        }
    }
}

public enum LevelTypes
{
    none,
    desert,
    forest,
    town
}
