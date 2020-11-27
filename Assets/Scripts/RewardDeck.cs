using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public GameObject RewardHand;
    public GameObject cardPrefab;
    private int deckSize;
    static private List<Card> rewardCardsGiven = new List<Card>();


    private void Start()
    {
        if (RewardHand == null)
        {
            RewardHand = GameObject.Find("Reward Hand");
        }
        deckSize = CardDB.rewardCardList.Count;
        deck.Clear();
        CreateDeck();

        // Note draw will only occur when a level deck build reward occurs
    }

    public List<T> Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int k = Random.Range(0, i);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }

        return list;
    }

    public void CreateDeck()
    {
        int i = 0;

        while (i < deckSize)
        {
            Card deckCard = new Card(CardDB.rewardCardList[i]) { Owner = Card.OwnerType.REWARD };
            bool addCard = true;

            foreach (Card aRewardedCard in rewardCardsGiven)
            {
                if (aRewardedCard.Id == deckCard.Id)
                {
                    addCard = false;
                    break;
                }
            }

            if (addCard == true)
            {
                deck.Add(deckCard);
            }
            i++;
        }
        deck = Shuffle(deck);
    }

    public void LevelDraw(int LevelToReward)
    {
        int cardsToDraw = LevelToReward + 1;

        // Don't draw more than in the deck
        if (cardsToDraw >= deckSize)
        {
            cardsToDraw = deckSize - 1;
        }
        DrawCard(cardsToDraw);
    }

    public void DrawCard(int amount)
    {
        if (RewardHand != null)
        {
            for (int i = 0; i < amount; i++)
            {
                var drawnCard = Instantiate(cardPrefab, RewardHand.transform.position, Quaternion.identity);
                drawnCard.transform.SetParent(RewardHand.transform);
                drawnCard.transform.localScale = new Vector3(1.6f, 1.6f, 1);

                CardController UI_Card = drawnCard.GetComponent<CardController>();
                UI_Card.InitWithCard(deck[i]);
                Debug.Log("Player reward card option " + UI_Card.Name);
            }
        }
        else
        {
            Debug.Log("ERROR! Connot deal Player reward card option as Reward Hand Dropzone is not assigned");
        }
    }

    public void ClearRewardCardsGiven()
    {
        rewardCardsGiven.Clear();
    }

    public Card RemoveCardFromDeck(int RewardId)
    {
        Card removedCard = null;

        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].Id == RewardId)
            {
                removedCard = deck[i];
                rewardCardsGiven.Add(removedCard);
                deck.RemoveAt(i);
                deckSize--;
                break;
            }
        }

        return removedCard;
    }
}
