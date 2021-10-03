using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverHUD : MonoBehaviour
{
    public static GameoverHUD instance;

    public GameObject GameOverGO;

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
        HideGameOver();
    }

    public void HideGameOver()
    {
        GameOverGO.SetActive(false);
    }

    public void ShowGameover()
    {
        GameOverGO.SetActive(true);
    }

    
}
