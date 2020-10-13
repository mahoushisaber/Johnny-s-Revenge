using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();

    public int x;
    public int deckSize;

    public GameObject EnemyHand;
    public GameObject cardPrefab;

    private void Start()
    {
        if (EnemyHand == null)
        {
            EnemyHand = GameObject.Find("Enemy Hand");
        }
        deck.Clear();
        CreateDeck();
    }


    public void CreateDeck()
    {
        x = 0;
        deckSize = 20;

        for (int i = 0; i < deckSize; i++)
        {
            x = Random.Range(1, 4);

            Card deckCard = CardDB.cardList[x];
            deckCard.cardOwner = Card.OwnerType.ENEMY;
            deck.Insert(i, deckCard);
        }
    }

    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var drawnCard = Instantiate(cardPrefab, EnemyHand.transform.position, Quaternion.identity);
            drawnCard.transform.SetParent(EnemyHand.transform);

            CardController UI_Card = drawnCard.GetComponent<CardController>();
            UI_Card.ThisCard.cardDescription = deck[0].cardDescription;
            UI_Card.ThisCard.cardName = deck[0].cardName;
            UI_Card.ThisCard.cardOwner = Card.OwnerType.ENEMY;
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
            Debug.Log("Enemy drew a " + UI_Card.CardName);
        }

        if (deck.Count > 0)
        {
            deck.RemoveAt(0);
        }
    }

    public bool PlayCard( int handPostion, GameObject newParent)
    {
        bool result = false;
        CardController[] UI_Cards = EnemyHand.gameObject.transform.GetComponentsInChildren<CardController>();

        // Valid deck card position and new parent?
        if (handPostion > 0 && handPostion <= UI_Cards.Length && newParent != null)
        {
            // Cards are always in order of add so Move the card to parent
            UI_Cards[handPostion - 1].transform.SetParent(newParent.transform);
            result = true;
        }
        
        Debug.Log(result?"Success:":"Failed" + "Playing an Enemy Card from Enemy Hand");
        
        return result;
    }
}
