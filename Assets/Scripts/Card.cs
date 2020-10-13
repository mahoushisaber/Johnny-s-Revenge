using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Card
{
    public int id;
    public string cardName;
    public int power;
    public string cardDescription;
    public bool enhanced;

    public enum OwnerType { UNKNOWN, ENEMY, PLAYER };
    public OwnerType cardOwner;

    public string colour;

    public Card()
    {
        cardOwner = OwnerType.UNKNOWN;
    }

    public Card(int Id, string CardName, int Power, string CardDescription, bool Enhanced, string Colour)
    {
        id = Id;
        cardName = CardName;
        power = Power;
        cardDescription = CardDescription;
        enhanced = Enhanced;
        
        cardOwner = OwnerType.UNKNOWN;

        colour = Colour;
    }
}
