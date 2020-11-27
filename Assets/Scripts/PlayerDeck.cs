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
    private RewardDeck rewardDeck;
    static private List<Card> rewardCards = new List<Card>();

    private void Start()
    {
        redLimit = 14;
        blueLimit = 2;
        greenLimit = 4;
        if (PlayerHand == null)
        {
            PlayerHand = GameObject.Find("Player Hand");
        }
        rewardDeck = FindObjectOfType<RewardDeck>();
        deck.Clear();
        CreateDeck();
        InitialDraw();
    }

    public void CreateDeck()
    {
        int x;
        int randomCeiling = CardDB.cardList.Count + rewardCards.Count;
        int redCount = 0, blueCount = 0, greenCount = 0;
        int i = 0;

        while (i < deckSize)
        {
            Card deckCard = null;
            x = Random.Range((int)1, randomCeiling);

            // Sanity check because Random.Range is not working the way it is advertised
            if (x >= randomCeiling)
            {
                x = randomCeiling - 1;
            }

            if (x >= CardDB.cardList.Count)
            {
                int y = Random.Range((int)0, (int)rewardCards.Count);
                if (y >= rewardCards.Count)
                {
                    y = rewardCards.Count - 1;
                }
                deckCard = new Card(rewardCards[y]) { Owner = Card.OwnerType.PLAYER };
            }
            else
            {
                deckCard = new Card(CardDB.cardList[x]) { Owner = Card.OwnerType.PLAYER };
            }

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

    public void ClearRewardCards()
    {
        rewardCards.Clear();
        rewardDeck.ClearRewardCardsGiven();
    }

    public void AddRewardCard(int RewardId)
    {
        Card rewardCard = rewardDeck.RemoveCardFromDeck(RewardId);

        if (rewardCard != null)
        {
            deck.Add(rewardCard);
            deckSize++;
            rewardCards.Add(rewardCard);
        }
    }
}
