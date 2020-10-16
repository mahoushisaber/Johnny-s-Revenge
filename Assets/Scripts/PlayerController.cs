using System.Collections;
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

    private PlayerDeck PlayerDeck;
    private float shieldAfterBattle = 0f;
    public static int attackCardCount = 0;
    public static int blockCardCount = 0;

    void Start()
    {
        if (Hand == null)
        {
            Hand = GameObject.Find("Hand");
        }
        PlayerDeck = FindObjectOfType<PlayerDeck>();
        Health = MaxHealth;
        Mana = MaxMana;
        cardsInDeck = PlayerDeck.deck.Count;
    }

    void Update()
    {
        cardsInHand = Hand.transform.childCount;
        cardsInDeck = PlayerDeck.deck.Count;
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
                    Debug.Log("Card to use " + cardToUse + " is Enhanced, New Power is " + cardPower);
                }
                else
                {
                    Debug.Log("Card to use = " + cardToUse);
                }

                if (cardToUse == "Slash")
                {
                    enemyHealth -= cardPower - useEnemyShield;
                    FindObjectOfType<AudioManager>().Play("Sword");

                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("Enemy now has " + enemyHealth + " health remaining");
                }
                else if (cardToUse == "Block")
                {
                    shieldAfterBattle += cardPower;
                    FindObjectOfType<AudioManager>().Play("CardFlipRepeat");

                    Debug.Log("You shielded for " + cardPower);
                }
                else if (cardToUse == "Siphon")
                {
                    enemyHealth -= cardPower - useEnemyShield;
                    Health += cardPower;
                    FindObjectOfType<AudioManager>().Play("CardFlipRepeat");

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

                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("Enemy now has " + enemyHealth + " health remaining");
                }
                else if (cardToUse == "Saber Attack")
                {
                    enemyHealth -= cardPower - useEnemyShield;
                    Health -= 3;
                    FindObjectOfType<AudioManager>().Play("Sword");

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
                    if (Mana >= 50 && Mana <= 75)
                    {
                        Mana += 25;
                    }
                    FindObjectOfType<AudioManager>().Play("Sword");

                    Debug.Log("You dealt " + cardPower + " damage");
                    Debug.Log("Enemy now has " + enemyHealth + " health remaining");
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
}
