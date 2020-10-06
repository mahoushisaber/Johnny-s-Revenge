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
        EnemyHand = GameObject.Find("Enemy Hand");
        deck.Clear();
        createDeck();
    }


    public void createDeck()
    {
        x = 0;
        deckSize = 20;

        for (int i = 0; i < deckSize; i++)
        {
            x = Random.Range(1, 4);
            deck.Insert(i, CardDB.cardList[x]);
        }
    }

    public void drawCard(int amount, bool removeFirst)
    {
        for (int i = 0; i < amount; i++)
        {
            var drawnCard = Instantiate(cardPrefab, EnemyHand.transform.position, Quaternion.identity);
            drawnCard.transform.SetParent(EnemyHand.transform);
            drawnCard.GetComponent<CardController>().thisId = deck[i].id;
            drawnCard.GetComponent<CardController>().id = deck[i].id;
            drawnCard.GetComponent<CardController>().cardName = deck[i].cardName;
            drawnCard.GetComponent<CardController>().cardDescription = deck[i].cardDescription;
            drawnCard.GetComponent<CardController>().power = deck[i].power;
            drawnCard.GetComponent<CardController>().enhanced = deck[i].enhanced;
            Debug.Log("Just drew a " + drawnCard.GetComponent<CardController>().cardName);
        }

        if (deck.Count > 0 && removeFirst)
        {
            deck.RemoveAt(0);
        }
    }
}
