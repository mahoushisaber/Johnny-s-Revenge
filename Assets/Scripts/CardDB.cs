using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDB : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    private void Awake()
    {
        cardList.Add(new Card(0, "None", 0, "None", false, "None"));
        cardList.Add(new Card(1, "Slash", 10, "Slash target enemy and deal 4 damage", false, "Red"));
        cardList.Add(new Card(2, "Block", 6, "Block 6 damage from the next enemy attack", false, "Blue"));
        cardList.Add(new Card(3, "Siphon", 2, "Attack the enemy for 2 damage and heal for the same amount", false, "Green"));
    }
}
