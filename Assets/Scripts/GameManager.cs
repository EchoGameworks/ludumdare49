using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Controls ctrl;

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

    private void Start()
    {
        SceneManager.LoadScene("Level", LoadSceneMode.Additive);
        ctrl.Menu.Restart.performed += Restart_performed;
    }

    private void Restart_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Restart();
    }

    private void Restart()
    {
        Destroy(BeakerManager.instance.gameObject);
        Destroy(FireballManager.instance.gameObject);
        Destroy(BoulderSpawner.instance.gameObject);
        SceneManager.UnloadSceneAsync("Level");
        SceneManager.LoadScene("Level");
        HUDManager.instance.NewGame();
        GameoverHUD.instance.HideGameOver();
    }

    public void GameOver()
    {
        //Scene levelScene = SceneManager.GetSceneByName("Level");
        print("Gameover in Manager");
        BeakerManager.instance.enabled = false;
        GameoverHUD.instance.ShowGameover();
        ctrl.Player.Disable();
    }

    private void OnEnable()
    {
        ctrl.Menu.Enable();
    }

    private void OnDisable()
    {
        ctrl.Menu.Disable();
    }
}
