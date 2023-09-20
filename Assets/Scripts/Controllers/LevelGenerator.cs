using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
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
    private Transform partsLocation;

    public void InitLevel(int level, GameManager gm)
    {
        partsLocation = GameObject.Find("PARTS").transform;
        this.gm = gm;
        this.level = level;
        resetGameManager();

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
        gm.VanCount = 3;
        gm.GameTime = 120;
        partsAngle(15);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(VanSpawnDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 60, 0), "", 11f, 4f);
        SetObject(VanRecDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, -120, 0));
        SetObject(Straight1Desert, Vector3.zero, new Vector3(0, -60, 0), "tutorial_try1", false);
        SetObject(Straight1Desert, Vector3.zero, new Vector3(0, 0, 0), "tutorial_try2", true);
    }


    private void SetObject(GameObject obj, Vector3 pos, Vector3 rot)
    {
        SetObject(obj, pos, rot, "", true);
    }
    private void SetObject(GameObject obj, Vector3 pos, Vector3 rot, string name, bool isRotating)
    {
        GameObject g = Instantiate(obj, partsLocation);
        g.transform.localPosition = pos;
        g.transform.localEulerAngles = rot;
        g.name = name;

        if (g.TryGetComponent(out Region r) && !isRotating)
        {
            r.RotationAngle = 0;
        }
    }

    private void SetObject_Spawner(GameObject obj, Vector3 pos, Vector3 rot, string name, float frequency, float delay)
    {
        GameObject g = Instantiate(obj, partsLocation);
        g.transform.localPosition = pos;
        g.transform.localEulerAngles = rot;
        g.name = name;
        g.GetComponent<VehicleSpawner>().SpawnFrequency = frequency;
        g.GetComponent<VehicleSpawner>().Delay = delay;
    }

    private void partsAngle(float angle) => partsLocation.eulerAngles = new Vector3(0, angle, 0);

    private void resetGameManager()
    {
        gm.TaxiCount = 0;
        gm.TaxiManCount = 0;
        gm.VanCount = 0;
        gm.VanCargoCount = 0;
        gm.AmbulanceCount = 0;
        gm.AmbulanceManCount = 0;
    }
}

public enum LevelTypes
{
    none,
    desert,
    forest,
    town
}
