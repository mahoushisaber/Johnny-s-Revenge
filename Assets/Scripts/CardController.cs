using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class CardController : MonoBehaviour
{
    public List<Card> thisCard = new List<Card>();
    public int thisId;

    public int id;
    public string cardName;
    public int power;
    public string cardDescription;
    public bool enhanced;

    public Text nameText;
    public Text powerText;
    public Text descriptionText;

    public Image cardColour;

    private void Start()
    {
        thisCard[0] = CardDB.cardList[thisId]; 
    }
    private void Update()
    {
        id = thisCard[0].id;
        cardName = thisCard[0].cardName;
        power = thisCard[0].power;
        cardDescription = thisCard[0].cardDescription;
        enhanced = thisCard[0].enhanced;

        nameText.text = "" + cardName;
        powerText.text = "" + power;
        descriptionText.text = "" + cardDescription; 

        if(thisCard[0].colour == "Red")
        {
            cardColour.GetComponent<Image>().color = new Color32(115, 15, 45, 255);
        } 
        if (thisCard[0].colour == "Blue")
        {
            cardColour.GetComponent<Image>().color = new Color32(25, 90, 140, 255);
        }
        if (thisCard[0].colour == "Green")
        {
            cardColour.GetComponent<Image>().color = new Color32(30, 125, 20, 255);
        }
        if (thisCard[0].colour == "None")
        {
            cardColour.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }

        if (enhanced)
        {
            thisCard[0].power *= 2;
        }
    }
    public int GetPower()
    {
        return this.power;
    }
}
