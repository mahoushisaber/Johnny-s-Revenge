﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class CardController : MonoBehaviour
{
    public int Id { get { return ThisCard.Id; } } // To set use InitWithCard(Card)
    public string Name { get { return ThisCard.Name; } }
    public int Power { get { return Enhanced ? ThisCard.Power * EnhancedMultiplier : ThisCard.Power; } }
    public string Description { get { return ThisCard.Description; } }
    public bool Enhanced { get { return ThisCard.Enhanced; } set { ThisCard.Enhanced = value; } }
    public Card.OwnerType Owner { get { return ThisCard.Owner; } set { ThisCard.Owner = value; } }

    public int EnhancedMultiplier { get { return EnhanceMulti; } set { EnhanceMulti = (value > 1) ? value : 1; } }

    public Image ImageToColour;
    public Text NameText;
    public Text PowerText;
    public Text DescriptionText;

    private Card ThisCard = new Card();
    private int EnhanceMulti = 2;

    private void Update()
    {
        NameText.text = Name;
        PowerText.text = Power.ToString();
        DescriptionText.text = Description; 

        if (ThisCard.Colour == "Red")
        {
            ImageToColour.GetComponent<Image>().color = new Color32(115, 15, 45, 255);
        } 
        if (ThisCard.Colour == "Blue")
        {
            ImageToColour.GetComponent<Image>().color = new Color32(25, 90, 140, 255);
        }
        if (ThisCard.Colour == "Green")
        {
            ImageToColour.GetComponent<Image>().color = new Color32(30, 125, 20, 255);
        }
        if (ThisCard.Colour == "None")
        {
            ImageToColour.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }
    }

    public void InitWithCard(Card ToLoad)
    {
        ThisCard.Copy(ToLoad);
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
