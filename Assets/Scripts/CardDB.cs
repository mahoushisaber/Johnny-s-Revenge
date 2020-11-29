using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDB : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();
    public static List<Card> enemyCardList = new List<Card>();
    public static List<Card> rewardCardList = new List<Card>();
    
    private void Awake()
    {
        // Clear must be called because Awake is called on each scene change. Consequently, anytime 
        // scene is loaded without clear we double size of DB by reloading the adds again.
        cardList.Clear();

        cardList.Add(new Card(0, "None", 0, "None", false, "None", Card.OwnerType.UNKNOWN, "0 (3)"));
        cardList.Add(new Card(1, "Slash", 8, "Deal 8 damage", false, "Red", Card.OwnerType.UNKNOWN, "0 (3)"));
        cardList.Add(new Card(2, "Block", 6, "Block 6 damage from the enemy this turn", false, "Blue", Card.OwnerType.UNKNOWN, "shield"));
        cardList.Add(new Card(3, "Siphon", 2, "Deal 2 damage and Restore 2 health", false, "Green", Card.OwnerType.UNKNOWN,"010-hook"));
        cardList.Add(new Card(4, "Saber Attack", 10, "Deal 10 damage but lose 3 health. ", false, "Red", Card.OwnerType.UNKNOWN, "0 (7)"));
        cardList.Add(new Card(5, "Power Strike", 5, "Deal 5 damage,  if your mana is above 50%, gain 25 mana", false, "Red", Card.OwnerType.UNKNOWN, "0 (6)"));
        cardList.Add(new Card(6, "Pierce", 6, "Deal 6 damage ignoring shield", false, "Red", Card.OwnerType.UNKNOWN, "saber (1)"));
        cardList.Add(new Card(7, "Heal", 5, "Restore 5 health", false, "Green", Card.OwnerType.UNKNOWN, "014-rum"));

        // -- The commented cards below are cards removed from the original deck. They were not 
        // -- working well, and playtests complained they were unfair as they are. For release 
        // -- it was decided to leave them out as it was too much work and would require a 
        // -- rebalance if they were modified and readded.
        //cardList.Add(new Card(5, "Retreat", 2, "Gain 2 blocks, every time this card is played, increase its block by 2 for this combat", false, "Blue", Card.OwnerType.UNKNOWN, "leg"));
        //cardList.Add(new Card(4, "Dagger Throw", 3, "Deal 3 damage. Every time this card is played, increase its damage by 3 for this combat", false, "Red", Card.OwnerType.UNKNOWN, "dagger (1)"));

        rewardCardList.Clear();

        // NOTE: ID must be consecutive or consequitive with cardList as these are added to the cardList when rewarded
        rewardCardList.Add(new Card(8, "Vision", 10, "Apply 10 blocks for next turn.", false, "Blue", Card.OwnerType.UNKNOWN, "024-telescope"));
        rewardCardList.Add(new Card(9, "Rifles", 8, "Deal 8 damage, deal 25 damage when upgraded", false, "Red", Card.OwnerType.UNKNOWN, "Pistol"));
        rewardCardList.Add(new Card(10, "Double Shot", 5, "Deal 5 damage 2 times", false, "Red", Card.OwnerType.UNKNOWN, "DoubleShot"));
        rewardCardList.Add(new Card(11, "Anger", 25, "gain 25% of your mana", false, "Green", Card.OwnerType.UNKNOWN, "038-tattoo-1"));

        enemyCardList.Clear();

        enemyCardList.Add(new Card(0, "None", 0, "None", false, "None", Card.OwnerType.UNKNOWN, "0 (3)"));
        enemyCardList.Add(new Card(1, "Slash", 8, "Deal 8 damage", false, "Red", Card.OwnerType.UNKNOWN, "0 (3)"));
        enemyCardList.Add(new Card(2, "Block", 6, "Block 6 damage from the next enemy attack", false, "Blue", Card.OwnerType.UNKNOWN, "shield"));
        enemyCardList.Add(new Card(3, "Siphon", 2, "Deal 2 damage and Restore 2 health", false, "Green", Card.OwnerType.UNKNOWN, "010-hook"));
        enemyCardList.Add(new Card(4, "Pierce", 6, "Deal 6 damage ignoring shield", false, "Red", Card.OwnerType.UNKNOWN, "saber (1)"));
        enemyCardList.Add(new Card(5, "Heal", 5, "Restore 5 health", false, "Green", Card.OwnerType.UNKNOWN, "014-rum"));
    }
}
