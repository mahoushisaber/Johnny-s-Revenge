using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdventureMapCtrl : MonoBehaviour
{
    public Button Level1Btn;
    public Text Level1BtnText;
    public Button Level2Btn;
    public Text Level2BtnText;
    public Button Level3Btn;
    public Text Level3BtnText;
    public Text GameResultText;

    private Vector3 L1BtnScale;
    private Vector3 L2BtnScale;
    private Vector3 L3BtnScale;

    private PersistentGameSettings gameSettings;
    private static int CurLevel = 0;
    private TextEffects GameResultTextEffects;

    private string[] LevelNames = { "Menu", "AdventureMap", "Gameplay" };

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Theme7");
        gameSettings = FindObjectOfType<PersistentGameSettings>();
        GameResultTextEffects = GameResultText.GetComponent<TextEffects>();
        CurLevel = gameSettings.CurrentLevel;
        Color nColor = Color.white;
        Color dColor = Color.grey;

        if (CurLevel > 0)
        {
            // We have played a level so now that we are back display the results of last game
            if (   gameSettings.Level1Outcome == PersistentGameSettings.OutcomeType.WON
                || gameSettings.Level2Outcome == PersistentGameSettings.OutcomeType.WON
                || gameSettings.Level3Outcome == PersistentGameSettings.OutcomeType.WON)
            {
                GameResultText.text = "You WON! - Select your next level";
                GameResultText.color = new Color32(0, 128, 0, 255); // Green
                GameResultTextEffects.visible = true; 
            }
            else
            {
                Debug.Log("LoadScene: Unknown Game Outcome");
            }
        }

        L1BtnScale = Level1Btn.transform.localScale;
        L2BtnScale = Level2Btn.transform.localScale;
        L3BtnScale = Level3Btn.transform.localScale;

        // Default is to disable level 2 & 3
        Level1Btn.interactable = false;
        Level2Btn.interactable = false;
        Level3Btn.interactable = false;
        Level1Btn.enabled = false;
        Level2Btn.enabled = false;
        Level3Btn.enabled = false;

        switch (CurLevel)
        {
            case 2:
                Level3Btn.interactable = true;
                Level3Btn.enabled = true;
                Level2BtnText.text = "LEVEL 2\ndone";
                Level1BtnText.text = "LEVEL 1\ndone";
                Level1Btn.GetComponent<Button>().image.color = nColor;
                Level2Btn.GetComponent<Button>().image.color = nColor;
                Level3Btn.GetComponent<Button>().image.color = nColor;
                break;
            case 1:
                Level2Btn.interactable = true;
                Level2Btn.enabled = true;
                Level1BtnText.text = "LEVEL 1\ndone";
                Level1Btn.GetComponent<Button>().image.color = nColor;
                Level2Btn.GetComponent<Button>().image.color = nColor;
                Level3Btn.GetComponent<Button>().image.color = dColor;
                break;
            case 0:
                Level1Btn.interactable = true;
                Level1Btn.enabled = true;
                Level1BtnText.text = "LEVEL 1";
                Level2BtnText.text = "LEVEL 2";
                Level1Btn.GetComponent<Button>().image.color = nColor;
                Level2Btn.GetComponent<Button>().image.color = dColor;
                Level3Btn.GetComponent<Button>().image.color = dColor;
                break;
            default:
                break;
        }
    }

    // OnMouseEnter is called when the mouse entered the GUIElement or Collider
    public void OnMouseEnter(int BtnId)
    {
        switch (BtnId)
        {
            case 1:
                if (Level1Btn.interactable == true && Level1Btn.enabled == true)
                {
                    L1BtnScale = Level1Btn.transform.localScale;
                    Level1Btn.transform.localScale = L1BtnScale * 1.2f;
                }
                break;
            case 2:
                if (Level2Btn.interactable == true && Level2Btn.enabled == true)
                {
                    L2BtnScale = Level2Btn.transform.localScale;
                    Level2Btn.transform.localScale = L2BtnScale * 1.2f;
                }
                break;
            case 3:
                if (Level3Btn.interactable == true && Level3Btn.enabled == true)
                {
                    L3BtnScale = Level3Btn.transform.localScale;
                    Level3Btn.transform.localScale = L3BtnScale * 1.2f;
                }
                break;
            default:
                break;
        }
    }

    // OnMouseExit is called when the mouse is no longer over the GUIElement or Collider
    public void OnMouseExit(int BtnId)
    {
        switch (BtnId)
        {
            case 1:
                Level1Btn.transform.localScale = L1BtnScale;
                break;
            case 2:
                Level2Btn.transform.localScale = L2BtnScale;
                break;
            case 3:
                Level3Btn.transform.localScale = L3BtnScale;
                break;
            default:
                break;
        }
    }

    // OnClick is called when the button has been clicked
    public void OnClick(int BtnId)
    {
        if (BtnId >= 1 && BtnId <= 3)
        {
            // Note: Scene strings are zero based where level buttons are 1 based representing level
            // Save the new selected level to be picked up by next scene
            gameSettings.CurrentLevel = BtnId - 1;
            FindObjectOfType<AudioManager>().Play("Button");
            SceneManager.LoadScene("3D Gameplay", LoadSceneMode.Single);
        }
    }
}
