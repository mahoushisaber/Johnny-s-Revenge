using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{

    public float Health = 30f;
    public float MaxHealth = 30f;
    public float Shield = 0;
    public bool EnemyTurn = false;
    PlayerController Player;
    PlayerDeck PlayerDeck;
    EnemyDeck EnemyDeck;
    EnemyCardController EnemyCard;
    public GameObject EnemyHand;
    void Start()
    {
        Health = MaxHealth;
        Player = FindObjectOfType<PlayerController>();
        PlayerDeck = FindObjectOfType<PlayerDeck>();
        EnemyDeck = FindObjectOfType<EnemyDeck>();
        EnemyHand = GameObject.Find("Enemy Hand");
        EnemyDeck.drawCard(1, false);
        EnemyCard = FindObjectOfType<EnemyCardController>();
    }

    void Update()
    {
        if (Health > MaxHealth) { Health = MaxHealth; }
        if (Health <= 0)
        {
            if (Player.CurrentStage == 3)
            {
                SceneManager.LoadScene("Menu");
            } else
            {
                Player.CurrentStage += 1;
                MaxHealth *= 1.5f;
                Health = MaxHealth;
                EnemyTurn = false;
                if (Player.cardsInDeck > 0)
                {
                    PlayerDeck.drawCard(1);
                }
                if (Player.cardsInHand == 0)
                {
                    PlayerDeck.createDeck();
                    PlayerDeck.initialDraw();
                }
                return;
            }
        }

        if (EnemyTurn)
        {
            this.Shield = 0;
            var cardToUse = EnemyDeck.deck[0].cardName;
            Debug.Log("Card to use = " + cardToUse);
            int cardPower = EnemyDeck.deck[0].power;

            EnemyCard.PlayCard(cardToUse, cardPower);

            EnemyDeck.deck.RemoveAt(0);

            if (EnemyDeck.deck.Count == 0)
            {
                Destroy(EnemyHand.transform.GetChild(0).gameObject);
                EnemyDeck.createDeck();
                EnemyDeck.drawCard(1, false);
            } else if (EnemyDeck.deck.Count > 0)
            {
                Destroy(EnemyHand.transform.GetChild(0).gameObject);
                EnemyDeck.drawCard(1, false);
            }

            /*if (Player.Health <= 0)
            {
                SceneManager.LoadScene("Menu");
            }*/

            if (Player.cardsInDeck > 0)
            {
                PlayerDeck.drawCard(1);
            }
            if (Player.cardsInHand == 0)
            {
                PlayerDeck.createDeck();
                PlayerDeck.initialDraw();
            }
            EnemyTurn = false;
            Player.Shield = 0;
            }
    }
}
