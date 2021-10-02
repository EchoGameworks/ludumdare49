using Constants;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    [SerializeField]
    private Image heartFill;
    [SerializeField]
    private TextMeshProUGUI mainScore;

    public int DisplayScore;
    public int ActualScore;

    public float ScoreFreezeTimerMax = 3f;
    private float scoreFreezeTimer;

    //private List<Score> scoreList;
    private List<GameObject> scoreGOList;

    [SerializeField]
    private RectTransform scoreViewHolder;

    public int SuperComboToBeAdded = 0;
    public bool firstComboCollect;

    [SerializeField]
    private GameObject prefabScoreBonus;

    [SerializeField]
    private TextMeshProUGUI LevelCounterText;

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

    void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        DisplayScore = 0;
        ActualScore = 0;
        heartFill.fillAmount = 1f;
        if(scoreGOList != null)
        {
            for (int i = scoreGOList.Count - 1; i >= 0; i--)
            {
                if (scoreGOList[i] != null) Destroy(scoreGOList[i]);
            }
        }
        scoreGOList = new List<GameObject>();
        //scoreList = new List<Score>();
    }

    public void UpdateLevel(int levelNumber)
    {
        LevelCounterText.text = "Level " + levelNumber.ToString();
    }

    void Update()
    {
        if(scoreFreezeTimer > 0)
        {
            scoreFreezeTimer -= Time.deltaTime;
            firstComboCollect = true;
        }
        else
        {
            //Collect Bonuses
            //if (firstComboCollect)
            //{
            //    firstComboCollect = false;
            //    ActualScore += Mathf.RoundToInt(SuperComboToBeAdded * (1 + scoreList.Count * 0.1f));
            //}
            if (DisplayScore < ActualScore)
            {
                if (ActualScore - DisplayScore > 100f)
                {
                    DisplayScore += 10;
                }
                else
                {
                    DisplayScore += 1;
                }
                mainScore.text = DisplayScore.ToString();
            }
            if (scoreGOList != null)
            {
                for (int i = scoreGOList.Count - 1; i >= 0; i--)
                {
                    if (scoreGOList[i] != null)
                    {
                        GameObject toDestroy = scoreGOList[i];
                        LeanTween.scale(scoreGOList[i], Vector3.zero, 0.3f).setEaseInOutCirc()
                            .setOnComplete(() => Destroy(toDestroy));
                      
                    }
                }
                scoreGOList = new List<GameObject>();
            }
        }
    }

    public void UpdateHealth(float health)
    {
        if(health < 0)
        {
            health = 0f;
        }
        heartFill.fillAmount = health / 100f;
    }

    public void UpdateScore(Score score)
    {
        scoreFreezeTimer = ScoreFreezeTimerMax;
        GameObject scoreGO = Instantiate(prefabScoreBonus, scoreViewHolder);
        TextMeshProUGUI scoreText = scoreGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        scoreGO.transform.localScale = Vector3.zero;
        LeanTween.scale(scoreGO, Vector3.one, 0.3f).setEaseInOutCirc();
        scoreText.text = score.Value.ToString() + "<size=50%>(x" + score.Combo.ToString() + ")</size>";
        scoreText.color = Helpers.GetElementColor(score.Element);
        scoreGOList.Add(scoreGO);
        ActualScore += (score.Value * score.Combo);
        SuperComboToBeAdded += (score.Value * score.Combo);
    }


}
