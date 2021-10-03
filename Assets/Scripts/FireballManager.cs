using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballManager : MonoBehaviour
{
    public static FireballManager instance;

    public List<Transform> SpawnPoints;
    public GameObject prefabFireball;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        //SpawnFireballs(new Score(Constants.ElementTypes.Fire, 2));
    }

    public void SpawnFireballs(Score score)
    {
        //print("firing balls: " + score);
        List<Transform> availableSpots = new List<Transform>(SpawnPoints);
        int directionBool = Random.Range(0, 2);
        Vector3 direction = new Vector3(0f, 0f, 180f + 45f);
        if (directionBool == 1)
        {
            direction = new Vector3(0f, 0f, 180f - 45f);
        }
        if (score.Value > 9) score.Value = 9;
        for(int i = score.Value * 2; i >= 0; i--)
        {
            int index = Random.Range(0, availableSpots.Count - 1);
            Transform spawnPoint = availableSpots[index];
            availableSpots.RemoveAt(index);
            GameObject fireGO = Instantiate(prefabFireball, spawnPoint.position, Quaternion.Euler(direction));
            AudioManager.instance.PlaySound(AudioManager.SoundEffects.FireSpawn);
        }
    }
}
