using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public Text stageResultText;
    public Text timeText;
    public Text timeSpentText;
    public Text damageTakenText;
    public Text manaUsedText;
    public Text cardsUsedText;
    public Text stageScoreText;
    public Text gameScoreText;
    public Image chestOpenImage;
    public GameObject ResultScreen;
    public bool ResultScreenShowing;
    public int gameScore;
    public int levelScore;

    private float healthBefore;
    private float manaBefore;
    private int cardsUsedBefore;
    private bool enemyDefeated;
    private float damageTaken;
    private float manaUsed;
    private int cardsUsed;
    private float currentTime;
    private PlayerController player;
    private GameController game;
    private BattleResults CtrlObj;

    private float chestTmr;
    private float currentHealthReward = 0f;
    private float currentManaReward = 0f;

    // Start is called before the first frame update
    void Start()
    {
        ResultScreenShowing = false;
        player = FindObjectOfType<PlayerController>();
        game = FindObjectOfType<GameController>();
        gameScore = 0;
        setStageStartStatus();
        ResultScreen.SetActive(false);
        CtrlObj = ResultScreen.GetComponent<BattleResults>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ResultScreen.activeSelf == true)
        {
            chestTmr += Time.deltaTime;

            if (chestTmr > 3.0f)
            {
                chestTmr -= 3.0f;
                chestOpenImage.gameObject.SetActive(!chestOpenImage.gameObject.activeSelf);
            }
        }
        else if (!enemyDefeated)
        {
            currentTime += Time.deltaTime;
            timeText.text = DisplayTime(currentTime);
        }

    }

    public void showResult()
    {
        Debug.Log("RESULT PRINTED");
        stageResultText.text = string.Format("Stage {0} Result", game.CurrentStage);
        enemyDefeated = true;
        damageTaken = healthBefore - player.Health;
        manaUsed = manaBefore - player.Mana;
        cardsUsed = player.cardsUsed - cardsUsedBefore;

        damageTakenText.text = damageTaken.ToString();
        manaUsedText.text = manaUsed.ToString();
        cardsUsedText.text = cardsUsed.ToString();
        timeSpentText.text = timeText.text;

        levelScore = (int)calculatelevelScore();
        gameScore += levelScore;

        stageScoreText.text = "Stage Score: " + levelScore.ToString();
        gameScoreText.text = "Total Score: " + gameScore.ToString();

        currentManaReward = manaUsed * levelScore / 1000;
        currentHealthReward = damageTaken * levelScore / 1000;
        CtrlObj.SetRewards(currentHealthReward, currentManaReward);
        Debug.LogFormat("health reward = {00:0}  mana reward = {00:0}", currentHealthReward, currentManaReward);

        ResultScreenShowing = true;
        ResultScreen.SetActive(true);
    }

    string DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void nextStageRewardHealth()
    {
        player.Health += Mathf.Round(currentHealthReward);
        startNextStage();
    }

    public void nextStageRewardMana()
    {
        player.Mana += Mathf.Round(currentManaReward);
        startNextStage();
    }

    void startNextStage()
    {
        setStageStartStatus();
        ResultScreen.SetActive(false);
        ResultScreenShowing = false;
        game.nextStage();
    }

    void setStageStartStatus()
    {
        currentTime = 0;
        chestTmr = 0.0f;
        healthBefore = player.Health;
        manaBefore = player.Mana;
        cardsUsedBefore = player.cardsUsed;
        enemyDefeated = false;
    }

    float calculatelevelScore()
    {
        float timeScore, healthScore, manaScore, cardScore;

        // Time - Maximum score is 250, every 30 seconds loses 50 points
        if (currentTime >= 150)
        {
            timeScore = 0;
        }
        else
        {
            timeScore = 250 - (currentTime/30)*50; 
        }
        
        // Health - Maximum score is 250, 1 health loses 2 points
        healthScore = 250 - damageTaken*2;

        // Mana - Maximum score is 250, using 1 mana loses 2 points
        manaScore = 250 - manaUsed*2;

        // Cards used - Maximum score is 250, 1 card loses 10 points
        if (cardsUsed > 25)
        {
            cardScore = 0;
        }
        else
        {
            cardScore = 250 - cardsUsed*10;
        }

        // Total
        return timeScore + healthScore + manaScore + cardScore;
    }
}
