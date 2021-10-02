using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Controls ctrl;

    public GameObject prefabBoulder;
    public Transform boulderSpawnPoint;
    public Transform MiscHolder;

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
        ctrl = new Controls();
        
    }

    public void TriggerReaction(Score score)
    {
        switch (score.Element)
        {
            case ElementTypes.Earth:

                for(int i = 0; i <= score.Value; i++)
                {
                    GameObject bGO = Instantiate(prefabBoulder, boulderSpawnPoint.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), MiscHolder);
                    bGO.GetComponent<Rigidbody2D>().AddForce(bGO.transform.up * 40f, ForceMode2D.Impulse);
                }
                return;
        }
    }

    public void GameOver()
    {

    }

    private void OnEnable()
    {
        ctrl.Player.Enable();
    }

    private void OnDisable()
    {
        ctrl.Player.Disable();
    }
}
