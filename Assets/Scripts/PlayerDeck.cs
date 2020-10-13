using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> storeDeck = new List<Card>();

    public int x;
    public int deckSize;

    public GameObject PlayerHand;
    public GameObject cardPrefab;

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
        x = 0;
        deckSize = 20;

        for (int i = 0; i < deckSize; i++)
        {
            x = Random.Range(1, 4);

            Card deckCard = CardDB.cardList[x];
            deckCard.cardOwner = Card.OwnerType.PLAYER;
            deck.Insert(i, deckCard);
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
            UI_Card.ThisCard.cardDescription = deck[0].cardDescription;
            UI_Card.ThisCard.cardName = deck[0].cardName;
            UI_Card.ThisCard.cardOwner = Card.OwnerType.PLAYER;
            UI_Card.ThisCard.colour = deck[0].colour;
            UI_Card.ThisCard.enhanced = deck[0].enhanced;
            UI_Card.ThisCard.id = deck[0].id;
            UI_Card.ThisCard.power = deck[0].power;
            UI_Card.ThisId = deck[0].id;
            UI_Card.Id = deck[0].id;
            UI_Card.CardName = deck[0].cardName;
            UI_Card.CardDescription = deck[0].cardDescription;
            UI_Card.Power = deck[0].power;
            UI_Card.Enhanced = deck[0].enhanced;
            Debug.Log("Player drew a " + UI_Card.CardName);

            if (deck.Count > 0)
            {
                deck.RemoveAt(0);
            }
        }
    }
}
