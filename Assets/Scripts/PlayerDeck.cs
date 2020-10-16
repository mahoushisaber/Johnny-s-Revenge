using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
//    public List<Card> storeDeck = new List<Card>();
    public GameObject PlayerHand;
    public GameObject cardPrefab;

    private int deckSize = 20;
    private int redLimit = 10;
    private int blueLimit = 5;
    private int greenLimit = 5;

    private void Start()
    {
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
