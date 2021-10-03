using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakerManager : MonoBehaviour
{
    public static BeakerManager instance;

    public List<GameObject> SpawnPoints;
    public GameObject prefabBeaker;
    public Difficulty difficulty;
    public float SpawnTimeMax = 3;
    [SerializeField]
    private float Timer;
    [SerializeField]
    private Transform beakerList;

    public float roundCount;
    public int LevelNumber = 1;

    public List<Transform> BatchSpawnPoints1;
    public List<Transform> BatchSpawnPoints2;
    public List<Transform> BatchSpawnPoints3;
    public Transform CannonLeftSpawnPoint;
    public Transform CannonRightSpawnPoint;

    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartLevel();
    }

    void StartLevel()
    {
        roundCount = 2;
        difficulty = Difficulty.Tutorial;
        HUDManager.instance.UpdateLevel(LevelNumber);
        Timer = SpawnTimeMax;
    }
    
    void Update()
    {
        if(Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            int rand = Random.Range(0, 4);
            if (rand == 3)
            {
                Line();
            }
            else if (rand == 2)
            {
                Rain();
            }
            else if(rand == 1)
            {
                Cannon();
            }
            else
            {
                Batch();
            }            
            Timer = SpawnTimeMax;
            roundCount++;
            CheckRounds();
        }
    }

    void CheckRounds()
    {
        if(roundCount >= 5)
        {
            roundCount = 0;
            LevelNumber++;
            HUDManager.instance.UpdateLevel(LevelNumber);
            IncrementDifficulty();
        }
    }

    void IncrementDifficulty()
    {
        switch (difficulty)
        {
            case Difficulty.Tutorial:
                difficulty = Difficulty.VeryEasy;
                break;
            case Difficulty.VeryEasy:
                difficulty = Difficulty.Easy;
                break;
            case Difficulty.Easy:
                difficulty = Difficulty.Moderate;
                break;
            case Difficulty.Moderate:
                difficulty = Difficulty.Hard;
                break;
            case Difficulty.Hard:
                difficulty = Difficulty.VeryHard;
                break;
            case Difficulty.VeryHard:
                if(SpawnTimeMax <= 1.2f)
                {
                    SpawnTimeMax = 1.2f;
                }
                else
                {
                    SpawnTimeMax *= 0.9f;
                }
                
                break;
        }
    }

    void Line()
    {
        int spawnCount = 0;
        List<GameObject> possibleSpawnPoints = new List<GameObject>(SpawnPoints);
        switch (difficulty)
        {
            case Difficulty.Tutorial:
                return;
            case Difficulty.VeryEasy:
                spawnCount = 1;
                break;
            case Difficulty.Easy:
                spawnCount = 2;
                break;
            case Difficulty.Moderate:
                spawnCount = 3;
                break;
            case Difficulty.Hard:
                spawnCount = 4;
                break;
            case Difficulty.VeryHard:
                spawnCount = 5;
                break;
        }

        GameObject spawnPoint = possibleSpawnPoints[Random.Range(0, possibleSpawnPoints.Count)];
        for (int i = spawnCount; i >= 0; i--)
        {
            float delayTime = Random.Range(0f, 0.5f);
            possibleSpawnPoints.Remove(spawnPoint);
            LeanTween.delayedCall(delayTime, () =>
            {
                GameObject beakerGO = Instantiate(prefabBeaker, spawnPoint.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), beakerList);
                Beaker b = beakerGO.GetComponent<Beaker>();
                b.rb2d.isKinematic = true;
                b.BeakerListContainer = beakerList;
                beakerGO.transform.localScale = Vector3.zero;
                LeanTween.scale(beakerGO, Vector3.one, 0.5f)
                    .setOnComplete(() => b.rb2d.isKinematic = false);
            });
        }
    }

    void Rain()
    {
        int spawnCount = 0;
        List<GameObject> possibleSpawnPoints = new List<GameObject>(SpawnPoints);
        switch (difficulty)
        {
            case Difficulty.Tutorial:
                return;
            case Difficulty.VeryEasy:
                spawnCount = 1;
                break;
            case Difficulty.Easy:
                spawnCount = 2;
                break;
            case Difficulty.Moderate:
                spawnCount = 3;
                break;
            case Difficulty.Hard:
                spawnCount = 4;
                break;
            case Difficulty.VeryHard:
                spawnCount = 5;
                break;
        }

        
        for (int i = spawnCount; i >= 0; i--)
        {
            GameObject spawnPoint = possibleSpawnPoints[Random.Range(0, possibleSpawnPoints.Count)];
            float delayTime = Random.Range(0f, 0.5f);
            possibleSpawnPoints.Remove(spawnPoint);
            LeanTween.delayedCall(delayTime, () =>
            {
                GameObject beakerGO = Instantiate(prefabBeaker, spawnPoint.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), beakerList);
                Beaker b = beakerGO.GetComponent<Beaker>();
                b.rb2d.isKinematic = true;
                b.BeakerListContainer = beakerList;
                beakerGO.transform.localScale = Vector3.zero;
                LeanTween.scale(beakerGO, Vector3.one, 0.5f)
                    .setOnComplete(() => b.rb2d.isKinematic = false);
            });
        }
        
    }

    void Batch()
    {
        int batchSpawnCount = 0;
        switch (difficulty)
        {
            case Difficulty.Tutorial:
                return;
            case Difficulty.VeryEasy:
                batchSpawnCount = 1;
                break;
            case Difficulty.Easy:
                batchSpawnCount = 1;
                break;
            case Difficulty.Moderate:
                batchSpawnCount = 2;
                break;
            case Difficulty.Hard:
                batchSpawnCount = 3;
                break;
            case Difficulty.VeryHard:
                batchSpawnCount = 3;
                break;
        }

        float delay = 0f;
        List<Transform> spawnList;
        for(int i = 0; i < batchSpawnCount; i++)
        {
            int spawnNum = Random.Range(1, 4);
            switch (spawnNum)
            {
                case 3:
                    spawnList = new List<Transform>(BatchSpawnPoints3);
                    break;
                case 2:
                    spawnList = new List<Transform>(BatchSpawnPoints2);
                    break;
                default:
                    spawnList = new List<Transform>(BatchSpawnPoints1);
                    break;
            }

            LeanTween.delayedCall(delay,() => BatchSpawn(spawnList));          
            delay += 0.4f;
        }
    }

    void BatchSpawn(List<Transform> spawnList)
    {
        for (int j = 0; j < spawnList.Count; j++)
        {
            GameObject beakerGO = Instantiate(prefabBeaker, spawnList[j].transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), beakerList);
            Beaker b = beakerGO.GetComponent<Beaker>();
            b.rb2d.isKinematic = true;
            b.BeakerListContainer = beakerList;
            beakerGO.transform.localScale = Vector3.zero;
            LeanTween.scale(beakerGO, Vector3.one, 0.5f)
                .setOnComplete(() => b.rb2d.isKinematic = false);
        }
    }

    void Cannon()
    {
        int shotCount = 0;
        switch (difficulty)
        {
            case Difficulty.Tutorial:

                return;
            case Difficulty.VeryEasy:
                shotCount = 1;
                break;
            case Difficulty.Easy:
                shotCount = 2;
                break;
            case Difficulty.Moderate:
                shotCount = 3;
                break;
            case Difficulty.Hard:
                shotCount = 4;
                break;
            case Difficulty.VeryHard:
                shotCount = 5;
                break;
        }

        float delay = 0f;
        for(int i = 0; i < shotCount; i++)
        {
            int side = Random.Range(0, 2);
            if(side == 0)
            {
                //Left Side
                LeanTween.delayedCall(delay, CannonLeft);
            }
            else
            {
                //Right Side
                LeanTween.delayedCall(delay, CannonRight);
            }
            delay += 0.5f;
        }
    }

    void CannonRight()
    {
        GameObject beakerGO = Instantiate(prefabBeaker, CannonRightSpawnPoint.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), beakerList);
        Beaker b = beakerGO.GetComponent<Beaker>();
        b.rb2d.isKinematic = true;
        b.BeakerListContainer = beakerList;
        beakerGO.transform.localScale = Vector3.zero;
        LeanTween.scale(beakerGO, Vector3.one, 0.1f)
            .setOnComplete(() =>
            {
                b.rb2d.isKinematic = false;
                b.rb2d.AddForce((Vector2.up - Vector2.right) * 40f, ForceMode2D.Impulse);
            });
    }

    void CannonLeft()
    {
        GameObject beakerGO = Instantiate(prefabBeaker, CannonLeftSpawnPoint.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), beakerList);
        Beaker b = beakerGO.GetComponent<Beaker>();
        b.rb2d.isKinematic = true;
        b.BeakerListContainer = beakerList;
        beakerGO.transform.localScale = Vector3.zero;
        LeanTween.scale(beakerGO, Vector3.one, 0.1f)
            .setOnComplete(() =>
            {
                b.rb2d.isKinematic = false;
                b.rb2d.AddForce((Vector2.up + Vector2.right) * 40f, ForceMode2D.Impulse);
            });
    }

}
