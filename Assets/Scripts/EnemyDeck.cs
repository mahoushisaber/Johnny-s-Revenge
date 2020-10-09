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
            deck.Insert(i, EnemyCardDB.cardList[x]);
        }
    }

    public void drawCard(int amount, bool removeFirst)
    {
        for (int i = 0; i < amount; i++)
        {
            var drawnCard = Instantiate(cardPrefab, EnemyHand.transform.position, Quaternion.identity);
            drawnCard.transform.SetParent(EnemyHand.transform);
            drawnCard.GetComponent<EnemyCardController>().thisId = deck[i].id;
            drawnCard.GetComponent<EnemyCardController>().id = deck[i].id;
            drawnCard.GetComponent<EnemyCardController>().cardName = deck[i].cardName;
            Debug.Log("SET NAME TO " + deck[i].cardName);
            drawnCard.GetComponent<EnemyCardController>().cardDescription = deck[i].cardDescription;
            drawnCard.GetComponent<EnemyCardController>().power = deck[i].power;
            drawnCard.GetComponent<EnemyCardController>().enhanced = deck[i].enhanced;
            drawnCard.gameObject.name = deck[i].cardName;
            Debug.Log("Enemy just drew a " + drawnCard.GetComponent<EnemyCardController>().cardName);

        }

        if (deck.Count > 0 && removeFirst)
        {
            deck.RemoveAt(0);
        }
    }
}
