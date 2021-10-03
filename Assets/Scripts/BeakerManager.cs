using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakerManager : MonoBehaviour
{
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

    void Start()
    {
        StartLevel();
    }

    void StartLevel()
    {
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
            int rand = Random.Range(0, 3);
            if (rand == 2)
            {
                Rain();
            }
            else if(rand == 1)
            {
                Rain();
            }
            else
            {
                Rain();
            }            
            Timer = SpawnTimeMax;
            roundCount++;
            CheckRounds();
        }
    }

    void CheckRounds()
    {
        if(roundCount >= 3)
        {
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
                SpawnTimeMax *= 0.8f;
                break;
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
        for(int i = spawnCount; i >= 0; i--)
        {
            GameObject spawnPoint = possibleSpawnPoints[Random.Range(0, possibleSpawnPoints.Count)];
            possibleSpawnPoints.Remove(spawnPoint);
            GameObject beakerGO = Instantiate(prefabBeaker, spawnPoint.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), beakerList);
            Beaker b = beakerGO.GetComponent<Beaker>();
            b.rb2d.isKinematic = true;
            b.BeakerListContainer = beakerList;
            beakerGO.transform.localScale = Vector3.zero;
            LeanTween.scale(beakerGO, Vector3.one, 0.5f)
                .setOnComplete(() => b.rb2d.isKinematic = false);
        }
        
    }

    void Line()
    {
        switch (difficulty)
        {
            case Difficulty.Tutorial:

                break;
            case Difficulty.VeryEasy:
 
                break;
            case Difficulty.Easy:
  

                break;
            case Difficulty.Moderate:
               

                break;
            case Difficulty.Hard:
               

                break;
            case Difficulty.VeryHard:
               

                break;
        }
    }

    void Cannon()
    {
        switch (difficulty)
        {
            case Difficulty.Tutorial:

                break;
            case Difficulty.VeryEasy:

                break;
            case Difficulty.Easy:


                break;
            case Difficulty.Moderate:


                break;
            case Difficulty.Hard:


                break;
            case Difficulty.VeryHard:


                break;
        }
    }

}
