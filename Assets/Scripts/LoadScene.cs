using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Text GameResultText;

    private PersistentGameSettings gameSettings;
    private static int LevelCount = 0;
    private TextEffects GameResultTextEffects;

    private void Start()
    {
        gameSettings = FindObjectOfType<PersistentGameSettings>();
        GameResultTextEffects = GameResultText.GetComponent<TextEffects>();

        // Start out new games by clearing level one of properties NOT high scores
        gameSettings.DeleteProperties();

        if (LevelCount > 0)
        {
            // We have played a level so now that we are back display the results of last game
            switch (gameSettings.Level1Outcome)
            {
                case PersistentGameSettings.OutcomeType.WON:
                    GameResultText.text = "You WON! - Try Again";
                    GameResultText.color = new Color32(0, 128, 0, 255); // Green
                    GameResultTextEffects.visible = true;
                    break;
                case PersistentGameSettings.OutcomeType.LOST:
                    GameResultText.text = "You LOST! - Try Again";
                    GameResultText.color = new Color32(255, 0, 0, 255); // Red
                    GameResultTextEffects.visible = true;
                    break;
                case PersistentGameSettings.OutcomeType.UNKNOWN:
                default:
                    Debug.Log("LoadScene: Unknown Game Outcome");
                    break;
            }
        }
    }

    public void LoadTheLevel(string theLevel)
    {
        // Init property so we know what is going on
        GameResultTextEffects.visible = false;
        LevelCount++;

        gameSettings.Level1Outcome = PersistentGameSettings.OutcomeType.UNKNOWN;
        gameSettings.CurrentLevel = 0;
        gameSettings.ActiveLevel = LevelCount;
        gameSettings.SaveProperties();

        SceneManager.LoadScene(theLevel);
    }
}