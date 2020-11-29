﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Health;
    public float Shield;
    public float Mana;
    public float MaxHealth;
    public float MaxMana;
    public float ManaUseCost;
    public int cardsInDeck;
    public int cardsInHand;
    public GameObject Hand;
    public GameObject BattleZone;
    public GameObject PlayerShield;
    public int cardsUsed;

    private PlayerDeck PlayerDeck;
    private float shieldAfterBattle = 0f;
    public static int attackCardCount = 0;
    public static int blockCardCount = 0;
    private AnimationController AC;
    private Process Process;
    private GameController gameCtrlr;


    void Start()
    {
        Process = GameObject.FindGameObjectWithTag("Processing").GetComponent<Process>();
        gameCtrlr = FindObjectOfType<GameController>();

        cardsUsed = 0;
        if (Hand == null)
        {
            Hand = GameObject.Find("Hand");
        }
        PlayerDeck = FindObjectOfType<PlayerDeck>();
        AC = FindObjectOfType<AnimationController>();
        Health = MaxHealth;
        Mana = MaxMana;
        cardsInDeck = PlayerDeck.deck.Count;
    }

    void Update()
    {
        cardsInHand = Hand.transform.childCount;
        if (gameCtrlr.IsCardBeingDragged() == true)
        {
            cardsInHand++;
        }
        cardsInDeck = PlayerDeck.deck.Count;

        if (Health <= 20)
        {
            Process.ProcessScreen("lowHP");
        }
    }

    public PlayerDeck GetAssignedDeck()
    {
        return PlayerDeck;
    }

    public void Died()
    {
        Health = MaxHealth;
    }

    public void EndTurn()
    {
        if (cardsInDeck > 0)
        {
            PlayerDeck.DrawCard(1);
            FindObjectOfType<AudioManager>().Play("DealingCard");
        }

        if (cardsInHand == 0)
        {
            FindObjectOfType<AudioManager>().Play("ShufflingCards");
            PlayerDeck.CreateDeck();
            PlayerDeck.InitialDraw();
        }
    }

    public float StartBattle(float enemyHealth, ref float enemyShield)
    {
        CardController[] UI_Cards = BattleZone.transform.GetComponentsInChildren<CardController>();

        foreach (CardController UI_card in UI_Cards)
        {
            if (UI_card.Owner == Card.OwnerType.PLAYER)
            {
                var cardToUse = UI_card.Name;
                int cardPower = UI_card.Power;

                cardsUsed += 1;

                // Copy over our player shield to use and clear the playerShield as it gets used up
                float useEnemyShield = enemyShield;
                enemyShield = 0;

                // Is the player shield to use greater than card power?
                if (useEnemyShield.CompareTo(cardPower) > 0)
                {
                    // Have to reduce the shield so it can only use as much as the card power
                    useEnemyShield = cardPower;
                }

                if (UI_card.Enhanced)
                {
                    Mana -= ManaUseCost;
                    FindObjectOfType<AudioManager>().Play("Upgrade");
                    if (cardToUse == "Rifles")
                    {
                        // Hardcoded the extra damage to make it deal 25 when upgraded
                        enemyHealth -= (25 - (8 * 2)) - useEnemyShield;
                    }
                    Debug.Log("Card to use " + cardToUse + " is Enhanced, New Power is " + cardPower);
                    if (cardToUse == "Power Strike")
                    {
                        Debug.Log("You current Mana is " + Mana);

                        if ((Mana >= 25) && Mana <= 100)
                        {
                            Mana += 25;
                        }
                        Debug.Log("You healed Mana is " + Mana);

                    }
                }
                else
                {
                    Debug.Log("Card to use = " + cardToUse);
                }

                if (cardToUse == "Slash")
                {
                    enemyHealth -= cardPower - useEnemyShield;
                    FindObjectOfType<AudioManager>().Play("Sword");
                    AC.PlaySlash();

                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("Enemy now has " + enemyHealth + " health remaining");
                }
                else if (cardToUse == "Block")
                {
                    Shield += cardPower;
                    FindObjectOfType<AudioManager>().Play("CardFlipRepeat");
                    AC.PlayShield();
                    Debug.Log("You shielded for " + cardPower);
                }
                else if (cardToUse == "Vision")
                {
                    AC.PlayVision();
                    shieldAfterBattle += cardPower;
                    FindObjectOfType<AudioManager>().Play("CardFlipRepeat");
                    Debug.Log("You shield for next is " + cardPower);
                }
                else if (cardToUse == "Siphon")
                {
                    enemyHealth -= cardPower - useEnemyShield;
                    Health += cardPower;
                    FindObjectOfType<AudioManager>().Play("Sword");
                    AC.PlaySiphon();

                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("The enemy now has " + enemyHealth + " health remaining");
                    Debug.Log("You healed for" + cardPower + " health");
                }
                else if (cardToUse == "Dagger Throw")
                {
                    // +3 attack each time 
                    int damageIncrease = 3;
                    cardPower = (cardPower + (attackCardCount * damageIncrease));
                    enemyHealth -= cardPower - useEnemyShield;
                    attackCardCount++;
                    FindObjectOfType<AudioManager>().Play("Sword");
                    AC.PlayDaggerThrow();

                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("Enemy now has " + enemyHealth + " health remaining");
                }
                else if (cardToUse == "Saber Attack")
                {
                    enemyHealth -= cardPower - useEnemyShield;
                    Health -= 3;
                    FindObjectOfType<AudioManager>().Play("Sword");
                    AC.PlaySaberAttack();

                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("Enemy now has " + enemyHealth + " health remaining");
                }
                else if (cardToUse == "Retreat")
                {
                    int blockIncrease = 2;
                    cardPower = (cardPower + (blockCardCount * blockIncrease));
                    shieldAfterBattle += cardPower;
                    blockCardCount++;
                    FindObjectOfType<AudioManager>().Play("CardFlipRepeat");

                    Debug.Log("You shielded for " + cardPower);
                }
                else if (cardToUse == "Power Strike")
                {
                    enemyHealth -= cardPower - useEnemyShield;

                    FindObjectOfType<AudioManager>().Play("Sword");
                    AC.PlayPowerStrike();

                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("Enemy now has " + enemyHealth + " health remaining");
                }
                else if (cardToUse == "Pierce")
                {
                    enemyHealth -= cardPower;
                    FindObjectOfType<AudioManager>().Play("Sword");
                    AC.PlayPierce();
                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("The enemy now has " + enemyHealth + " health remaining");
                }
                else if (cardToUse == "Rifles")
                {
                    AC.PlayRifle();
                    enemyHealth -= cardPower - useEnemyShield;
                    FindObjectOfType<AudioManager>().Play("Pistol");

                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("The enemy now has " + enemyHealth + " health remaining");
                }
                else if (cardToUse == "Double Shot")
                {
                    AC.PlayDoubleShot();
                    cardPower *= 2;
                    enemyHealth -= cardPower - useEnemyShield;
                    FindObjectOfType<AudioManager>().Play("DoubleShot");
                  
                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("The enemy now has " + enemyHealth + " health remaining");
                }
                else if (cardToUse == "Heal")
                {
                    Health += cardPower;
                    FindObjectOfType<AudioManager>().Play("CardFlipRepeat");
                    AC.PlayHeal();
                    Debug.Log("You healed for" + cardPower + " health");
                }
                else if (cardToUse == "Anger")
                {
                    AC.PlayAnger();
                    FindObjectOfType<AudioManager>().Play("CardFlipRepeat");
                    Mana += cardPower;
                    Debug.Log("You healed for" + cardPower + " mana");
                }
                else
                {
                    // Can't find a battle tactic so card no used so restore mana we took away
                    if (UI_card.Enhanced)
                    {
                        Mana += ManaUseCost;
                    }
                    Debug.Log("PlayerController: Can't find a battle tactic to match " + cardToUse + " played");
                }
                if (Health > MaxHealth) Health = MaxHealth;
                if (Mana > MaxMana) Mana = MaxMana;
            }
        }

        // Update Player Shield if assigned
        if (PlayerShield != null)
        {
            PlayerShield.GetComponent<ShieldScript>().CurrentShield = Shield;
        }

        return enemyHealth;
    }

    public void EndBattle()
    {
        // Update the Shield with earnings from battle to apply on next turn
        Shield += shieldAfterBattle;
        shieldAfterBattle = 0f;
        if (PlayerShield != null)
        {
            PlayerShield.GetComponent<ShieldScript>().CurrentShield = Shield;
        }

        // Destroy all player owned cards from battle zone
        CardController[] UI_Cards = BattleZone.transform.GetComponentsInChildren<CardController>();
        
        foreach (CardController UI_card in UI_Cards)
        {
            if (UI_card.Owner == Card.OwnerType.PLAYER)
            {
                Destroy(UI_card.gameObject);
            }
        }
    }

    public void EndGame()
    {
        PlayerDeck.ClearRewardCards();
    }
}
