using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    public enum TrainingStep { Intro, Details, Beakers, Controls }
    public TrainingStep CurrentStep;

    [SerializeField]
    private GameObject Intro;
    [SerializeField]
    private GameObject Details;
    [SerializeField]
    private GameObject Beakers;
    [SerializeField]
    private GameObject Controls;
    [SerializeField]
    private GameObject Continue;

    GameManager gameManager;
    Controls ctrl;
    Rigidbody2D playerRB2D;
    BeakerManager beakerManager;
    public TextMeshProUGUI continueText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        CurrentStep = TrainingStep.Intro;
        ctrl = gameManager.ctrl;
        ctrl.Menu.Help.performed += Help_performed;
        GameObject player = GameObject.FindGameObjectWithTag("Player");       
        playerRB2D = player.GetComponent<Rigidbody2D>();
        GameObject beakerManagerGO = GameObject.FindGameObjectWithTag("Beaker Manager");
        beakerManager = beakerManagerGO.GetComponent<BeakerManager>();

        beakerManager.enabled = false;
        playerRB2D.isKinematic = true;
        Intro.SetActive(true);
        Details.SetActive(false);
        Beakers.SetActive(false);
        Controls.SetActive(false);
        Continue.SetActive(true);

        ctrl.Player.Disable();
    }

    private void Help_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        switch (CurrentStep)
        {
            case TrainingStep.Intro:

                //Go To Details...
                Intro.SetActive(false);
                Details.SetActive(true);
                Beakers.SetActive(false);
                Controls.SetActive(false);
                Continue.SetActive(true);

                //Unfreeze Prakzar from fall
                playerRB2D.isKinematic = false;
                CurrentStep = TrainingStep.Details;
                break;
            case TrainingStep.Details:
                Intro.SetActive(false);
                Details.SetActive(false);
                Beakers.SetActive(true);
                Controls.SetActive(false);
                Continue.SetActive(true);
                CurrentStep = TrainingStep.Beakers;
                break;
            case TrainingStep.Beakers:
                Intro.SetActive(false);
                Details.SetActive(false);
                Beakers.SetActive(false);
                Controls.SetActive(true);
                Continue.SetActive(true);

                //Unlock Player Controls
                ctrl.Player.Enable();
                beakerManager.enabled = true;
                CurrentStep = TrainingStep.Controls;
                continueText.text = "to Hide Menu";
                break;
            case TrainingStep.Controls:
                Intro.SetActive(false);
                Details.SetActive(false);
                Beakers.SetActive(false);
                Controls.SetActive(false);
                Continue.SetActive(false);
                
                break;
        }
    }

    private void OnDisable()
    {
        ctrl.Menu.Help.performed -= Help_performed;
    }

    private void OnDestroy()
    {
        ctrl.Menu.Help.performed -= Help_performed;
    }
    
}
