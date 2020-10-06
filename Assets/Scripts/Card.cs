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

    public string colour;

    public Card()
    {

    }

    public Card(int Id, string CardName, int Power, string CardDescription, bool Enhanced, string Colour)
    {
        id = Id;
        cardName = CardName;
        power = Power;
        cardDescription = CardDescription;
        enhanced = Enhanced;

        colour = Colour;
    }
}
