using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class CardController : MonoBehaviour
{
    public Card ThisCard = new Card();
    public int ThisId;

    public int Id;
    public string CardName;
    public int Power;
    public string CardDescription;
    public bool Enhanced;

    public Text NameText;
    public Text PowerText;
    public Text DescriptionText;

    public Image CardColour;

    EnemyController Enemy;
    PlayerController Player;
    private void Start()
    {
        // The owner of card was assigned during the creation so save and restore during load of defaults
//        Card.OwnerType cardOwner = ThisCard.cardOwner;
//        ThisCard = CardDB.cardList[ThisId];
//        ThisCard.cardOwner = cardOwner;

        thisCard[0] = CardDB.cardList[thisId];
        Enemy = FindObjectOfType<EnemyController>();
        Player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        Id = ThisCard.id;
        CardName = ThisCard.cardName;
        Power = ThisCard.power;
        CardDescription = ThisCard.cardDescription;

        if (Enhanced)
        {
            Power *= 2;
        }

        NameText.text = CardName;
        PowerText.text = Power.ToString();
        DescriptionText.text = CardDescription; 

        if (ThisCard.colour == "Red")
        {
            CardColour.GetComponent<Image>().color = new Color32(115, 15, 45, 255);
        } 
        if (ThisCard.colour == "Blue")
        {
            CardColour.GetComponent<Image>().color = new Color32(25, 90, 140, 255);
        }
        if (ThisCard.colour == "Green")
        {
            CardColour.GetComponent<Image>().color = new Color32(30, 125, 20, 255);
        }
        if (ThisCard.colour == "None")
        {
            CardColour.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }
    }

    public int GetPower()
    {
        return Enhanced ? Power*2 : Power;
    }

    public void PlayCard(string cardName, int cardPower)
    {
        if (cardName == "Slash")
        {
            float damageToDeal = cardPower - Enemy.Shield;
            if (damageToDeal < 0) { damageToDeal = 0; }
            Enemy.Health -= damageToDeal; 
            Debug.Log("You dealt " + damageToDeal + " damage");
            Debug.Log("The enemy now has " + Enemy.Health + " health");
        }
        if (cardName == "Block")
        {
            Player.Shield += cardPower;
            Debug.Log("You shielded for " + cardPower);
        }
        if (cardName == "Siphon")
        {
            float damageToDeal = cardPower - Enemy.Shield;
            if (damageToDeal < 0) { damageToDeal = 0; }
            Enemy.Health -= damageToDeal;
            Player.Health += cardPower;
            Debug.Log("You dealt " + damageToDeal + " damage");
            Debug.Log("The enemy now has " + Enemy.Health + " health");
            Debug.Log("You healed for " + cardPower + " health");
        }
    }
}
