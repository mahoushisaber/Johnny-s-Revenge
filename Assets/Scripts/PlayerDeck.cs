using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public GameObject PlayerHand;
    public GameObject cardPrefab;
    public int deckSize;
    
    private int redLimit;
    private int blueLimit;
    private int greenLimit;

    static public bool level1completed;
    static public bool level2completed;
    static public int rewardCard1Id;
    static public int rewardCard2Id;

    private void Start()
    {
        redLimit = 14;
        blueLimit = 2;
        greenLimit = 4;
        if (PlayerHand == null)
        {
            PlayerHand = GameObject.Find("Player Hand");
        }
        deck.Clear();
        CreateDeck();
        InitialDraw();
    }

    public void CreateDeck()
    {
        int x;
        int randomCeiling = CardDB.cardList.Count;
        int redCount = 0, blueCount = 0, greenCount = 0;
        int i = 0;

        while (i < deckSize)
        {
            if (level1completed && i < 2)
            {
                Card rewardCard = new Card(CardDB.rewardCardList.Find(Card => Card.Id == rewardCard1Id))
                {
                    Owner = Card.OwnerType.PLAYER
                };
                if (rewardCard1Id == 8) blueCount++;
                if (rewardCard1Id == 9) redCount++;
                deck.Add(rewardCard);
                i++;
                continue;
            }

            if (level2completed && i < 4)
            {
                Card rewardCard = new Card(CardDB.rewardCardList.Find(Card => Card.Id == rewardCard2Id))
                {
                    Owner = Card.OwnerType.PLAYER
                };
                if (rewardCard1Id == 10) redCount++;
                if (rewardCard1Id == 11) greenCount++;
                deck.Add(rewardCard);
                i++;
                continue;
            }


            x = Random.Range(1, randomCeiling);

            // Sanity check because Random.Range is not working the way it is advertised
            if (x >= randomCeiling)
            {
                x = randomCeiling - 1;
            }

            Card deckCard = new Card(CardDB.cardList[x])
            {
                Owner = Card.OwnerType.PLAYER
            };
            
            if (deckCard.Colour == "Red")
            {
                if (redCount >= redLimit) continue;
                else
                {
                    redCount++;
                }
            }
            if (deckCard.Colour == "Blue")
            {
                if (blueCount >= blueLimit) continue;
                else
                {
                    blueCount++;
                }
            }
            if (deckCard.Colour == "Green")
            {
                if (greenCount >= greenLimit) continue;
                else greenCount++;

            }
            
            deck.Add(deckCard);
            i++;
        }
    }

    public void InitialDraw()
    {
        int cardsToDraw = 5;
        DrawCard(cardsToDraw); 
    }

    public void DrawCard(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            var drawnCard = Instantiate(cardPrefab, PlayerHand.transform.position, Quaternion.identity);
            drawnCard.transform.SetParent(PlayerHand.transform);
            drawnCard.transform.localScale = new Vector3(1.6f, 1.6f, 1);
            //drawnCard.transform.Rotate(65, 0, 0);

            CardController UI_Card = drawnCard.GetComponent<CardController>();
            UI_Card.InitWithCard(deck[0]);
            Debug.Log("Player drew a " + UI_Card.Name);

            if (deck.Count > 0)
            {
                deck.RemoveAt(0);
            }
        }
    }
}
