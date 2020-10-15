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
    public GameObject EnemyHand;
    public GameObject BattleZone;
    public GameObject EnemyShield;

    private EnemyDeck EnemyDeck;
    private float shieldAfterBattle = 0f;

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

    public float StartBattle(float playerHealth, ref float playerShield)
    {
        CardController[] UI_Cards = BattleZone.transform.GetComponentsInChildren<CardController>();

        foreach (CardController UI_card in UI_Cards)
        {
            if (UI_card.Owner == Card.OwnerType.ENEMY)
            {
                var cardToUse = UI_card.Name;
                Debug.Log("Card to use = " + cardToUse);
                int cardPower = UI_card.Power;

                // Copy over our plyare shield to use and clear the playerShield as it gets used up
                float usePlayerShield = playerShield;
                playerShield = 0;

                // Is the player shield to use greater than card power?
                if (usePlayerShield.CompareTo(cardPower) > 0)
                {
                    // Have to reduce the shield so it can only use as much as the card power
                    usePlayerShield = cardPower;
                }

                if (cardToUse == "Slash")
                {
                    playerHealth -= cardPower - usePlayerShield;
                    Debug.Log("Enemy dealt " + cardPower + " damage");
                    Debug.Log("You have " + playerHealth + " health remaining");
                }
                else if (cardToUse == "Block")
                {
                    shieldAfterBattle += cardPower;
                    Debug.Log("Enemy shielded for " + cardPower);
                }
                else if (cardToUse == "Siphon")
                {
                    playerHealth -= cardPower - usePlayerShield;
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

        // Update Player Shield if assigned
        if (EnemyShield != null)
        {
            EnemyShield.GetComponent<ShieldScript>().CurrentShield = Shield;
        }

        return playerHealth;
    }

    public void EndBattle()
    {
        // Update the Shield with earnings from battle to apply on next turn
        Shield += shieldAfterBattle;
        shieldAfterBattle = 0f;
        if (EnemyShield != null)
        {
            EnemyShield.GetComponent<ShieldScript>().CurrentShield = Shield;
        }

        // Destroy all enemy owned cards from battle zone
        CardController[] UI_Cards = BattleZone.transform.GetComponentsInChildren<CardController>();

        foreach (CardController UI_card in UI_Cards)
        {
            if (UI_card.Owner == Card.OwnerType.ENEMY)
            {
                Destroy(UI_card.gameObject);
            }
        }
    }
}
