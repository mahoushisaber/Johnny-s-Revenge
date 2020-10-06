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
    public GameObject EnemyHand;
    void Start()
    {
        Health = MaxHealth;
        Player = FindObjectOfType<PlayerController>();
        PlayerDeck = FindObjectOfType<PlayerDeck>();
        EnemyDeck = FindObjectOfType<EnemyDeck>();
        EnemyHand = GameObject.Find("Enemy Hand");
        EnemyDeck.drawCard(1, false);
    }

    void Update()
    {
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
            if (EnemyDeck.deck.Count <= 0)
            {
                EnemyDeck.createDeck();
            }

            var cardToUse = EnemyDeck.deck[0].cardName;
            Debug.Log("Card to use = " + cardToUse);
            int cardPower = EnemyDeck.deck[0].power;

            if (cardToUse == "Slash")
            {
                Player.Health -= cardPower;
                Debug.Log("Enemy dealt " + cardPower + " damage");
                Debug.Log("You have " + Player.Health + " health remaining");
            }
            else if (cardToUse == "Block")
            {
                this.Shield += cardPower;
                Debug.Log("Enemy shielded for " + cardPower);
            }
            else if (cardToUse == "Siphon")
            {
                Player.Health -= cardPower;
                this.Health += cardPower;
                Debug.Log("Enemy dealt " + cardPower + " damage");
                Debug.Log("You have " + Player.Health + " health remaining");
                Debug.Log("Enemy healed " + cardPower + " health");
            }
            EnemyDeck.drawCard(1, true);
            Destroy(EnemyHand.transform.GetChild(0).gameObject);

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
            }
    }
}
