using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemyCardController : MonoBehaviour
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

    EnemyController Enemy;
    PlayerController Player;
    private void Start()
    {
        thisCard[0] = EnemyCardDB.cardList[thisId];
        Enemy = FindObjectOfType<EnemyController>();
        Player = FindObjectOfType<PlayerController>();
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

        if (thisCard[0].colour == "Red")
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

    public void PlayCard(string cardName, int cardPower)
    {
        if (cardName == "Slap")
        {
            float damageToDeal = cardPower - Player.Shield;
            if (damageToDeal < 0) { damageToDeal = 0; }
            Player.Health -= damageToDeal;
            Debug.Log("Enemy dealt " + damageToDeal + " damage");
            //Debug.Log("You have " + Player.Health + " health remaining");
        }
        if (cardName == "Block")
        {
            Enemy.Shield += cardPower;
            Debug.Log("Enemy shielded for " + cardPower);
        }
        if (cardName == "Drain")
        {
            float damageToDeal = cardPower - Player.Shield;
            if (damageToDeal < 0) { damageToDeal = 0; }
            Player.Health -= damageToDeal;
            Enemy.Health += cardPower;
            Debug.Log("Enemy dealt " + damageToDeal + " damage");
            //Debug.Log("You have " + Player.Health + " health remaining");
            Debug.Log("Enemy healed " + cardPower + " health");
        }
    }
}
