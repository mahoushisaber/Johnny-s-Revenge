using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public Text timeText;
    public Text timeSpentText;
    public Text damageTakenText;
    public Text manaUsedText;
    public Text cardsUsedText;
    public Text totalScoreText;
    public GameObject ResultScreen;
    
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
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        game = FindObjectOfType<GameController>();
        setStageStartStatus();
        ResultScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyDefeated)
        {
            currentTime += Time.deltaTime;
            timeText.text = DisplayTime(currentTime);
        }

    }

    public void showResult()
    {
        Debug.Log("RESULT PRINTED");
        Time.timeScale = 0;
        enemyDefeated = true;
        damageTaken = healthBefore - player.Health;
        manaUsed = manaBefore - player.Mana;
        cardsUsed = player.cardsUsed - cardsUsedBefore;

        damageTakenText.text = damageTaken.ToString();
        manaUsedText.text = manaUsed.ToString();
        cardsUsedText.text = cardsUsed.ToString();
        timeSpentText.text = timeText.text;

        int totalScore = (int)calculateTotalScore();

        totalScoreText.text = "Total Score: " + totalScore.ToString();

        ResultScreen.SetActive(true);
    }

    string DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void nextStage()
    {
        setStageStartStatus();
        ResultScreen.SetActive(false);
        Time.timeScale = 1;
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

    float calculateTotalScore()
    {
        float timeScore, healthScore, manaScore, cardScore;

        // Time - Maximum score is 250, every 30 seconds loses 50 points
        timeScore = 250 - (currentTime/30)*50; 
        
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
