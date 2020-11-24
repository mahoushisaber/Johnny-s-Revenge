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
    public Sprite Level1Stage1;
    public Sprite Level1Stage2;
    public Sprite Level1Stage3;
    public Sprite Level2Stage1;
    public Sprite Level2Stage2;
    public Sprite Level2Stage3;
    public Sprite Level3Stage1;
    public Sprite Level3Stage2;
    public Sprite Level3Stage3;
    public GameObject GameManager;

    private EnemyDeck EnemyDeck;
    private Sprite[] EnemySprites;
    private float shieldAfterBattle = 0f;
    // Counting number of times played for dagger and retreat 
    public static int attackCardCount = 0;
    public static int blockCardCount = 0;

    void Start()
    {
        EnemySprites = new Sprite[9];
        EnemySprites[0] = Level1Stage1;
        EnemySprites[1] = Level1Stage2;
        EnemySprites[2] = Level1Stage3;
        EnemySprites[3] = Level2Stage1;
        EnemySprites[4] = Level2Stage2;
        EnemySprites[5] = Level2Stage3;
        EnemySprites[6] = Level3Stage1;
        EnemySprites[7] = Level3Stage2;
        EnemySprites[8] = Level3Stage3;

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
    /*
    public void Died()
    {
        MaxHealth *= 1.5f;
        Health = MaxHealth;
        IsTurn = false;
    }
    */
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
                else if (cardToUse == "Dagger Throw")
                {
                    int damageIncrease = 3;
                    cardPower = (cardPower + (attackCardCount * damageIncrease));
                    playerHealth -= (cardPower) - usePlayerShield;
                    attackCardCount++;
                    Debug.Log("Enemy dealt " + cardPower + " damage");
                    Debug.Log("You have " + playerHealth + " health remaining");
                }
                else if (cardToUse == "Saber Attack")
                {
                    playerHealth -= cardPower - usePlayerShield;
                    Health -= 3;
                    Debug.Log("Enemy dealt " + cardPower + " damage");
                    Debug.Log("You have " + playerHealth + " health remaining");
                }
                else if (cardToUse == "Retreat")
                {
                    int blockIncrease = 2;
                    cardPower = (cardPower + (blockCardCount * blockIncrease));
                    shieldAfterBattle += cardPower;
                    blockCardCount++;
                    Debug.Log("Enemy shielded for " + cardPower);
                }
                else if (cardToUse == "Power Strike")
                {
                    playerHealth -= cardPower - usePlayerShield;
                    Debug.Log("Enemy dealt " + cardPower + " damage");
                    Debug.Log("You have " + playerHealth + " health remaining");
                }
                else if (cardToUse == "Pierce")
                {
                    playerHealth -= cardPower;
                    Debug.Log("Enemy dealt " + cardPower + " damage");
                    Debug.Log("You have " + playerHealth + " health remaining");
                }
                else if (cardToUse == "Heal")
                {
                    Health += cardPower;
                    Debug.Log("Enemy healed " + cardPower + " health");
                }
                else
                {
                    Debug.Log("EnemyController: Can't find a battle tactic to match " + cardToUse + " played");
                }
                if (Health > MaxHealth) Health = MaxHealth;
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

    public void setEnemy(int levelNum, int stageNum)
    {
        setEnemySprite(levelNum, stageNum);
        setEnemyDifficulty(levelNum, stageNum);
    }

    void setEnemySprite(int levelNum, int stageNum)
    {
        GameManager.GetComponent<SpriteRenderer>().sprite = EnemySprites[ stageNum - 1 + (levelNum - 1) * 3 ];
    }

    void setEnemyDifficulty(int levelNum, int stageNum)
    {
        MaxHealth = levelNum * 10 + (levelNum - 1) * 5 + stageNum*10;
        Health = levelNum * 10 + (levelNum - 1) * 5 + stageNum*10;
    }
}
