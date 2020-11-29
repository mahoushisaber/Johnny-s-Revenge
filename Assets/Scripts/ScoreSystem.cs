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
    public GameObject ResultScreen;
    public GameObject RewardDropZone;
    public GameObject dropZoneObj;
    public bool ResultScreenShowing;
    public bool dropZoneObjShowing;
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
    private RewardDeck rewardDeck;
    private BattleResults CtrlObj;
    private bool showingLevelReward;
    private int rewardCardsTakenCount;

    private float currentHealthReward = 0f;
    private float currentManaReward = 0f;

    // Start is called before the first frame update
    void Start()
    {
        ResultScreenShowing = false;
        dropZoneObjShowing = false;
        showingLevelReward = false;
        player = FindObjectOfType<PlayerController>();
        game = FindObjectOfType<GameController>();
        rewardDeck = FindObjectOfType<RewardDeck>();
        gameScore = 0;
        setStageStartStatus();
        ResultScreen.SetActive(false);
        dropZoneObj.SetActive(false);
        CtrlObj = ResultScreen.GetComponent<BattleResults>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // Monitor for reward drop when result screen showing
        if (   ResultScreenShowing == true && showingLevelReward == true 
            && RewardDropZone.transform.childCount > 0)
        {
            CardController[] UI_Cards = 
                RewardDropZone.transform.GetComponentsInChildren<CardController>();

            foreach (CardController UI_card in UI_Cards)
            {
                if (UI_card.Owner == Card.OwnerType.REWARD)
                {
                    // Record Card dropped and then delete it from UI
                    rewardCardDropped(UI_card.Id);
                    Destroy(UI_card.gameObject);
                }
            }
        }

        if (!enemyDefeated)
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

        if (game.CurrentStage == game.TotalStages && game.CurrentLevel != game.TotalLevels)
        {
            showingLevelReward = true;
            rewardDeck.LevelDraw(game.CurrentLevel);
        }
        else
        {
            showingLevelReward = false;
        }
        //CtrlObjRewardCard.transform.Find("Reward Drop Zone").gameObject.SetActive(true);
        if (game.CurrentStage == 3)
        {
            dropZoneObjShowing = true;
            dropZoneObj.SetActive(true);
            if (game.CurrentLevel == 3)
            {
                dropZoneObj.SetActive(false);
                // Ideally, We can add a button to proceed instead of choosing health or mana
            }
        }
       
        ResultScreenShowing = true;
        ResultScreen.SetActive(true);
        dropZoneObjShowing = true;
        //dropZoneObj.SetActive(true);
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
        FindObjectOfType<AudioManager>().Play("Button");

        startNextStage();
    }

    public void nextStageRewardMana()
    {
        player.Mana += Mathf.Round(currentManaReward);
        FindObjectOfType<AudioManager>().Play("Button");

        startNextStage();
    }

    void rewardCardDropped(int RewardCardId)
    {
        // Add the reward card to Player deck and then delete the UI_Card
        player.GetAssignedDeck().AddRewardCard(RewardCardId);
        rewardCardsTakenCount++;

        if (   (game.CurrentLevel == 1 && rewardCardsTakenCount == 1)
            || (game.CurrentLevel == 2 && rewardCardsTakenCount == 2))
        {
            rewardCardsTakenCount = 0;
            startNextStage();
        }
    }

    void startNextStage()
    {
        setStageStartStatus();
        ResultScreen.SetActive(false);
        ResultScreenShowing = false;

        dropZoneObj.SetActive(false);
        dropZoneObjShowing = false;
        game.nextStage();
    }

    void setStageStartStatus()
    {
        currentTime = 0;
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
