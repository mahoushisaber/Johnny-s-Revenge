using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
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
                Owner = Card.OwnerType.ENEMY
            };
            deck.Add(deckCard);
        }
    }

    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var drawnCard = Instantiate(cardPrefab, EnemyHand.transform.position, Quaternion.identity);
            drawnCard.transform.SetParent(EnemyHand.transform);
            drawnCard.transform.localScale = new Vector3(1.6f, 1.6f, 1);
            //drawnCard.transform.Rotate(65, 0, 0); 

            CardController UI_Card = drawnCard.GetComponent<CardController>();
            UI_Card.InitWithCard(deck[0]);
            Debug.Log("Enemy drew a " + UI_Card.Name);
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
