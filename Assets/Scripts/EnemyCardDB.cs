using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardDB : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    private void Awake()
    {
        cardList.Add(new Card(0, "None", 0, "None", false, "None"));
        cardList.Add(new Card(1, "Slap", 10, "Slap target enemy and deal 10 damage", false, "Red"));
        cardList.Add(new Card(2, "Block", 6, "Block 6 damage from the next enemy attack", false, "Blue"));
        cardList.Add(new Card(3, "Drain", 5, "Drain 5 health from the enemy", false, "Green"));
    }
}
