using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Text GameResultText;
    public GameObject HighScoreDialog;
    public Text NewPlayerName;

    private PersistentGameSettings gameSettings;
    private HighScoreController scoreCntrl;
    private static int LevelCount = 0;
    private TextEffects GameResultTextEffects;
    private bool newHSFound = false;
    private int newGameHighScore = 0;

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Theme5");
        gameSettings = FindObjectOfType<PersistentGameSettings>();
        GameResultTextEffects = GameResultText.GetComponent<TextEffects>();
        scoreCntrl = FindObjectOfType<HighScoreController>();

        // Start out new games by clearing level one of properties NOT high scores
        gameSettings.DeleteProperties();

        if (LevelCount > 0)
        {
            PersistentGameSettings.OutcomeType lastLevelOutcome = PersistentGameSettings.OutcomeType.UNKNOWN;

            // We have played a level so now that we are back display the results of last game
            switch (gameSettings.CurrentLevel)
            {
                case 1:
                    lastLevelOutcome = gameSettings.Level1Outcome;
                    break;
                case 2:
                    lastLevelOutcome = gameSettings.Level2Outcome;
                    break;
                case 3:
                    lastLevelOutcome = gameSettings.Level3Outcome;
                    break;
                default:
                    Debug.Log("LoadScene: Unknown Game Outcome");
                    break;
            }
            switch (lastLevelOutcome)
            {
                case PersistentGameSettings.OutcomeType.WON:
                    newGameHighScore = gameSettings.Level1Score + gameSettings.Level2Score + gameSettings.Level3Score;
                    newHSFound = scoreCntrl.IsThisAHighScore(newGameHighScore);
                    if (newHSFound == true)
                    {
                        // Show Dialog for collecting high score and set flag to wait for results
                        HighScoreDialog.SetActive(true);
                    }
                    else
                    {
                        GameResultText.text = "You WON! - Try Again";
                        GameResultText.color = new Color32(0, 128, 0, 255); // Green
                        GameResultTextEffects.visible = true;
                    }
                    LevelCount = 0;
                    break;

                case PersistentGameSettings.OutcomeType.LOST:
                    GameResultText.text = "You LOST! - Try Again";
                    GameResultText.color = new Color32(255, 0, 0, 255); // Red
                    GameResultTextEffects.visible = true;
                    LevelCount = 0;
                    break;
                case PersistentGameSettings.OutcomeType.UNKNOWN:
                default:
                    Debug.Log("LoadScene: Unknown Game Outcome");
                    break;
            }
        }
    }

    public void OnClick_OK_HSPlayerEntry()
    {
        scoreCntrl.SaveHighScore(newGameHighScore, NewPlayerName.text);
        HighScoreDialog.SetActive(false);
        GameResultText.text = "HIGH SCORE! - Try Again";
        GameResultText.color = new Color32(0, 128, 0, 255); // Green
        GameResultTextEffects.visible = true;
    }

    public void LoadTheLevel(string theLevel)
    {
        // Init property so we know what is going on
        GameResultTextEffects.visible = false;

        if (theLevel != "HighScores")
        {
            LevelCount++;
        }
        gameSettings.CurrentLevel = 0;
        gameSettings.ActiveLevel = LevelCount;
        gameSettings.SaveProperties();

        SceneManager.LoadScene(theLevel, LoadSceneMode.Single);
    }
}