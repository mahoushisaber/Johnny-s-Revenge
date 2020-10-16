using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDB : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    private void Awake()
    {
        // Clear must be called because Awake is called on each scene change. Consequently, anytime 
        // scene is loaded without clear we double size of DB by reloading the adds again.
        cardList.Clear();

        cardList.Add(new Card(0, "None", 0, "None", false, "None", Card.OwnerType.UNKNOWN));
        cardList.Add(new Card(1, "Slash", 10, "Slash target enemy and deal 4 damage", false, "Red", Card.OwnerType.UNKNOWN));
        cardList.Add(new Card(2, "Block", 6, "Block 6 damage from the next enemy attack", false, "Blue", Card.OwnerType.UNKNOWN));
        cardList.Add(new Card(3, "Siphon", 2, "Attack the enemy for 2 damage and heal for the same amount", false, "Green", Card.OwnerType.UNKNOWN));
    }
}
