using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakerManager : MonoBehaviour
{
    public List<GameObject> SpawnPoints;
    public GameObject prefabBeaker;

    public float SpawnTimeMax = 3;
    [SerializeField]
    private float Timer;
    [SerializeField]
    private Transform beakerList;

    void Start()
    {
        
    }

    void StartLevel()
    {
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
            GameObject spawnPoint = SpawnPoints[Random.Range(0,SpawnPoints.Count-1)];
            GameObject beakerGO = Instantiate(prefabBeaker, spawnPoint.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), beakerList);
            Beaker b = beakerGO.GetComponent<Beaker>();
            b.rb2d.isKinematic = true;
            b.BeakerListContainer = beakerList;
            beakerGO.transform.localScale = Vector3.zero;
            LeanTween.scale(beakerGO, Vector3.one, 0.5f)
                .setOnComplete(() => b.rb2d.isKinematic = false);
            Timer = SpawnTimeMax;
        }
    }
}
