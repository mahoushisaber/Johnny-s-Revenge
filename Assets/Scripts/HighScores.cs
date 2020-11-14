using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScores : MonoBehaviour
{
    public Text HighScore1Text;
    public Text HighScore1PlayerText;
    public Text HighScore2Text;
    public Text HighScore2PlayerText;
    public Text HighScore3Text;
    public Text HighScore3PlayerText;
    public Text HighScore4Text;
    public Text HighScore4PlayerText;
    public Text HighScore5Text;
    public Text HighScore5PlayerText;

    // Start is called before the first frame update
    void Start()
    {
        HighScore1Text.text = HighScoreController.gCtrl.highScores[0].ToString();
        HighScore1PlayerText.text = HighScoreController.gCtrl.players[0];
        HighScore2Text.text = HighScoreController.gCtrl.highScores[1].ToString();
        HighScore2PlayerText.text = HighScoreController.gCtrl.players[1];
        HighScore3Text.text = HighScoreController.gCtrl.highScores[2].ToString();
        HighScore3PlayerText.text = HighScoreController.gCtrl.players[2];
        HighScore4Text.text = HighScoreController.gCtrl.highScores[3].ToString();
        HighScore4PlayerText.text = HighScoreController.gCtrl.players[3];
        HighScore5Text.text = HighScoreController.gCtrl.highScores[4].ToString();
        HighScore5PlayerText.text = HighScoreController.gCtrl.players[4];
    }

    public void OnClick_Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
