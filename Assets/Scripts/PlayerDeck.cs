using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
//    public List<Card> storeDeck = new List<Card>();
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
        int x;
        int randomCeiling = CardDB.cardList.Count;

        for (int i = 0; i < deckSize; i++)
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
            deck.Add(deckCard);
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
