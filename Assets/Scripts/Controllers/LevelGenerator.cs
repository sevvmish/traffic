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

            case 2:
                level_2();
                break;

            case 3:
                level_3();
                break;

            case 4:
                level_4();
                break;

            case 5:
                level_5();
                break;

            case 6:
                level_6();
                break;

            case 7:
                level_7();
                break;

            case 8:
                level_8();
                break;

            case 9:
                level_9();
                break;

            case 10:
                level_10();
                break;

            case 11:
                level_11();
                break;

            case 12:
                level_12();
                break;

            case 13:
                level_13();
                break;

            case 14:
                level_14();
                break;

            case 15:
                level_15();
                break;

            case 16:
                level_16();
                break;

            case 17:
                level_17();
                break;

            case 18:
                level_18();
                break;

            case 19:
                level_19();
                break;

            case 20:
                level_20();
                break;

            case 21:
                level_21();
                break;

            case 22:
                level_22();
                break;

            case 23:
                level_23();
                break;

            case 24:
                level_24();
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
        gm.CameraZShift = -9;
        partsAngle(15);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(VanSpawnDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 60, 0), "", 11f, 4f, 0);
        SetObject(VanRecDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, -120, 0), false);
        SetObject(Straight1Desert, Vector3.zero, new Vector3(0, -60, 0), "tutorial_try1", false);
        SetObject(Straight1Desert, Vector3.zero, new Vector3(0, 0, 0), "tutorial_try2", true);
    }

    private void level_2()
    {
        setDesert();
        gm.VanCount = 3;
        gm.GameTime = 40;
        gm.StarsLimitTimer = 0.25f;
        gm.StarsLimitMistakes = 0;
        gm.StarsLimitAccidents = 0;
        gm.CameraZShift = -9;
        partsAngle(-5);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(VanSpawnDesert, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 120, 0), "", 6f, 1f, 0);
        SetObject(VanRecDesert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, -60, 0), false);
        SetObject(NonStraight1Desert, Vector3.zero, new Vector3(0, 60, 0), true);
    }

    private void level_3()
    {
        setDesert();
        gm.VanCount = 2;
        gm.GameTime = 30;
        gm.StarsLimitTimer = 0.45f;
        gm.StarsLimitMistakes = 0;
        gm.StarsLimitAccidents = 0;
        gm.CameraZShift = -9;
        partsAngle(5);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(VanSpawnDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, 240, 0), "", 7f, 3f, 0);
        SetObject(VanRecDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 60, 0), false);
        SetObject(NonStraight2Desert, Vector3.zero, new Vector3(0, 60, 0), true);
    }

    private void level_4()
    {
        setDesert();
        gm.VanCount = 2;
        gm.GameTime = 30;
        gm.StarsLimitTimer = 0.42f;
        gm.StarsLimitMistakes = 0;
        gm.StarsLimitAccidents = 0;
        gm.CameraZShift = -10.5f;
        partsAngle(5);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(VanSpawnDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 60, 0), "", 5f, 3f, 0);
        SetObject(VanRecDesert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, -60, 0), true);
        SetObject(NonStraight2Desert, Vector3.zero, new Vector3(0, 120, 0), true);
    }

    private void level_5()
    {
        setDesert();
        gm.VanCount = 5;
        gm.GameTime = 30;
        gm.StarsLimitTimer = 0.2f;
        gm.StarsLimitMistakes = 0;
        gm.StarsLimitAccidents = 0;
        gm.CameraZShift = -8;
        partsAngle(0);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(VanSpawnDesert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, 300, 0), "", 3f, 3f, 0);
        SetObject_Spawner(VanSpawnDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 60, 0), "", 3f, 3f, 0);
        SetObject(VanRecDesert, new Vector3(0, 0, 9.5f), new Vector3(0, 180, 0), false);
        SetObject(Double1Desert, Vector3.zero, new Vector3(0, 0, 0), true);
    }

    private void level_6()
    {
        setDesert();
        gm.VanCount = 5;
        gm.GameTime = 30;
        gm.StarsLimitTimer = 0.25f;
        gm.StarsLimitMistakes = 0;
        gm.StarsLimitAccidents = 0;
        gm.CameraZShift = -8;
        partsAngle(0);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(VanSpawnDesert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, 300, 0), "", 4f, 2f, 0);
        SetObject_Spawner(VanSpawnDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 60, 0), "", 4f, 4f, 0);        
        SetObject(VanRecDesert, new Vector3(0, 0, 9.5f), new Vector3(0, 180, 0), false);
        SetObject(Straight2Desert, Vector3.zero, new Vector3(0, 0, 0), "main1", true);
    }

    private void level_7()
    {
        setDesert();
        gm.TaxiCount = 3;
        gm.GameTime = 30;
        gm.StarsLimitTimer = 0.2f;
        gm.StarsLimitMistakes = 0;
        gm.StarsLimitAccidents = 0;
        gm.CameraZShift = -9;
        partsAngle(10);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(TaxiSpawnDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 60, 0), "", 4f, 3f, 0);
        SetObject(TaxiRecDesert, new Vector3(16.5f, 0, 9.5f), new Vector3(0, 120, 0), false);
        SetObject(NonStraight1Desert, Vector3.zero, new Vector3(0, 0, 0), true);
        SetObject(Straight2Desert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, 0, 0), true);
    }

    private void level_8()
    {
        setDesert();
        gm.TaxiCount = 3;
        gm.GameTime = 30;
        gm.StarsLimitTimer = 0.2f;
        gm.StarsLimitMistakes = 0;
        gm.StarsLimitAccidents = 0;
        gm.CameraZShift = -11;
        partsAngle(10);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(TaxiSpawnDesert, new Vector3(16.5f, 0, 9.5f), new Vector3(0, -120, 0), "", 4f, 3f, 0);
        SetObject(TaxiRecDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 300, 0), false);
        SetObject(NonStraight1Desert, Vector3.zero, new Vector3(0, 0, 0), true);
        SetObject(NonStraight1Desert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, 180, 0), true);
        SetObject(NonStraight2Desert, new Vector3(0, 0, 9.5f), new Vector3(0, 180, 0), true);
    }

    private void level_9()
    {
        setDesert();
        gm.TaxiCount = 5;
        gm.GameTime = 40;
        gm.StarsLimitTimer = 0.3f;
        gm.StarsLimitMistakes = 0;
        gm.StarsLimitAccidents = 0;
        gm.CameraZShift = -8;
        partsAngle(-8);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(TaxiSpawnDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, -120, 0), "", 6f, 3f, 0);
        SetObject_Spawner(TaxiSpawnDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 60, 0), "", 6f, 5f, 0);
        SetObject(TaxiRecDesert, new Vector3(16.5f, 0, 0), new Vector3(0, 120, 0), false);
        SetObject(NonStraight1Desert, Vector3.zero, new Vector3(0, 0, 0), "main2", true);
        SetObject(NonStraight1Desert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, 180, 0), "main1", true);
    }

    private void level_10()
    {
        setDesert();
        gm.TaxiCount = 5;
        gm.GameTime = 45;
        gm.StarsLimitTimer = 0.33f;
        gm.StarsLimitMistakes = 0;
        gm.StarsLimitAccidents = 0;
        gm.CameraZShift = -9;
        partsAngle(65);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(TaxiSpawnDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, -120, 0), "", 6f, 2f, 0);
        SetObject_Spawner(TaxiSpawnDesert, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 120, 0), "", 6f, 6f, 0);
        SetObject(TaxiRecDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 0, 0), false);
        SetObject(TaxiRecDesert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, 120, 0), false);
        SetObject(NonStraight2Desert, Vector3.zero, new Vector3(0, 60, 0), "main0", true);
        SetObject(Double1Desert, new Vector3(0, 0, -9.5f), new Vector3(0, 0, 0), "main1", true);
    }

    private void level_11()
    {
        setDesert();
        gm.TaxiCount = 1;
        gm.VanCount = 1;
        gm.GameTime = 50;
        gm.StarsLimitTimer = 0.2f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -9;
        partsAngle(50);
        gm.SetScreenFOV(LevelScale.small);

        SetObject(Straight1Desert, Vector3.zero, new Vector3(0, -60, 0), "tutorial_try1", false);
        SetObject(NonStraight2Desert, Vector3.zero, new Vector3(0, 60, 0), "tutorial_try2", true);


        SetObject_Spawner(TaxiSpawnDesert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 60, 0), "", 6f, 3f, 0);
        SetObject_Spawner(VanSpawnDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, -120, 0), "", 6f, 3f, 0);
        
        SetObject(VanRecDesert, new Vector3(0, 0, 9.5f), new Vector3(0, -180, 0), false);
        SetObject(TaxiRecDesert, new Vector3(0, 0, -9.5f), new Vector3(0, -120, 0), false);

        //SetObject(NonStraight2Desert, Vector3.zero, new Vector3(0, 60, 0));
    }

    private void level_12()
    {
        setDesert();
        gm.TaxiCount = 3;
        gm.VanCount = 3;
        gm.GameTime = 40;
        gm.StarsLimitTimer = 0.3f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -8;
        partsAngle(-50);
        gm.SetScreenFOV(LevelScale.small);
        
        SetObject_Spawner(TaxiSpawnDesert, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 120, 0), "", 6f, 4f, 0);
        SetObject_Spawner(VanSpawnDesert, new Vector3(0f, 0, 9.5f), new Vector3(0, -180, 0), "", 6f, 4f, 0);

        SetObject(TaxiRecDesert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, -180, 0), false);
        SetObject(VanRecDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, -120, 0), false);

        SetObject(NonStraight2Desert, Vector3.zero, new Vector3(0, -60, 0), "main0", true);
    }

    private void level_13()
    {
        setDesert();
        gm.TaxiCount = 3;
        gm.VanCount = 3;
        gm.GameTime = 40;
        gm.StarsLimitTimer = 0.4f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -9;
        partsAngle(120);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(TaxiSpawnDesert, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 120, 0), "", 6f, 4f, 0);
        SetObject_Spawner(VanSpawnDesert, new Vector3(0f, 0, 9.5f), new Vector3(0, -180, 0), "", 6f, 4f, 0);

        SetObject(TaxiRecDesert, new Vector3(0, 0, -9.5f), new Vector3(0, -120, 0), false);
        SetObject(VanRecDesert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, -60, 0), false);

        SetObject(Double1Desert, Vector3.zero, new Vector3(0, 60, 0), true);
    }

    private void level_14()
    {
        setDesert();
        gm.TaxiCount = 3;
        gm.GameTime = 40;
        gm.StarsLimitTimer = 0.32f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -9;
        partsAngle(-90);
        gm.SetScreenFOV(LevelScale.small);

        SetObject_Spawner(TaxiSpawnDesert, new Vector3(0, 0, 9.5f), new Vector3(0, 180, 0), "", 6f, 5f, 0);
        SetObject_Spawner(VanSpawnDesert, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 120, 0), "", 6f, 3f, 0);
        SetObject_Spawner(AmbSpawnDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, -120, 0), "", 6f, 4f, 0);

        SetObject(TaxiRecDesert, new Vector3(0, 0, -9.5f), new Vector3(0, -120, 0), false);

        SetObject(NonStraight2Desert, Vector3.zero, new Vector3(0, -60, 0), true);
    }

    private void level_15()
    {
        setDesert();
        gm.TaxiCount = 3;
        gm.GameTime = 40;
        gm.StarsLimitTimer = 0.30f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -6;
        partsAngle(-30);
        gm.SetScreenFOV(LevelScale.medium);

        SetObject_Spawner(TaxiSpawnDesert, new Vector3(- 8.25f, 0, 4.75f), new Vector3(0, 120, 0), "", 6f, 3f, 0);
        SetObject_Spawner(VanSpawnDesert, new Vector3(0f, 0, 9.5f), new Vector3(0, 180, 0), "", 6f, 3f, 0);
        SetObject_Spawner(AmbSpawnDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, -180, 0), "", 6f, 7f, 0);
        SetObject_Spawner(AmbSpawnDesert, new Vector3(16.5f, 0, 0f), new Vector3(0, -120, 0), "", 6f, 7f, 0);

        SetObject(TaxiRecDesert, new Vector3(16.5f, 0, -9.5f), new Vector3(0, 180, 0), false);

        SetObject(Double1Desert, new Vector3(0, 0, 0), new Vector3(0, -60, 0), "main0", true);
        SetObject(Double1Desert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, 0, 0), "main1", true);
    }

    private void level_16()
    {
        setForest();
        gm.VanCount = 2;
        gm.AmbulanceCount = 2;
        gm.GameTime = 50;
        gm.StarsLimitTimer = 0.30f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -10;
        partsAngle(-30);
        gm.SetScreenFOV(LevelScale.medium);
        
        SetObject_Spawner(VanSpawnForest, new Vector3(-8.25f, 0, 14.25f), new Vector3(0, 180, 0), "", 6f, 4f, 0);
        SetObject_Spawner(AmbSpawnForest, new Vector3(0, 0, -9.5f), new Vector3(0, 0, 0), "", 6f, 6f, 0);

        SetObject(VanRecForest, new Vector3(8.25f, 0, -4.75f), new Vector3(0, -60, 0), false);
        SetObject(AmbRecForest, new Vector3(-16.5f, 0, 9.5f), new Vector3(0, 0, 0), false);

        SetObject(NonStraight2Forest, new Vector3(0, 0, 9.5f), new Vector3(0, -180, 0), "main0", true);
        SetObject(Double1Forest, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 0, 0), "main1", true);
        SetObject(Double2Forest, new Vector3(0, 0, 0), new Vector3(0, 0, 0), "main2", true);
    }

    private void level_17()
    {
        setDesert();
        gm.TaxiCount = 2;
        gm.VanCount = 2;
        gm.GameTime = 45;
        gm.StarsLimitTimer = 0.25f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -10;
        partsAngle(150);
        gm.SetScreenFOV(LevelScale.medium);

        SetObject_Spawner(TaxiSpawnDesert, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 120, 0), "", 6f, 3f, 0);
        SetObject_Spawner(VanSpawnDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, 240, 0), "", 6f, 3f, 0);

        SetObject(TaxiRecDesert, new Vector3(16.5f, 0, 0), new Vector3(0, 120, 0), false);
        SetObject(VanRecDesert, new Vector3(0, 0, -9.5f), new Vector3(0, 60, 0), false);

        SetObject(NonStraight2Desert, Vector3.zero, new Vector3(0, 0, 0), true);
        SetObject(Double1Desert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, 0, 0), true);
    }

    private void level_18()
    {
        setDesert();
        gm.TaxiCount = 2;
        gm.VanCount = 2;
        gm.GameTime = 40;
        gm.StarsLimitTimer = 0.18f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -9;
        partsAngle(0);
        gm.SetScreenFOV(LevelScale.medium);

        SetObject_Spawner(TaxiSpawnDesert, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 120, 0), "", 7f, 2f, 0);
        SetObject_Spawner(VanSpawnDesert, new Vector3(8.25f, 0, 4.75f), new Vector3(0, 240, 0), "", 7f, 6f, 0);
        
        SetObject(TaxiRecDesert, new Vector3(16.5f, 0, 0), new Vector3(0, 120, 0), false);
        SetObject(VanRecDesert, new Vector3(0, 0, -9.5f), new Vector3(0, 60, 0), false);

        SetObject(Double1Desert, Vector3.zero, new Vector3(0, 0, 0), true);
        SetObject(Double1Desert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, 0, 0), true);
    }

    private void level_19()
    {
        setForest();        
        gm.VanCount = 2;
        gm.GameTime = 40;
        gm.StarsLimitTimer = 0.25f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -8f;
        gm.CameraXAngle = 65f;
        partsAngle(-150);
        gm.SetScreenFOV(LevelScale.medium);
                
        SetObject_Spawner(VanSpawnForest, new Vector3(8.25f, 0, 14.25f), new Vector3(0, 240, 0), "", 6f, 3f, 0);
        SetObject(VanRecForest, new Vector3(-16.5f, 0, -9.5f), new Vector3(0, 60, 0), false);

        SetObject_Spawner(TaxiSpawnForest, new Vector3(8.25f, 0, 4.75f), new Vector3(0, -60, 0), "", 6f, 3f, 0);
        SetObject_Spawner(TaxiSpawnForest, new Vector3(0, 0, -9.5f), new Vector3(0, 0, 0), "", 6f, 0, 0);
        SetObject_Spawner(TaxiSpawnForest, new Vector3(-16.5f, 0, 0), new Vector3(0, 60, 0), "", 6f, 0, 0);

        SetObject(Double2Forest, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 0, 0), true);
        SetObject(Double2Forest, new Vector3(0, 0, 0), new Vector3(0, 0, 0), true);
        SetObject(NonStraight1Forest, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 0, 0), true);
        SetObject(NonStraight2Forest, new Vector3(0, 0, 9.5f), new Vector3(0, -120, 0), true);
    }

    private void level_20()
    {
        setDesert();
        gm.TaxiCount = 2;        
        gm.GameTime = 45;
        gm.StarsLimitTimer = 0.25f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -9;
        gm.CameraXAngle = 65f;
        partsAngle(0);
        gm.SetScreenFOV(LevelScale.medium);
        
        SetObject_Spawner(TaxiSpawnDesert, new Vector3(-16.5f, 0, 0f), new Vector3(0, 120, 0), "", 7f, 2f, 0);
        SetObject(TaxiRecDesert, new Vector3(16.5f, 0, -9.5f), new Vector3(0, 180, 0), false);
        SetObject(TaxiRecDesert, new Vector3(16.5f, 0, 0), new Vector3(0, 120, 0), false);

        SetObject_Spawner(VanSpawnDesert, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 120, 0), "", 7f, 6f, 0);
        SetObject_Spawner(AmbSpawnDesert, new Vector3(-16.5f, 0, -9.5f), new Vector3(0, -300, 0), "", 5f, 0f, 0);
        SetObject_Spawner(AmbSpawnDesert, new Vector3(-8.25f, 0, -14.25f), new Vector3(0, 60, 0), "", 5f, 0f, 0);

        SetObject(Double1Desert, new Vector3(0, 0, 0f), new Vector3(0, 60, 0), true);
        SetObject(Double1Desert, new Vector3(8.25f, 0, -4.75f), new Vector3(0, 0, 0), true);
        SetObject(Double1Desert, new Vector3(0, 0, -9.5f), new Vector3(0, 0, 0), true);
        SetObject(Double1Desert, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 0, 0), true);
    }

    private void level_21()
    {
        setForest();
        gm.AmbulanceCount = 2;
        gm.GameTime = 50;
        gm.StarsLimitTimer = 0.21f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -8f;
        gm.CameraXAngle = 65f;
        partsAngle(90);
        gm.SetScreenFOV(LevelScale.large);

        
        SetObject_Spawner(AmbSpawnForest, new Vector3(0f, 0, 9.5f), new Vector3(0, 240, 0), "", 5f, 2f, 0);
        SetObject(AmbRecForest, new Vector3(0, 0, 0), new Vector3(0, 60, 0), false);

        SetObject_Spawner(TaxiSpawnForest, new Vector3(0, 0, -19), new Vector3(0, 0, 0), "", 6f, 3f, 0);
        SetObject_Spawner(VanSpawnForest, new Vector3(-8.25f, 0, -14.25f), new Vector3(0, 0, 0), "", 7f, 3, 0);
        
        SetObject(Straight4Forest, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 0, 0), true);
        SetObject(Double2Forest, new Vector3(0, 0, -9.5f), new Vector3(0, 0, 0), true);
        SetObject(Double2Forest, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 0, 0), true);        
    }

    private void level_22()
    {
        setForest();
        gm.VanCount = 2;
        gm.TaxiCount = 2;
        gm.GameTime = 60;
        gm.StarsLimitTimer = 0.2f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -5.5f;
        gm.CameraXAngle = 65f;
        partsAngle(-90);
        gm.SetScreenFOV(LevelScale.medium);


        SetObject(TaxiRecForest, new Vector3(0, 0, 19), new Vector3(0, 60, 0), false);
        SetObject(VanRecForest, new Vector3(8.25f, 0, -4.75f), new Vector3(0, 240, 0), false);

        SetObject_Spawner(TaxiSpawnForest, new Vector3(0, 0, -19), new Vector3(0, 0, 0), "", 5, 0, 0);
        SetObject_Spawner(VanSpawnForest, new Vector3(8.25f, 0, 4.75f), new Vector3(0, -60, 0), "", 5, 0, 0);

        SetObject(Straight1Forest, new Vector3(0, 0, 0), new Vector3(0, 60, 0), true);
        SetObject(Double2Forest, new Vector3(0, 0, -9.5f), new Vector3(0, 0, 0), true);
        SetObject(Double2Forest, new Vector3(0, 0, 9.5f), new Vector3(0, 0, 0), true);
    }

    private void level_23()
    {
        setForest();
        gm.VanCount = 2;
        gm.AmbulanceCount = 2;
        gm.GameTime = 60;
        gm.StarsLimitTimer = 0.2f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -7.8f;
        gm.CameraXAngle = 65f;
        partsAngle(-90);
        gm.SetScreenFOV(LevelScale.large);

        
        SetObject(AmbRecForest, new Vector3(0, 0, 19), new Vector3(0, 60, 0), false);
        SetObject(VanRecForest, new Vector3(0, 0, -19f), new Vector3(0, 0, 0), false);

        SetObject_Spawner(AmbSpawnForest, new Vector3(-8.25f, 0, -14.25f), new Vector3(0, 0, 0), "", 6, 5, 0);
        SetObject_Spawner(VanSpawnForest, new Vector3(-8.25f, 0, 14.25f), new Vector3(0, -180, 0), "", 6, 2, 0);
                
        SetObject(Double2Forest, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 0, 0), true);
        SetObject(Double2Forest, new Vector3(-8.25f, 0, -4.75f), new Vector3(0, 0, 0), true);
        SetObject(NonStraight2Forest, new Vector3(0, 0, 0f), new Vector3(0, 0, 0), true);
        SetObject(Straight4Forest, new Vector3(0, 0, -9.50f), new Vector3(0, 0, 0), true);
        SetObject(Straight4Forest, new Vector3(0, 0, 9.50f), new Vector3(0, 0, 0), true);
    }

    private void level_24()
    {
        setForest();
        gm.VanCount = 2;
        gm.AmbulanceCount = 2;
        gm.GameTime = 65;
        gm.StarsLimitTimer = 0.2f;
        gm.StarsLimitMistakes = 1;
        gm.StarsLimitAccidents = 1;
        gm.CameraZShift = -8f;
        gm.CameraXAngle = 65f;
        partsAngle(-30);
        gm.SetScreenFOV(LevelScale.large);

        
        SetObject(AmbRecForest, new Vector3(0, 0, 9.5f), new Vector3(0, 120, 0), false);
        SetObject(VanRecForest, new Vector3(0, 0, -9.5f), new Vector3(0, 60, 0), false);

        SetObject_Spawner(AmbSpawnForest, new Vector3(16.5f, 0, 0), new Vector3(0, -120, 0), "", 4, 0, 0);
        SetObject_Spawner(VanSpawnForest, new Vector3(-16.5f, 0, 0), new Vector3(0, -300, 0), "", 4, 0, 0);

        SetObject(Double2Forest, new Vector3(-8.25f, 0, 4.75f), new Vector3(0, 60, 0), true);
        SetObject(Double2Forest, new Vector3(8.25f, 0, -4.75f), new Vector3(0, 60, 0), true);
        SetObject(NonStraight2Forest, new Vector3(0, 0, 0f), new Vector3(0, 0, 0), true);
    }


    private void SetObject(GameObject obj, Vector3 pos, Vector3 rot, bool isRotating)
    {
        SetObject(obj, pos, rot, "", isRotating);
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
        else if (g.TryGetComponent(out VehicleReceiver vr))
        {
            if (isRotating)
            {
                vr.Rotator.SetRotationAngle(60);
            }
            else
            {
                vr.Rotator.SetRotationAngle(0);
            }
        }
    }

    private void SetObject_Spawner(GameObject obj, Vector3 pos, Vector3 rot, string name, float frequency, float delay, int limit)
    {
        SetObject_Spawner(obj, pos, rot, name, frequency, delay, limit, false);
    }

    private void SetObject_Spawner(GameObject obj, Vector3 pos, Vector3 rot, string name, float frequency, float delay, int limit, bool isRotating)
    {
        GameObject g = Instantiate(obj, partsLocation);
        g.transform.localPosition = pos;
        g.transform.localEulerAngles = rot;
        g.name = name;
        g.GetComponent<VehicleSpawner>().SpawnFrequency = frequency;
        g.GetComponent<VehicleSpawner>().Delay = delay;
        g.GetComponent<VehicleSpawner>().Limit = limit;

        if (isRotating)
        {
            g.GetComponent<VehicleSpawner>().Rotator.SetRotationAngle(60);
        }
        else
        {
            g.GetComponent<VehicleSpawner>().Rotator.SetRotationAngle(0);
        }

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
