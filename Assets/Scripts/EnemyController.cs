using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public float Shield;
    public bool IsTurn = false;
    EnemyDeck EnemyDeck;
    public GameObject EnemyHand;
    public GameObject BattleZone;


    void Start()
    {
        Health = MaxHealth;
        if (EnemyHand == null)
        {
            EnemyHand = GameObject.Find("Enemy Hand");
        }
        EnemyDeck = FindObjectOfType<EnemyDeck>();
        EnemyDeck.DrawCard(1);
    }

    void Update()
    {
        // Any animations or other stuff specific to Enemy controller
        ;
    }

    public void Died()
    {
        MaxHealth *= 1.5f;
        Health = MaxHealth;
        IsTurn = false;
    }

    public void StartTurn()
    {
        IsTurn = EnemyDeck.PlayCard(1, BattleZone); 
    }

    public void EndTurn()
    {
        if (EnemyDeck.deck.Count <= 0)
        {
            EnemyDeck.CreateDeck();
        }

        // TODO : Don't draw card if one already there!
        EnemyDeck.DrawCard(1);

        IsTurn = false;
    }

    public float StartBattle(float playerHealth)
    {
        CardController[] UI_Cards = BattleZone.transform.GetComponentsInChildren<CardController>();

        foreach (CardController UI_card in UI_Cards)
        {
            if (UI_card.ThisCard.cardOwner == Card.OwnerType.ENEMY)
            {
                var cardToUse = UI_card.CardName;
                Debug.Log("Card to use = " + cardToUse);
                int cardPower = UI_card.Power;

                if (cardToUse == "Slash")
                {
                    playerHealth -= cardPower;
                    Debug.Log("Enemy dealt " + cardPower + " damage");
                    Debug.Log("You have " + playerHealth + " health remaining");
                }
                else if (cardToUse == "Block")
                {
                    Shield += cardPower;
                    Debug.Log("Enemy shielded for " + cardPower);
                }
                else if (cardToUse == "Siphon")
                {
                    playerHealth -= cardPower;
                    Health += cardPower;
                    Debug.Log("Enemy dealt " + cardPower + " damage");
                    Debug.Log("You have " + playerHealth + " health remaining");
                    Debug.Log("Enemy healed " + cardPower + " health");
                }
                else
                {
                    Debug.Log("EnemyController: Can't find a battle tactic to match " + cardToUse + " played");
                }
            }
        }

        return playerHealth;
    }

    public void EndBattle()
    {
        // Destroy all enemy owned cards from battle zone
        CardController[] UI_Cards = BattleZone.transform.GetComponentsInChildren<CardController>();

        foreach (CardController UI_card in UI_Cards)
        {
            if (UI_card.ThisCard.cardOwner == Card.OwnerType.ENEMY)
            {
                Destroy(UI_card.gameObject);
            }
        }
    }
}
